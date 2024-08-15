using inventarioApi.Data.Models;
using InventarioApi;
using Microsoft.EntityFrameworkCore;

namespace inventarioApi.Data.Services
{
    public class MonthlyRegisterService
    {
        private readonly InventarioContext _context;
        public MonthlyRegisterService(InventarioContext context)
        {
            _context = context;
        }

        public async Task<List<MonthlyRegister>> GetMonthlyRegisters()
        {
            var result = await _context.MonthlyRegister.ToListAsync();

            if (result != null)
            {
                return result;
            }
            else
            {
                throw new Exception("Null value");
            }
        }

        public async Task<List<MonthlyExpence>> GetMonthlyExpences(int ID_MONTHLY_REGISTER)
        {
            var result = await _context.MonthlyExpences
                .Where(me => me.MonthlyRegister == ID_MONTHLY_REGISTER)
                .ToListAsync();

            if (result != null)
            {
                return result;
            }
            else
            {
                throw new Exception("Null value");
            }
        }

        public async Task<MonthlyRegister> CreateMonthlyRegister(MonthlyRegister MONTHLY_REGISTER)
        {
            var MonthlyRegisterEntity = new MonthlyRegister
            {
                InitialDate = MONTHLY_REGISTER.InitialDate,
                FinalDate = MONTHLY_REGISTER.FinalDate,
                InitialInventory = MONTHLY_REGISTER.InitialInventory,
                FinalInventory = MONTHLY_REGISTER.FinalInventory,
                Purchases = MONTHLY_REGISTER.Purchases,
                Expenses = MONTHLY_REGISTER.Expenses,
            };

            await _context.MonthlyRegister.AddAsync(MonthlyRegisterEntity);
            await _context.SaveChangesAsync();
            return MonthlyRegisterEntity;
        }

        public async Task<MonthlyExpence> CreateMonthlyExpence(MonthlyExpence MONTHLY_EXPENCE)
        {
            var MonthlyExpenseEntity = new MonthlyExpence
            {
                Expense = MONTHLY_EXPENCE.Expense,
                MonthlyRegister = MONTHLY_EXPENCE.MonthlyRegister
            };

            await _context.MonthlyExpences.AddAsync(MonthlyExpenseEntity);
            await _context.SaveChangesAsync();
            return MonthlyExpenseEntity;
        }


        public async Task<MonthlyRegister> UpdateMonthlyRegister(MonthlyRegister MONTHLY_REGISTER)
        {
            var MonthlyRegisterEntity = new MonthlyRegister
            {
                InitialDate = MONTHLY_REGISTER.InitialDate,
                FinalDate = MONTHLY_REGISTER.FinalDate,
                InitialInventory = MONTHLY_REGISTER.InitialInventory,
                FinalInventory = MONTHLY_REGISTER.FinalInventory,
                Purchases = MONTHLY_REGISTER.Purchases,
                Expenses = MONTHLY_REGISTER.Expenses,
            };

            _context.MonthlyRegister.Update(MonthlyRegisterEntity);
            await _context.SaveChangesAsync();
            return MonthlyRegisterEntity;
        }
    }
}
