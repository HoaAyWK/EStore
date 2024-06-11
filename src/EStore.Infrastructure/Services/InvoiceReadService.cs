using EStore.Application.Invoices.Services;
using EStore.Contracts.Common;
using EStore.Contracts.Invoices;
using EStore.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EStore.Infrastructure.Services;

public class InvoiceReadService : IInvoiceReadService
{
    private readonly EStoreDbContext _dbContext;

    public InvoiceReadService(EStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedList<InvoiceResponse>> GetListPagedAsync(
        int page,
        int pageSize,
        string? order,
        string? orderBy)
    {
        var invoicesQuery = _dbContext.Invoices.AsNoTracking();

        var invoiceResponsesQuery = invoicesQuery.Select(invoice => new InvoiceResponse
        {
            Id = invoice.Id.Value,
            InvoiceNumber = invoice.InvoiceNumber,
            CreatedDateTime = invoice.CreatedDateTime,
            UpdatedDateTime = invoice.UpdatedDateTime,
            CustomerId = invoice.CustomerId.Value,
            Customer = _dbContext.Customers.AsNoTracking()
                .Where(customer => customer.Id == invoice.CustomerId)
                .Select(customer => new CustomerResponse(
                    customer.Id.Value,
                    customer.FirstName,
                    customer.LastName,
                    customer.Email,
                    customer.AvatarUrl))
                .SingleOrDefault(),
            OrderItems = _dbContext.Orders.AsNoTracking()
                .Where(order => order.Id == invoice.OrderId)
                .SelectMany(order => order.OrderItems)
                .Select(orderItem => new OrderItemResponse(
                    orderItem.ItemOrdered.ProductId.Value,
                    orderItem.ItemOrdered.ProductVariantId! == null!
                        ? null
                        : orderItem.ItemOrdered.ProductVariantId.Value,
                    orderItem.ItemOrdered.ProductName,
                    orderItem.ItemOrdered.ProductImage!,
                    orderItem.ItemOrdered.ProductAttributes,
                    orderItem.UnitPrice,
                    orderItem.SubTotal,
                    orderItem.TotalDiscount,
                    orderItem.Quantity)).ToList(),
        });

        if (!string.IsNullOrEmpty(order))
        {
            if (!string.IsNullOrWhiteSpace(order) && order.ToLower() == "desc")
            {
                invoiceResponsesQuery = orderBy switch
                {
                    "customer" => invoiceResponsesQuery.OrderByDescending(invoice => invoice.Customer!.FirstName),
                    "createdDateTime" => invoiceResponsesQuery.OrderByDescending(invoice => invoice.CreatedDateTime),
                    _ => invoiceResponsesQuery.OrderByDescending(invoice => invoice.InvoiceNumber)
                };
            }
            else
            {
                invoiceResponsesQuery = orderBy switch
                {
                    "customer" => invoiceResponsesQuery.OrderBy(invoice => invoice.Customer!.FirstName),
                    "createdDateTime" => invoiceResponsesQuery.OrderBy(invoice => invoice.CreatedDateTime),
                    _ => invoiceResponsesQuery.OrderBy(invoice => invoice.InvoiceNumber)
                };
            }
        }

        var totalItems =  await invoicesQuery.CountAsync();
        var totalPages = 1;

        if (page is not 0)
        {
            if (pageSize is 0)
            {
                page = 1;
                totalPages = 1;
            }
            else
            {
                invoiceResponsesQuery = invoiceResponsesQuery
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize);

                totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            }
        }
        else
        {
            page = 1;
        }

        var invoiceResponses = await invoiceResponsesQuery
            .ToListAsync();

        return new PagedList<InvoiceResponse>(invoiceResponses, page, pageSize, totalItems, totalPages);
    }
}
