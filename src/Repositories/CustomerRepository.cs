using BugStore.Data;
using BugStore.Models;
using BugStore.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace BugStore.Repositories;

public class CustomerRepository : IRepository<Customer>
{
    private readonly AppDbContext _context;

    public CustomerRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Customer> GetByIdAsync(Guid id) => await _context.Customers.FindAsync(id);
    public async Task<IEnumerable<Customer>> GetAllAsync() => await _context.Customers.ToListAsync();
    public async Task AddAsync(Customer entity)
    {
        _context.Customers.Add(entity);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateAsync(Customer entity)
    {
        _context.Customers.Update(entity);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteAsync(Guid id)
    {
        var entity = await _context.Customers.FindAsync(id);
        if (entity != null)
        {
            _context.Customers.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}