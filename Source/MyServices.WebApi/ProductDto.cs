namespace MyServices.WebApi;

using System.ComponentModel.DataAnnotations;

public struct ProductDto
{
    public Guid? Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [Required, Range(1, double.MaxValue)]
    public decimal Price { get; set; }

    public Product ToProduct()
    {
        return new Product(Id ?? Guid.NewGuid(), Name) { Price = Price };
    }
}
