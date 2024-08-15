using inventarioApi.Data.Models;
using InventarioApi;
using Microsoft.EntityFrameworkCore;

namespace inventarioApi.Data.Services
{
    public class ExpenseService
    {
        private readonly InventarioContext _context;
        public ExpenseService(InventarioContext context)
        {
            _context = context;
        }

        public async Task<List<Expense>> GetExpenses()
        {
            var result = await _context.Expences
            .ToListAsync();

            List<Transaction> transaction = _context.Transactions.ToList();

            if (result != null)
            {
                return result;
            }
            else
            {
                throw new Exception("Null value");
            }
        }

        public async Task<Expense> CreateExpense(Expense EXPENCE)
        {
            var ExpenceEntity = new Expense
            {
                Value = EXPENCE.Value,
                Name = EXPENCE.Name,
                Description = EXPENCE.Description,
                Type = EXPENCE.Type,
                Active = EXPENCE.Active,
                Multiplier = EXPENCE.Multiplier
            };

            await _context.Expences.AddAsync(ExpenceEntity);
            await _context.SaveChangesAsync();
            return ExpenceEntity;
        }

        public async Task<Expense> UpdateExpense(Expense EXPENCE)
        {
            var existingeExpense = await _context.Expences
            .Where(e => e.IdExpences == EXPENCE.IdExpences)
            .FirstAsync();

            if (existingeExpense == null)
            {
                throw new Exception("Null value");
            }

            existingeExpense.Value = EXPENCE.Value;
            existingeExpense.Name = EXPENCE.Name;
            existingeExpense.Description = EXPENCE.Description;
            existingeExpense.Type = EXPENCE.Type;
            existingeExpense.Active = EXPENCE.Active;
            existingeExpense.Multiplier = EXPENCE.Multiplier;

            _context.Expences.Update(existingeExpense);
            await _context.SaveChangesAsync();
            return existingeExpense;
        }
    }
}
