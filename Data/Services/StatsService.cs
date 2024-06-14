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

        public async Task<Sales> GetSalesAndInflowsStatistics()
        {
            var endDate = DateTimeOffset.UtcNow.AddHours(-5);
            var startDate = endDate.AddDays(-39);

            var transactions = await _context.Transactions
                .Where(t => t.Date >= startDate && t.Date <= endDate)
                .ToListAsync();

            var dateRange = Enumerable.Range(0, 40)
                .Select(offset => startDate.AddDays(offset).Date)
                .ToList();

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
            var endDate = DateTimeOffset.UtcNow.AddHours(-5); // Adjust for time zone
            var startDate = endDate.AddDays(-30);

            // Retrieve transactions within the specified date range and of type OUTPUT
            var transactions = await _context.Transactions
                .Where(t => t.Date >= startDate && t.Date <= endDate && t.Type == TransactionType.OUTPUT)
                .Include(t => t.TransactionDetail) // Include the transaction details
                .ToListAsync();

            // Extract transaction details from the filtered transactions
            var transactionDetails = transactions
                .SelectMany(t => t.TransactionDetail)
                .ToList();

            // Group transaction details by presentation ID and calculate total quantities
            var presentationOutflows = transactionDetails
                .GroupBy(td => td.Presentation)
                .Select(g => new
                {
                    PresentationId = g.Key,
                    TotalQuantity = g.Sum(td => td.Quantity)
                })
                .ToList();

            // Get the list of presentation IDs
            var presentationIds = presentationOutflows.Select(po => po.PresentationId).ToList();

            // Fetch the presentations with their current stock and related product information
            var presentations = await _context.Presentations
                .Where(p => presentationIds.Contains(p.IdPresentation))
                .Include(p => p.Products)
                .ToListAsync();

            // Calculate stock coverage for each presentation
            var coverageList = presentationOutflows
                .Select(po =>
                {
                    var presentation = presentations.FirstOrDefault(p => p.IdPresentation == po.PresentationId);
                    if (presentation != null)
                    {
                        var daysOfCoverage = po.TotalQuantity == 0 ? 0 : (presentation.Stock / po.TotalQuantity);
                        return new Coverage
                        {
                            product = presentation.Products?.Name ?? "Unknown",
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
                < 1 => "red-700",
                >= 1 and < 5 => "red-500",
                >= 5 and < 9 => "yellow-500",
                >= 9 and < 14 => "green-500",
                _ => "green-800"
            };
        }
    }
}
