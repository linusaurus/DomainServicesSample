namespace MyServices.Data;

using DomainServices;
using DomainServices.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

public class ProductRepository : JsonRepository<Product, Guid>, IProductRepository
{
    public ProductRepository(string filePath) : base(filePath)
    {
    }

    public bool ContainsName(string name)
    {
        return GetAll().Any(product => product.Name.Equals(name));
    }

    public IEnumerable<Product> Get(Query<Product> query, ClaimsPrincipal? user = null)
    {
        return Get(query.ToExpression(), user);
    }
}
