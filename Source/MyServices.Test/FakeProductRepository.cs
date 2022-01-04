namespace MyServices.Test;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using DomainServices;
using DomainServices.Repositories;

public class FakeProductRepository : FakeRepository<Product, Guid>, IProductRepository
{
    public FakeProductRepository()
    {
    }

    public FakeProductRepository(IEnumerable<Product> products) : base(products)
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
