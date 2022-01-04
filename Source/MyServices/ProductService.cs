namespace MyServices;

using System;
using System.Collections.Generic;
using System.Security.Claims;
using DomainServices;
using DomainServices.Abstractions;
using DomainServices.Logging;

public class ProductService : BaseUpdatableDiscreteService<Product, Guid>
{
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public ProductService(IProductRepository repository, ILogger logger) : base(repository, logger)
    {
        _repository = repository;
    }

    public override void Add(Product product, ClaimsPrincipal? user = null)
    {
        if (_repository.ContainsName(product.Name))
        {
            throw new ArgumentException($"There is already a product with the name '{product.Name}'.", nameof(product));
        }

        base.Add(product, user);
    }

    public IEnumerable<Product> Get(Query<Product> query, ClaimsPrincipal? user = null)
    {
        return _repository.Get(query, user);
    }
}
