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

            var TransactionEntity = new Transaction
            {
                Value = TRANSACTION.Value,
                Type = TRANSACTION.Type,
                Date = DateTimeOffset.UtcNow,
                TransactionDetail = TRANSACTION.TransactionDetail
            };

            await _context.Transactions.AddAsync(TransactionEntity);

            try
            {
                await _context.SaveChangesAsync();
                int newTransactionId = TransactionEntity.IdTransaction;

                //Create detail on presentation
                foreach (TransactionDetail transactionDetails in TransactionEntity.TransactionDetail)
                {
                    var TransactionDetailEntity = new TransactionDetail
                    {
                        Detail = transactionDetails.Detail,
                        Quantity = transactionDetails.Quantity,
                        Presentation = transactionDetails.Presentation,
                        Transaction = transactionDetails.Transaction
                    };

                    //Presentation stock ajustment
                    var presentationEntity = await _context.Presentations.FindAsync(transactionDetails.Presentation);
                    switch (TransactionEntity.Type)
                    {
                        case TransactionType.INCOME:
                            presentationEntity.Stock += TransactionDetailEntity.Quantity;
                        break;
                        
                        case TransactionType.OUTPUT:
                            //Detail sale?
                            if (presentationEntity.RetailStockRatio! == 1)
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
                    await _context.TransactionDetails.AddAsync(transactionDetails);
                }

                //var createUserInventory = await _context.UserInventories.AddAsync(userInventoryEntity);
                //await _context.SaveChangesAsync();
                return TransactionEntity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
