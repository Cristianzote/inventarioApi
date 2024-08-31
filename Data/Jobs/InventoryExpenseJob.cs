using inventarioApi.Data.Models;
using inventarioApi.Data.Services;
using InventarioApi;
using Quartz;
using System.Threading.Tasks;

namespace inventarioApi.Data.Jobs
{
    public class InventoryExpenseJob : IJob
    {
        private readonly ExpenseService _expenceService;
        private readonly TransactionService _transactionService;
        private readonly ProductService _productService;
        private readonly MonthlyRegisterService _monthlyRegisterService;

        public InventoryExpenseJob(ExpenseService expenseService, TransactionService transactionService, ProductService productService, MonthlyRegisterService monthlyRegisterService)
        {
            _expenceService = expenseService;
            _transactionService = transactionService;
            _productService = productService;
            _monthlyRegisterService = monthlyRegisterService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            //Create register
            var monthlyRegisterEntity = new MonthlyRegister
            {
                InitialDate = DateTime.UtcNow.AddMonths(-1).AddDays(1),
                FinalDate = DateTime.UtcNow,
                InitialInventory = 0,
                FinalInventory = 0,
                Purchases = 0,
                Expenses = 0,
            };

            //Initial inventory
            var monthlyRegisters = await _monthlyRegisterService.GetMonthlyRegisters();
            if (monthlyRegisters.Any())
            {
                monthlyRegisterEntity.InitialInventory = monthlyRegisters.LastOrDefault().FinalInventory;
                DateTime dateTime = monthlyRegisters.LastOrDefault().FinalDate.Date.ToUniversalTime();
                monthlyRegisterEntity.InitialDate = dateTime.AddDays(1);
            }

            //Final inventory
            var products = await _productService.GetProducts();
            foreach (var product in products)
            {
                foreach (var presentation in product.Presentations)
                {
                    monthlyRegisterEntity.FinalInventory += (int)(presentation.PriceIncome * presentation.Stock);
                }
            }

            //Purchases
            var transactions = await _transactionService.GetTransactions();
            transactions = transactions
                .Where(t => t.Date >= monthlyRegisterEntity.InitialDate)
                .Where(t => t.Date <= monthlyRegisterEntity.FinalDate)
                .ToList();
            foreach (var t in transactions)
            {
                if (t.Type == TransactionType.INCOME)
                {
                    monthlyRegisterEntity.Purchases += (int)t.Value;
                }
            }

            //Expenses
            var expenses = await _expenceService.GetExpenses();
            expenses = expenses
                .Where(e => e.Active == true)
                .ToList();
            foreach (var expense in expenses)
            {
                monthlyRegisterEntity.Expenses += (int)(expense.Value * expense.Multiplier);
            }

            //Create register
            var register = await _monthlyRegisterService.CreateMonthlyRegister(monthlyRegisterEntity);

            //Create register - expense relation
            foreach (var expense in expenses)
            {
                await _monthlyRegisterService.CreateMonthlyExpence(new MonthlyExpense
                {
                    Expense = expense.IdExpense,
                    MonthlyRegister = register.IdMonthlyRegister
                });

                if (expense.Type == ExpenceType.OCCASIONAL)
                {
                    expense.Active = false;
                    await _expenceService.UpdateExpense(expense);
                }
            }
        }
    }
}
