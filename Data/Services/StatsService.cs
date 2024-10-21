using inventarioApi.Data.Models;
using InventarioApi;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace inventarioApi.Data.Services
{
    public class StatsService
    {
        private readonly InventarioContext _context;
        public StatsService(InventarioContext context)
        {
            _context = context;
        }

        //GET
        public async Task<Sales> GetSalesAndInflowsStatistics()
        {
            //Date ranges
            var endDate = DateTimeOffset.UtcNow.AddHours(-5);
            var startDate = endDate.AddDays(-39);
            var transactions = await _context.Transactions
                .Where(t => t.Date >= startDate && t.Date <= endDate)
                .ToListAsync();
            var dateRange = Enumerable.Range(0, 40)
                .Select(offset => startDate.AddDays(offset).Date)
                .ToList();

            //Get the total value of sales and incomes
            var salesValues = dateRange
                .Select(d => transactions
                    .Where(t => t.Date.Date == d && t.Type == TransactionType.OUTPUT)
                    .Sum(t => t.Value))
                .ToArray();
            var inflowValues = dateRange
                .Select(d => transactions
                    .Where(t => t.Date.Date == d && t.Type == TransactionType.INCOME)
                    .Sum(t => t.Value))
                .ToArray();
            //Set formated dates
            var dates = dateRange
            .Select(d => FormatDateInSpanish(d))
            .ToArray();

            return new Sales
            {
                date = dates,
                income = inflowValues,
                outflow = salesValues
            };
        }

        public async Task<List<Coverage>> GetStockCoverage()
        {
            //Date ranges
            var endDate = DateTimeOffset.UtcNow.AddHours(-5);
            var startDate = endDate.AddDays(-30);

            //Get transactions
            var transactions = await _context.Transactions
                .Where(t => t.Date >= startDate && t.Date <= endDate && t.Type == TransactionType.OUTPUT)
                .Include(t => t.TransactionDetail)
                .ToListAsync();

            //Get transaction details from transactions
            var transactionDetails = transactions
                .SelectMany(t => t.TransactionDetail)
                .Where(t => t.Detail == false)
                .ToList();

            //Get presentation sales
            var presentationOutflows = transactionDetails
                .GroupBy(td => td.Presentation)
                .Select(g => new
                {
                    PresentationId = g.Key,
                    TotalQuantity = g.Sum(td => td.Quantity)
                })
                .ToList();

            var presentationIds = presentationOutflows.Select(po => po.PresentationId).ToList();

            //Get the selected presentations
            var presentations = await _context.Presentations
                .Where(p => presentationIds.Contains(p.IdPresentation))
                .Include(p => p.Products)
                .ToListAsync();

            var coverageList = presentationOutflows
                .Select(po =>
                {
                    var presentation = presentations.FirstOrDefault(p => p.IdPresentation == po.PresentationId);
                    if (presentation != null)
                    {
                        var daysOfCoverage = po.TotalQuantity == 0 ? 0 : (presentation.Stock / po.TotalQuantity);
                        //Build each presentation coverage
                        return new Coverage
                        {
                            product = presentation.Products?.Name ?? "Desconocido",
                            presentation = presentation.Name,
                            days = daysOfCoverage,
                            color = AsignColor(daysOfCoverage)
                        };
                    }
                    return null;
                })
                .Where(c => c != null)
                .ToList();

            return coverageList;
        }

        /*public async Task<List<Coverage>> GetStockCoverage()
        {
            //Date ranges
            var endDate = DateTimeOffset.UtcNow.AddHours(-5);
            var startDate = endDate.AddDays(-30);

            //Get transactions
            var transactions = await _context.Transactions
                .Where(t => t.Date >= startDate && t.Date <= endDate && t.Type == TransactionType.OUTPUT)
                .Include(t => t.TransactionDetail)
                .ToListAsync();

            //Get transaction details from transactions
            var transactionDetails = transactions
                .SelectMany(t => t.TransactionDetail)
                .ToList();

            //Get presentation sales
            var presentationOutflows = transactionDetails
                .GroupBy(td => td.Presentation)
                .Select(g => new
                {
                    PresentationId = g.Key,
                    TotalQuantity = g.Sum(td => td.Quantity)
                })
                .ToList();

            var presentationIds = presentationOutflows.Select(po => po.PresentationId).ToList();

            //Get the selected presentations
            var presentations = await _context.Presentations
                .Where(p => presentationIds.Contains(p.IdPresentation))
                .Include(p => p.Products)
                .ToListAsync();

            var coverageList = presentationOutflows
                .Select(po =>
                {
                    var presentation = presentations.FirstOrDefault(p => p.IdPresentation == po.PresentationId);
                    if (presentation != null)
                    {
                        var daysOfCoverage = po.TotalQuantity == 0 ? 0 : (presentation.Stock / po.TotalQuantity);
                        //Build each presentation coverage
                        return new Coverage
                        {
                            product = presentation.Products?.Name ?? "Desconocido",
                            presentation = presentation.Name,
                            days = daysOfCoverage,
                            color = AsignColor(daysOfCoverage)
                        };
                    }
                    return null;
                })
                .Where(c => c != null)
                .ToList();

            return coverageList;
        }*/


        private string FormatDateInSpanish(DateTime date)
        {
            var months = new[]
            {
                "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio",
                "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"
            };

            var day = date.Day.ToString("00");
            var month = months[date.Month - 1];
            return $"{day} de {month}";
        }

        private string AsignColor(float days)
        {
            return days switch
            {
                < 1 => "red-500",
                >= 1 and < 5 => "red-300",
                >= 5 and < 9 => "yellow-500",
                >= 9 and < 14 => "green-500",
                _ => "green-800"
            };
        }
    }
}
