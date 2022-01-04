namespace MyServices.Data;

using DomainServices;
using DomainServices.Repositories;
using System.Security.Claims;

public class InMemoryProductRepository : FakeRepository<Product, Guid>, IProductRepository
{
    public InMemoryProductRepository()
    {
    }

    public InMemoryProductRepository(IEnumerable<Product> products) : base(products)
    {
    }

    public bool ContainsName(string name)
    {
        return Entities.Values.Any(product => product.Name.Equals(name));
    }

    public IEnumerable<Product> Get(Query<Product> query, ClaimsPrincipal? user = null)
    {
        return Get(query.ToExpression());
    }
}
