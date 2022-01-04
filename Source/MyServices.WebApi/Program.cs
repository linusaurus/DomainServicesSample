using MyServices;
using MyServices.Data;
using MyServices.WebApi;

var builder = WebApplication.CreateBuilder(args);

var products = new List<Product> { 
    new Product(Guid.NewGuid(), "Coke") { Price = 1.35M },
    new Product(Guid.NewGuid(), "Fanta") { Price = 1.85M }
};
var productRepository = new InMemoryProductRepository(products);
builder.Services.AddScoped<IProductRepository>(_ => productRepository);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => { options.DefaultModelsExpandDepth(-1); });
}

app.UseExceptionHandling();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
