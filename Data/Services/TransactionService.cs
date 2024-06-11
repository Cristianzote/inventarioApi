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

        //POST
        public async Task<Transaction> CreateTransaction(Transaction TRANSACTION)
        {
            // Create a new Transaction entity
            var TransactionEntity = new Transaction
            {
                Value = TRANSACTION.Value,
                Type = TRANSACTION.Type,
                Date = DateTimeOffset.UtcNow,
                // Initialize an empty list for TransactionDetail to avoid tracking issues
                TransactionDetail = new List<TransactionDetail>()
            };

            // Add the new Transaction entity to the context
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
                        Transaction = newTransactionId
                    };

                    // Adjust stock based on the transaction type
                    var presentationEntity = await _context.Presentations.FindAsync(transactionDetail.Presentation);
                    switch (TransactionEntity.Type)
                    {
                        case TransactionType.INCOME:
                            presentationEntity.Stock += TransactionDetailEntity.Quantity;
                            break;

                        case TransactionType.OUTPUT:
                            // Adjust stock based on retail stock ratio
                            if (presentationEntity.RetailStockRatio == 1)
                            {
                                presentationEntity.Stock -= TransactionDetailEntity.Quantity;
                            }
                            else
                            {
                                if (presentationEntity.RetailStock - TransactionDetailEntity.Quantity > 0)
                                {
                                    float reducedStock = (TransactionDetailEntity.Quantity / presentationEntity.RetailStock);
                                    presentationEntity.Stock -= (int)reducedStock;

                                    int reducedRetailStock = (TransactionDetailEntity.Quantity % presentationEntity.RetailStock) * presentationEntity.RetailStockRatio;
                                    if (reducedRetailStock < presentationEntity.RetailStock)
                                    {
                                        presentationEntity.RetailStock -= reducedRetailStock;
                                    }
                                    else
                                    {
                                        presentationEntity.RetailStock -= (presentationEntity.RetailStockRatio - reducedRetailStock);
                                    }
                                }
                                else
                                {
                                    presentationEntity.RetailStock -= TransactionDetailEntity.Quantity;
                                }
                            }
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
