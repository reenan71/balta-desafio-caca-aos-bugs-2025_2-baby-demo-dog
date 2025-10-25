using BugStore.Models;
using BugStore.Repositories.Interface;
using BugStore.Service.Interface;

namespace BugStore.Service;

public class ProductService : IProductService
{
    private readonly IRepository<Product> _repository;

    public ProductService(IRepository<Product> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Product>> GetAllAsync() => await _repository.GetAllAsync();
    public async Task<Product> GetByIdAsync(Guid id) => await _repository.GetByIdAsync(id);
    public async Task AddAsync(Product product)
    {
        if (string.IsNullOrEmpty(product.Title)) throw new ArgumentException("Title is required");
        await _repository.AddAsync(product);
    }
    public async Task UpdateAsync(Product product) => await _repository.UpdateAsync(product);
    public async Task DeleteAsync(Guid id) => await _repository.DeleteAsync(id);
}