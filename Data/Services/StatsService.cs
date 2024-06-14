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

            // Retrieve transaction details within the last 31 days for transactions of type OUTPUT
            var transactionDetails = await _context.TransactionDetails
                .Where(td => td.Transactions.Date >= startDate && td.Transactions.Date <= endDate && td.Transactions.Type == TransactionType.OUTPUT)
                .ToListAsync();

            // Group transaction details by presentation and calculate total quantities
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

            // Fetch the presentations with their current stock
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
                        var daysOfCoverage = po.TotalQuantity == 0 ? "N/A" : (presentation.Stock / (float)po.TotalQuantity).ToString("F2");
                        return new Coverage
                        {
                            product = presentation.Products?.Name ?? "Unknown",
                            presentation = presentation.Name,
                            days = daysOfCoverage
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

    }
}
