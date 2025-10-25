using BugStore.Data;
using BugStore.Models;
using BugStore.Repositories;
using BugStore.Repositories.Interface;
using BugStore.Service;
using BugStore.Service.Interface;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var conn = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(x => x.UseSqlite(conn));
builder.Services.AddScoped<IRepository<Customer>, CustomerRepository>();
builder.Services.AddScoped<IRepository<Product>, ProductRepository>();
builder.Services.AddScoped<IRepository<Order>, OrderRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();

var app = builder.Build();

// Customers
app.MapGet("/v1/customers", async (ICustomerService service) => 
    Results.Ok(await service.GetAllAsync()));
app.MapGet("/v1/customers/{id}", async (Guid id, ICustomerService service) => 
    await service.GetByIdAsync(id) is Customer customer 
        ? Results.Ok(customer) 
        : Results.NotFound());
app.MapPost("/v1/customers", async (Customer customer, ICustomerService service) => 
{
    await service.AddAsync(customer);
    return Results.Created($"/v1/customers/{customer.Id}", customer);
});
app.MapPut("/v1/customers/{id}", async (Guid id, Customer customer, ICustomerService service) => 
{
    customer.Id = id;
    await service.UpdateAsync(customer);
    return Results.NoContent();
});
app.MapDelete("/v1/customers/{id}", async (Guid id, ICustomerService service) => 
{
    await service.DeleteAsync(id);
    return Results.NoContent();
});

// Products
app.MapGet("/v1/products", async (IProductService service) => 
    Results.Ok(await service.GetAllAsync()));
app.MapGet("/v1/products/{id:guid}", async (Guid id, IProductService service) => 
    await service.GetByIdAsync(id) is Product product 
        ? Results.Ok(product) 
        : Results.NotFound());
app.MapPost("/v1/products", async (Product product, IProductService service) => 
{
    await service.AddAsync(product);
    return Results.Created($"/v1/products/{product.Id}", product);
});
app.MapPut("/v1/products/{id:guid}", async (Guid id, Product product, IProductService service) => 
{
    product.Id = id;
    await service.UpdateAsync(product);
    return Results.NoContent();
});
app.MapDelete("/v1/products/{id:guid}", async (Guid id, IProductService service) => 
{
    await service.DeleteAsync(id);
    return Results.NoContent();
});

//Orders
app.MapGet("/v1/orders", async (IOrderService service) => 
    Results.Ok(await service.GetAllAsync()));

app.MapGet("/v1/orders/{id:guid}", async (Guid id, IOrderService service) => 
    await service.GetByIdAsync(id) is Order order 
        ? Results.Ok(order) 
        : Results.NotFound());

app.MapPost("/v1/orders", async (Order order, IOrderService service) => 
{
    await service.AddAsync(order);
    return Results.Created($"/v1/orders/{order.Id}", order);
});

app.MapPut("/v1/orders/{id:guid}", async (Guid id, Order order, IOrderService service) => 
{
    order.Id = id;
    await service.UpdateAsync(order);
    return Results.NoContent();
});

app.MapDelete("/v1/orders/{id:guid}", async (Guid id, IOrderService service) => 
{
    await service.DeleteAsync(id);
    return Results.NoContent();
});

app.Run();
