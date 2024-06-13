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
            var endDate = DateTimeOffset.UtcNow.AddHours(-5); // Adjust for time zone
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
