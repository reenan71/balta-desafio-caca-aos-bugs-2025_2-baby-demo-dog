using BugStore.Models;
using BugStore.Repositories.Interface;
using BugStore.Service.Interface;

namespace BugStore.Service;

public class CustomerService : ICustomerService
{
    private readonly IRepository<Customer> _repository;

    public CustomerService(IRepository<Customer> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Customer>> GetAllAsync() => await _repository.GetAllAsync();
    public async Task<Customer> GetByIdAsync(Guid id) => await _repository.GetByIdAsync(id);
    public async Task AddAsync(Customer customer)
    {
        if (string.IsNullOrEmpty(customer.Name)) throw new ArgumentException("Name is required");
        await _repository.AddAsync(customer);
    }
    public async Task UpdateAsync(Customer customer) => await _repository.UpdateAsync(customer);
    public async Task DeleteAsync(Guid id) => await _repository.DeleteAsync(id);
}