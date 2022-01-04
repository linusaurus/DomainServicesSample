namespace MyServices;

using System;
using System.Collections.Generic;
using System.Security.Claims;
using DomainServices;
using DomainServices.Abstractions;

public interface IProductRepository : IRepository<Product, Guid>, IDiscreteRepository<Product, Guid>, IUpdatableRepository<Product, Guid>
{
    bool ContainsName(string name);

    IEnumerable<Product> Get(Query<Product> query, ClaimsPrincipal? user = null);
}
