using EStore.Contracts.Orders;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace EStore.Infrastructure.Services.FileGenerators.Invoices;

public class InvoiceDocument : IDocument
{
    private static readonly ShippingAddressResponse s_SellerAddress = new(
        "EStore Inc.",
        "+84987654123",
        "1 Vo Van Ngan St",
        "Thu Duc",
        "Ho Chi Minh",
        "Vietnam",
        "70000");

    // public static Image LogoImage { get; } = Image.FromFile("wwwroot/images/logo.svg");
    public OrderResponse Model { get; set; }

    public InvoiceDocument(OrderResponse model)
    {
        Model = model;
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

    public void Compose(IDocumentContainer container)
    {
        container
            .Page(page =>
            {
                page.Margin(50);
                
                page.Header().Element(ComposeHeader);
                page.Content().Element(ComposeContent);
                
                page.Footer().AlignCenter().Text(text =>
                {
                    text.CurrentPageNumber();
                    text.Span(" / ");
                    text.TotalPages();
                });
            });
    }

    void ComposeHeader(IContainer container)
    {
        container.Row(row =>
        {
            row.RelativeItem().Column(column =>
            {
                column
                    .Item().Text($"Invoice #{Model.OrderNumber}")
                    .FontSize(20).SemiBold().FontColor(Colors.Blue.Medium);

                column.Item().Text(text =>
                {
                    text.Span("Issue date: ").SemiBold();
                    text.Span($"{Model.CreatedDateTime:d}");
                });
                
                column.Item().Text(text =>
                {
                    text.Span("Due date: ").SemiBold();
                    text.Span($"{Model.CreatedDateTime.AddDays(7):d}");
                });
            });

            // row.ConstantItem(175).Image(LogoImage);
        });
    }

    void ComposeContent(IContainer container)
    {
        container.PaddingVertical(40).Column(column => 
        {
            column.Spacing(20);
            
            column.Item().Row(row =>
            {
                row.RelativeItem().Component(new AddressComponent("From", s_SellerAddress));
                row.ConstantItem(50);
                row.RelativeItem().Component(new AddressComponent("For", Model.ShippingAddress));
            });

            var totalDiscount = Model.OrderItems.Sum(item => item.TotalDiscount);

            column.Item().Element(ComposeTable);
            column.Item().PaddingRight(5).AlignRight().Text($"Total discount: {totalDiscount:C}").SemiBold();
            column.Item().PaddingRight(5).AlignRight().Text($"Grand total: {Model.TotalAmount:C}").Bold();
        });
    }

    void ComposeTable(IContainer container)
    {
        var headerStyle = TextStyle.Default.SemiBold();
        
        container.Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.ConstantColumn(25);
                columns.RelativeColumn(3);
                columns.RelativeColumn();
                columns.RelativeColumn();
                columns.RelativeColumn();
                columns.RelativeColumn();
            });
            
            table.Header(header =>
            {
                header.Cell().Text("#");
                header.Cell().Text("Product").Style(headerStyle);
                header.Cell().AlignRight().Text("Unit price").Style(headerStyle);
                header.Cell().AlignRight().Text("Quantity").Style(headerStyle);
                header.Cell().AlignRight().Text("Total").Style(headerStyle);
                header.Cell().AlignRight().Text("Discount").Style(headerStyle);
                
                header.Cell().ColumnSpan(6).PaddingTop(5).BorderBottom(1).BorderColor(Colors.Black);
            });
            
            foreach (var item in Model.OrderItems)
            {
                var index = Model.OrderItems.IndexOf(item) + 1;
                var productName = item.ProductAttributes == null
                    ? item.ProductName
                    : $"{item.ProductName} - {item.ProductAttributes}"; 

                table.Cell().Element(CellStyle).Text($"{index}");
                table.Cell().Element(CellStyle).Text(productName);
                table.Cell().Element(CellStyle).AlignRight().Text($"{item.UnitPrice:C}");
                table.Cell().Element(CellStyle).AlignRight().Text($"{item.Quantity}");
                table.Cell().Element(CellStyle).AlignRight().Text($"{item.SubTotal:C}");
                table.Cell().Element(CellStyle).AlignRight().Text($"{item.TotalDiscount:C}");
                
                static IContainer CellStyle(IContainer container)
                    => container.BorderBottom(1)
                        .BorderColor(Colors.Grey.Lighten2)
                        .PaddingVertical(5);
            }
        });
    }

    public class AddressComponent : IComponent
    {
        private string Title { get; }
        private ShippingAddressResponse ShippingAddress { get; }

        public AddressComponent(string title, ShippingAddressResponse address)
        {
            Title = title;
            ShippingAddress = address;
        }
        
        public void Compose(IContainer container)
        {
            container.ShowEntire().Column(column =>
            {
                column.Spacing(2);

                column.Item().Text(Title).SemiBold();
                column.Item().PaddingBottom(5).LineHorizontal(1); 
                
                column.Item().Text(ShippingAddress.ReceiverName);
                column.Item().Text(ShippingAddress.Street);
                column.Item().Text($"{ShippingAddress.City}, {ShippingAddress.State}, {ShippingAddress.Country}");
                column.Item().Text(ShippingAddress.PhoneNumber);
            });
        }
    }
}
