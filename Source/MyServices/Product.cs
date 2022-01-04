namespace MyServices;

using System;
using DomainServices.Abstractions;

public class Product : BaseNamedEntity<Guid>
{
    public Product(Guid id, string name) : base(id, name)
    {
    }

    public virtual decimal Price { get; set; }
}
