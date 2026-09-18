using Ecommerce_api.Data;
using Ecommerce_api.Domain;
using Ecommerce_api.Domain.Orders;
using Ecommerce_api.Domain.Payments;
using Ecommerce_api.Domain.Products;
using Ecommerce_api.Exceptions;
using Ecommerce_api.Infrastructure.Auth;
using Ecommerce_api.Infrastructure.FileStorage;
using Ecommerce_api.Infrastructure.Payments;
using Microsoft.EntityFrameworkCore;
using File = Ecommerce_api.Domain.File;

namespace Ecommerce_api.Features.Orders.Create;

public class CreateOrderHandler
{
    private readonly AppDbContext _context;
    private readonly IFileStorage _fileStorage;
    private readonly IPaymentService _paymentService;
    private readonly ICurrentUser _currentUser;

    public CreateOrderHandler(
        AppDbContext dbContext,
        IFileStorage fileStorage,
        IPaymentService paymentService,
        ICurrentUser currentUser
    )
    {
        _context = dbContext;
        _fileStorage = fileStorage;
        _paymentService = paymentService;
        _currentUser = currentUser;
    }

    public async Task Handle(
        CreateOrderRequest request,
        CancellationToken cancellationToken
    )
    {
        var productIds = request.Items
            .Select(item => item.ProductId)
            .Distinct()
            .ToList();

        var products = await _context.Products
            .Where(product => productIds.Contains(product.Id))
            .ToListAsync(cancellationToken);

        if (products.Count != productIds.Count)
            throw new NotFoundException("One or more products");

        var orderItems = new List<OrderItem>();

        foreach (var item in request.Items)
        {
            var product = products.First(product => product.Id == item.ProductId);

            if (product.Stock < item.Quantity)
            {
                throw new ConflictException(
                    $"Insufficient stock for product {product.Id}");
            }

            orderItems.Add(
                new OrderItem
                {
                    ProductId = product.Id,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price
                }
            );
        }

        var total = orderItems.Sum(item => item.UnitPrice * item.Quantity);

        var order = new Order()
        {
            Total = total,
            Status = Status.Pending,
            Address = new Address()
            {
                Street = request.Address.Street,
                Number = request.Address.Number,
                Apartment = request.Address.Apartment,
                City = request.Address.City,
                State = request.Address.State,
                ZipCode = request.Address.ZipCode,
                Country = request.Address.Country,
                Phone = request.Address.Phone,
                Email = request.Address.Email
            },
            UserId = _currentUser.Id,
        };

        _context.Add(order);
        await _context.SaveChangesAsync(cancellationToken);

        var paymentSession =
            await _paymentService.CreateCheckoutSessionAsync(
                new PaymentSessionRequest(
                    order.Id,
                    order.Address.Email,
                    orderItems.Select(orderItem =>
                        new PaymentItem(orderItem.Product.Name, orderItem.Quantity, orderItem.UnitPrice)
                    ).ToList()
                ),
                cancellationToken
            );

        var payment = new Payment()
        {
            OrderId = order.Id,
            Total = total,
            ExternalId = paymentSession.SessionId
        };

        order.Payments.Add(payment);
        await _context.SaveChangesAsync(cancellationToken);
    }
}