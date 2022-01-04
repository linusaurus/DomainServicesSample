namespace MyServices.Test;

using System;
using System.Collections.Generic;
using System.Linq;
using DomainServices;
using Xunit;

public class ProductServiceTest
{
    [Fact]
    public void CreateWithNullRepositoryWillThrow()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new ProductService(null!));
        Assert.Equal("repository", exception.ParamName);
    }

    [Fact]
    public void GetNonExistingThrows()
    {
        var products = new ProductService(new FakeProductRepository());
        Assert.Throws<KeyNotFoundException>(() => products.Get(Guid.NewGuid()));
    }

    [Fact]
    public void RemoveNonExistingThrows()
    {
        var products = new ProductService(new FakeProductRepository());
        Assert.Throws<KeyNotFoundException>(() => products.Remove(Guid.NewGuid()));
    }

    [Fact]
    public void UpdateNonExistingThrows()
    {
        var products = new ProductService(new FakeProductRepository());
        var product = new Product(Guid.NewGuid(), "MyProduct name");
        Assert.Throws<KeyNotFoundException>(() => products.Update(product));
    }

    [Fact]
    public void AddExistingThrows()
    {
        var products = new ProductService(new FakeProductRepository());
        var product = new Product(Guid.NewGuid(), "MyProduct name");
        products.Add(product);
        Assert.Throws<ArgumentException>(() => products.Add(product));
    }

    [Fact]
    public void AddWithExistingNameThrows()
    {
        var products = new ProductService(new FakeProductRepository());
        var product = new Product(Guid.NewGuid(), "MyProduct name");
        products.Add(product);
        var productWithSameName = new Product(Guid.NewGuid(), product.Name);
        Assert.Throws<ArgumentException>(() => products.Add(productWithSameName));
    }

    [Fact]
    public void AddAndGetIsOk()
    {
        var products = new ProductService(new FakeProductRepository());
        var product = new Product(Guid.NewGuid(), "MyProduct name");
        products.Add(product);
        Assert.Equal(product.Id, products.Get(product.Id).Id);
    }

    [Fact]
    public void EventsAreRaisedOnAdd()
    {
        var raisedEvents = new List<string>();
        var products = new ProductService(new FakeProductRepository());
        products.Adding += (_, _) => { raisedEvents.Add("Adding"); };
        products.Added += (_, _) => { raisedEvents.Add("Added"); };
        var product = new Product(Guid.NewGuid(), "MyProduct name");
        products.Add(product);
        Assert.Equal("Adding", raisedEvents[0]);
        Assert.Equal("Added", raisedEvents[1]);
    }

    [Fact]
    public void GetAllIsOk()
    {
        var products = new ProductService(new FakeProductRepository());
        var product1 = new Product(Guid.NewGuid(), "MyProduct name");
        var product2 = new Product(Guid.NewGuid(), "MyOtherProduct name");
        products.Add(product1);
        products.Add(product2);
        Assert.Equal(2, products.GetAll().Count());
    }

    [Fact]
    public void GetByQueryIsOk()
    {
        var coke = new Product(Guid.NewGuid(), "Coke") { Price = 9.95M };
        var cokeLight = new Product(Guid.NewGuid(), "Coke Light") { Price = 10.95M };
        var fanta = new Product(Guid.NewGuid(), "Fanta") { Price = 8.95M };
        var products = new ProductService(new FakeProductRepository(new[] { coke, cokeLight, fanta }));
        var queryCheapest = new Query<Product> { new("Price", QueryOperator.LessThan, 10M) };
        var cheapest = products.Get(queryCheapest).ToList();
        var queryCoke = new Query<Product> { new("Name", QueryOperator.Like, "Coke") };
        var cokes = products.Get(queryCoke).ToList();
        var queryNotCoke = new Query<Product> { new("Name", QueryOperator.NotLike, "Coke") };
        var notCokes = products.Get(queryNotCoke).ToList();

        Assert.Equal(2, cheapest.Count);
        Assert.DoesNotContain(cheapest, product => product.Id == cokeLight.Id);
        Assert.Equal(2, cokes.Count);
        Assert.DoesNotContain(cokes, product => product.Id == fanta.Id);
        Assert.Single(notCokes);
        Assert.Contains(notCokes, product => product.Id == fanta.Id);
    }

    [Fact]
    public void CountIsOk()
    {
        var products = new ProductService(new FakeProductRepository());
        var product1 = new Product(Guid.NewGuid(), "MyProduct name");
        var product2 = new Product(Guid.NewGuid(), "MyOtherProduct name");
        products.Add(product1);
        products.Add(product2);
        Assert.Equal(2, products.Count());
    }

    [Fact]
    public void ExistsIsOk()
    {
        var products = new ProductService(new FakeProductRepository());
        var product = new Product(Guid.NewGuid(), "MyProduct name");
        products.Add(product);
        Assert.True(products.Exists(product.Id));
    }

    [Fact]
    public void DoesNotExistIsOk()
    {
        var products = new ProductService(new FakeProductRepository());
        Assert.False(products.Exists(Guid.NewGuid()));
    }

    [Fact]
    public void RemoveIsOk()
    {
        var products = new ProductService(new FakeProductRepository());
        var product = new Product(Guid.NewGuid(), "MyProduct name");
        products.Add(product);
        products.Remove(product.Id);
        Assert.False(products.Exists(product.Id));
    }

    [Fact]
    public void EventsAreRaisedOnRemove()
    {
        var raisedEvents = new List<string>();
        var products = new ProductService(new FakeProductRepository());
        products.Deleting += (_, _) => { raisedEvents.Add("Deleting"); };
        products.Deleted += (_, _) => { raisedEvents.Add("Deleted"); };
        var product = new Product(Guid.NewGuid(), "MyProduct name");
        products.Add(product);
        products.Remove(product.Id);
        Assert.Equal("Deleting", raisedEvents[0]);
        Assert.Equal("Deleted", raisedEvents[1]);
    }

    [Fact]
    public void UpdateIsOk()
    {
        var products = new ProductService(new FakeProductRepository());
        var product = new Product(Guid.NewGuid(), "MyProduct name");
        products.Add(product);
        var productUpdated = new Product(product.Id, "MyProduct new name");
        products.Update(productUpdated);
        Assert.Equal(productUpdated.Name, products.Get(product.Id).Name);
    }

    [Fact]
    public void EventsAreRaisedOnUpdate()
    {
        var raisedEvents = new List<string>();
        var products = new ProductService(new FakeProductRepository());
        products.Updating += (_, _) => { raisedEvents.Add("Updating"); };
        products.Updated += (_, _) => { raisedEvents.Add("Updated"); };
        var product = new Product(Guid.NewGuid(), "MyProduct name");
        products.Add(product);
        var productUpdated = new Product(product.Id, "MyProduct new name");
        products.Update(productUpdated);
        Assert.Equal("Updating", raisedEvents[0]);
        Assert.Equal("Updated", raisedEvents[1]);
    }
}
