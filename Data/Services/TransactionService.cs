using inventarioApi.Data.Models;
using InventarioApi;
using Microsoft.EntityFrameworkCore;

namespace inventarioApi.Data.Services
{
    public class TransactionService
    {
        private readonly InventarioContext _context;
        public TransactionService(InventarioContext context)
        {
            _context = context;
        }

        //GET
        public async Task<List<Transaction>> GetTransactions()
        {
            var result = await _context.Transactions
            .Include(t => t.TransactionDetail)
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
        public async Task<Transaction> GetTransactionById(int id)
        {
            var result = await _context.Transactions
            .Include(t => t.TransactionDetail)
            .Where(t => t.IdTransaction == id)
            .FirstOrDefaultAsync();

            if (result != null)
            {
                return result;
            }
            else
            {
                throw new Exception("Null value");
            }
        }

        public async Task<Transaction> GetBill(int ID)
        {
            var transaction = await _context.Transactions
                .Include(t => t.TransactionDetail)
                .Where(t => t.IdTransaction == ID)
                .Select(t => new Transaction()
                {
                    Date = t.Date,
                    IdTransaction = t.IdTransaction,
                    Table = t.Table,
                    Type = t.Type,
                    Cover = t.Cover,
                    Value = t.Value,
                    TransactionDetail = t.TransactionDetail

                })
            .FirstOrDefaultAsync();

            return transaction;
        }

        //POST
        public async Task<Transaction> CreateTransaction(Transaction TRANSACTION)
        {
            // Create a new Transaction entity
            var TransactionEntity = new Transaction
            {
                Value = TRANSACTION.Value,
                Type = TRANSACTION.Type,
                Table = TRANSACTION.Table,
                Cover = TRANSACTION.Cover,
                Date = DateTimeOffset.UtcNow.AddHours(-5),
                TransactionDetail = new List<TransactionDetail>()
            };

            await _context.Transactions.AddAsync(TransactionEntity);

            try
            {
                // Save changes to get the new Transaction ID
                await _context.SaveChangesAsync();
                int newTransactionId = TransactionEntity.IdTransaction;

                // Iterate over the details from the input transaction
                foreach (var transactionDetail in TRANSACTION.TransactionDetail)
                {
                    // Create a new TransactionDetail entity for each detail
                    var TransactionDetailEntity = new TransactionDetail
                    {
                        Detail = transactionDetail.Detail,
                        Quantity = transactionDetail.Quantity,
                        Presentation = transactionDetail.Presentation,
                        Transaction = newTransactionId,
                        Discounting = transactionDetail.Discounting,
                    };

                    // Adjust stock based on the transaction type
                    var presentationEntity = await _context.Presentations.FindAsync(transactionDetail.Presentation);

                    if (presentationEntity == null) 
                    {
                        throw new Exception("Null Presentation");
                    }
                    switch (TransactionEntity.Type)
                    {
                        case TransactionType.INCOME:
                            presentationEntity.Stock += TransactionDetailEntity.Quantity;
                            break;

                        case TransactionType.OUTPUT:
                            if(TransactionDetailEntity.Detail == false)
                            {
                                presentationEntity.Stock -= TransactionDetailEntity.Quantity;
                            }
                            else if(presentationEntity.HasRetail)
                            {
                                presentationEntity.Stock -= TransactionDetailEntity.Discounting;
                            }
                            /*if (presentationEntity.RetailStockRatio <= 1)
                            {
                                presentationEntity.Stock -= TransactionDetailEntity.Quantity;
                            }
                            else
                            {
                                if (presentationEntity.RetailStock - TransactionDetailEntity.Quantity <= 0)
                                {
                                    //Adjust stock and retail stock
                                    int Quantity = TransactionDetailEntity.Quantity;
                                    int RetailStock = presentationEntity.RetailStock;
                                    int ReducedRetailStock = RetailStock - Quantity;
                                    int ReducedStock = 0;

                                    while (ReducedRetailStock <= 0)
                                    {
                                        var localStock = presentationEntity.RetailStockRatio;
                                        ReducedRetailStock += localStock;
                                        ReducedStock += 1;
                                    }

                                    presentationEntity.RetailStock = ReducedRetailStock;
                                    presentationEntity.Stock -= ReducedStock;
                                }
                                else
                                {
                                    presentationEntity.RetailStock -= TransactionDetailEntity.Quantity;
                                }
                            }*/
                            break;
                    }

                    // Add the new TransactionDetail entity to the context
                    await _context.TransactionDetails.AddAsync(TransactionDetailEntity);
                    // Also add it to the TransactionEntity's details list
                    TransactionEntity.TransactionDetail.Add(TransactionDetailEntity);
                }

                // Save all changes
                await _context.SaveChangesAsync();
                return TransactionEntity;
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                throw new Exception("Error creating transaction", ex);
            }
        }

    }
}
