using inventarioApi.Data.Models;
using inventarioApi.Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestPDF;
using QuestPDF.Helpers;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using static System.Net.Mime.MediaTypeNames;
using Humanizer;
using static QuestPDF.Helpers.Colors;
using System.Net.Mail;

namespace inventarioApi.Controllers
{
    [ApiController]
    [Route("/api/v1/transaction")]
    public class TransactionController : ControllerBase
    {
        private readonly TransactionService _transactionService;
        private readonly ProductService _productService;
        private readonly Message _message;

        public TransactionController(TransactionService transactionService, ProductService productService, Message message)
        {
            _transactionService = transactionService;
            _productService = productService;
            _message = message;
        }

        //GET
        [HttpGet()]
        [AllowAnonymous]
        public async Task<ActionResult<Inventory>> GetTransaction()
        {
            var transactions = await _transactionService.GetTransactions();

            if (transactions == null)
            {
                return NotFound();
            }
            return Ok(transactions);
        }

        [HttpGet("bill/{ID}")]
        [AllowAnonymous]
        public async Task<IResult> GetBill(int ID)
        {
            Transaction transaction = await _transactionService.GetBill(ID);
            List<Product> products = await _productService.GetProducts();

            if (transaction == null)
            {
                return Results.NotFound("Transaction not found");
            }
            if (products == null)
            {
                return Results.NotFound("Products not found");
            }
            try
            {
                // use any method to create a document, e.g.: injected service
                var document = CreateDocument(transaction, products);

                // generate PDF file and return it as a response
                var pdf = document.GeneratePdf();
                return Results.File(pdf, "application/pdf", $"Factura Atlenal {transaction.IdTransaction} {transaction.Date.Day}-{transaction.Date.Month}-{transaction.Date.Year}.pdf");
            }
            catch (Exception e)
            {
                // Log the exception details
                Console.Error.WriteLine($"Error generating PDF: {e.Message}");
                Console.Error.WriteLine(e.StackTrace);

                // Return a detailed error response for debugging
                return Results.Problem(detail: e.Message, title: "Error generating PDF", statusCode: 500);
            }
        }

        QuestPDF.Infrastructure.IDocument CreateDocument(Transaction transaction, List<Product> products)
        {
            var transactionInstance = new Transaction
            {
                Cover = transaction.Cover,
                Date = transaction.Date,
                TransactionDetail = transaction.TransactionDetail,
                Table = transaction.Table,
                Type = transaction.Type,
                IdTransaction = transaction.IdTransaction,
                Value = transaction.Value
            };

            var ProductIntance = new Product 
            {
                Name = "",
                Description = "",
                Image = ""
            };

            var PresentationInstance = new Presentation { Description = "", Name = "" };

            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(1, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));
                    page.Background().Image("Views/img/BillBG.png", ImageScaling.FitArea);

                    page.Header()
                        .Row(row =>
                        {
                            row.RelativeItem().Column(col => {});
                            row.RelativeItem().Column(col => {});
                            row.RelativeItem().Column(col => {});
                            row.RelativeItem().Column(col => {});
                            row.RelativeItem().Column(col => {});
                            row.RelativeItem().Column(col => {});
                            row.RelativeItem().Column(col => {});
                            row.RelativeItem().Column(col => {});
                            row.RelativeItem().Column(col => {});
                            //row.ConstantItem(150).Height(150).Svg(SvgImage.FromFile("Views/img/appiconfg.svg"));
                            row.ConstantItem(150).Height(150).Svg(SvgImage.FromFile("Views/img/appiconfg.svg"));
                            row.RelativeItem().Column(col => {});
                            row.RelativeItem().Column(col => {});
                        });

                    page.Content()
                        .Column(column =>
                        {
                            //Info
                            column.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                });

                                table.Cell().Element(CellStyle).Text("ID de venta:");
                                table.Cell().Element(CellStyle).Text(transaction.IdTransaction.ToString());

                                table.Cell().Element(CellStyle).Text("Mesa:");
                                table.Cell().Element(CellStyle).Text(transaction.Table.ToString().Pascalize());

                                table.Cell().Element(CellStyle).Text("Cover:");
                                table.Cell().Element(CellStyle).Text(transaction.Cover ? "Sí" : "No");

                                table.Cell().Element(CellStyle).Text("Fecha:");
                                table.Cell().Element(CellStyle).Text(transaction.Date.UtcDateTime.ToString().Split(" ")[0]);

                                table.Cell().Element(CellStyle).Text("Hora:");
                                table.Cell().Element(CellStyle).Text(transaction.Date.UtcDateTime.ToString().Split(" ")[1]);

