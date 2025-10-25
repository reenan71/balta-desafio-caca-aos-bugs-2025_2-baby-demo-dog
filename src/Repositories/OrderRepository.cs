using BugStore.Data;
using BugStore.Models;
using BugStore.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace BugStore.Repositories;

public class OrderRepository : IRepository<Order>
{
    private readonly AppDbContext _context;

    public OrderRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Order> GetByIdAsync(Guid id)
    {
        return await _context.Orders
            .Include(o => o.Lines)
            .ThenInclude(ol => ol.Product)
            .FirstOrDefaultAsync(o => o.Id == id) ?? throw new KeyNotFoundException("Order not found");
    }

    public async Task<IEnumerable<Order>> GetAllAsync()
    {
        return await _context.Orders
            .Include(o => o.Lines)
            .ThenInclude(ol => ol.Product)
            .ToListAsync();
    }

    public async Task AddAsync(Order entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
        _context.Orders.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Order entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
        var existingOrder = await _context.Orders.FindAsync(entity.Id);
        if (existingOrder == null) throw new KeyNotFoundException("Order not found");
        _context.Entry(existingOrder).CurrentValues.SetValues(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null) throw new KeyNotFoundException("Order not found");
        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();
    }
}