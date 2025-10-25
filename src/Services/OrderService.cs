using BugStore.Models;
using BugStore.Repositories.Interface;
using BugStore.Service.Interface;

namespace BugStore.Service;

public class OrderService : IOrderService
{
    private readonly IRepository<Order> _orderRepository;
    private readonly IRepository<Customer> _customerRepository;
    private readonly IRepository<Product> _productRepository;

    public OrderService(IRepository<Order> orderRepository, IRepository<Customer> customerRepository, IRepository<Product> productRepository)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
    }

    public async Task<IEnumerable<Order>> GetAllAsync()
    {
        return await _orderRepository.GetAllAsync();
    }

    public async Task<Order> GetByIdAsync(Guid id)
    {
        return await _orderRepository.GetByIdAsync(id);
    }

    public async Task AddAsync(Order order)
    {
        if (order == null) throw new ArgumentNullException(nameof(order));
        if (order.CustomerId == Guid.Empty) throw new ArgumentException("CustomerId is required");
        var customer = await _customerRepository.GetByIdAsync(order.CustomerId);
        if (customer == null) throw new ArgumentException("Invalid CustomerId");

        order.CreatedAt = DateTime.UtcNow; // Ex.: 2025-10-25T21:46:00Z
        order.UpdatedAt = order.CreatedAt;
        order.Lines ??= new List<OrderLine>();

        foreach (var line in order.Lines)
        {
            if (line.ProductId == Guid.Empty) throw new ArgumentException("ProductId is required in OrderLine");
            var product = await _productRepository.GetByIdAsync(line.ProductId);
            if (product == null) throw new ArgumentException("Invalid ProductId");
            line.Total = line.Quantity * product.Price; // Calcula o total por linha
        }

        await _orderRepository.AddAsync(order);
    }

    public async Task UpdateAsync(Order order)
    {
        if (order == null) throw new ArgumentNullException(nameof(order));
        var existingOrder = await _orderRepository.GetByIdAsync(order.Id);
        if (existingOrder == null) throw new KeyNotFoundException("Order not found");
        if (order.CustomerId != existingOrder.CustomerId)
        {
            var customer = await _customerRepository.GetByIdAsync(order.CustomerId);
            if (customer == null) throw new ArgumentException("Invalid CustomerId");
        }

        order.UpdatedAt = DateTime.UtcNow; // Ex.: 2025-10-25T21:46:00Z
        order.Lines ??= new List<OrderLine>();

        foreach (var line in order.Lines)
        {
            var product = await _productRepository.GetByIdAsync(line.ProductId);
            if (product == null) throw new ArgumentException("Invalid ProductId");
            line.Total = line.Quantity * product.Price;
        }

        await _orderRepository.UpdateAsync(order);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _orderRepository.DeleteAsync(id);
    }
}