                                static IContainer CellStyle(IContainer container)
                                {
                                    return container.DefaultTextStyle(x => x.FontSize(12)).BorderBottom(1).BorderColor("#22c55e").Padding(5);
                                }

                            });
                            //Padding
                            column.Item().PaddingVertical(10);
                            //Presentations
                            column.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Element(SmallCellStyle).Text("Producto");
                                    header.Cell().Element(SmallCellStyle).Text("Presentación");
                                    header.Cell().Element(BigCellStyle).Text("Trago / Menudeado");
                                    header.Cell().Element(SmallCellStyle).Text("Cantidad");
                                    header.Cell().Element(BigCellStyle).Text("Precio individual");
                                    header.Cell().Element(SmallCellStyle).Text("Precio total");

                                    static IContainer BigCellStyle(IContainer container)
                                    {
                                        return container
                                        .DefaultTextStyle(x => x.FontSize(12).Bold())
                                        .AlignMiddle().Background("#22c55e").BorderColor("#22c55e").PaddingVertical(2).PaddingLeft(5);
                                    }
                                    static IContainer SmallCellStyle(IContainer container)
                                    {
                                        return container
                                        .DefaultTextStyle(x => x.FontSize(12).Bold())
                                        .AlignMiddle().Background("#22c55e").BorderColor("#22c55e").PaddingVertical(9).PaddingLeft(5);
                                    }
                                });

                                foreach (var item in transaction.TransactionDetail)
                                {
                                    var detailPresentation = PresentationInstance.GetPresentation(item.Presentation, products);
                                    var detailProduct = ProductIntance.GetProduct(detailPresentation.Product, products);
                                    float presentationPrice = 0;
                                    /*if (transaction.Cover)
                                    {
                                        presentationPrice = item.Detail ? detailPresentation.PriceRetailCover : detailPresentation.PriceOutputCover;
                                    }
                                    else
                                    {
                                        presentationPrice = item.Detail ? detailPresentation.PriceRetail : detailPresentation.PriceOutput;
                                    }*/
                                    if (item.Detail)
                                    {
                                        if (transaction.Cover && (detailPresentation.PriceRetailCover > 1))
                                        {
                                            presentationPrice = detailPresentation.PriceRetailCover;
                                        }
                                        else
                                        {
                                            presentationPrice = detailPresentation.PriceRetail;
                                        }
                                    }
                                    else
                                    {
                                        if (transaction.Cover && (detailPresentation.PriceOutputCover > 1))
                                        {
                                            presentationPrice = detailPresentation.PriceOutputCover;
                                        }
                                        else
                                        {
                                            presentationPrice = detailPresentation.PriceOutput;
                                        }
                                    }

                                    table.Cell().Element(CellStyle).Text(detailProduct.Name);
                                    table.Cell().Element(CellStyle).Text(detailPresentation.Name);
                                    table.Cell().Element(CellStyle).Text(item.Detail ? "Sí" : "No");
                                    table.Cell().Element(CellStyle).Text(item.Quantity.ToString());
                                    table.Cell().Element(CellStyle).Text(presentationPrice.ToString("C"));
                                    table.Cell().Element(CellStyle).Text((presentationPrice* item.Quantity).ToString("C"));

                                    static IContainer CellStyle(IContainer container)
                                    {
                                        return container.BorderHorizontal(1).BorderColor(Colors.Green.Lighten1).AlignMiddle().Padding(5);
                                    }
                                }

                                table.Footer(footer =>
                                {
                                    footer.Cell().ColumnSpan(5).AlignRight().Padding(5).Text("Total:");
                                    footer.Cell().AlignRight().Padding(5).Text(transaction.Value.ToString("C"));
                                });
                            });
                        });

                    page.Footer().Row(row =>
                    {
                        row.RelativeItem().AlignCenter().Text($"Factura generada el {DateTime.Now.ToShortDateString()} - NIT: 890901352-3").FontSize(10).FontColor("#ffffff");
                        //row.RelativeItem().AlignRight().Text("¡Gracias por su compra!").FontSize(10).FontColor("#052e16");
                    });
                });
            });
        }

        [HttpGet("SendEmail/{EMAIL}/{ID}")]
        [AllowAnonymous]
        public async Task<IActionResult> SendEmail(string EMAIL, int ID)
        {
            var transaction = await _transactionService.GetTransactionById(ID);
            var result = await _message.SendEmail(
                $"Factura Atlenal: {transaction.IdTransaction} - {transaction.Date.Day}",
                "./Templates/EBill/EBill",
                transaction,
                EMAIL);
            return result ? Ok(transaction) : StatusCode(500, "Error sending email");
        }

        //POST
        [HttpPost()]
        [AllowAnonymous]
        public async Task<ActionResult> CreateTransaction([FromBody] Transaction TRANSACTION)
        {
            var transaction = await _transactionService.CreateTransaction(TRANSACTION);
            return Ok(transaction);
        }
    }
}
