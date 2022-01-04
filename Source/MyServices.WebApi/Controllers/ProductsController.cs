namespace MyServices.WebApi.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly ProductService _productService;

    public ProductsController(IProductRepository productRepository)
    {
        _productService = new ProductService(productRepository);
    }

    [HttpGet]
    public ActionResult<IEnumerable<Product>> GetAll() => Ok(_productService.GetAll());

    [HttpGet("{id}")]
    public ActionResult<Product> Get(Guid id) => Ok(_productService.Get(id));

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public ActionResult<Product> Add(ProductDto productDto)
    {
        var product = productDto.ToProduct();
        _productService.Add(product);
        return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
    }

    [HttpPut]
    public ActionResult<Product> Update(ProductDto productDto)
    {
        var product = productDto.ToProduct();
        _productService.Update(product);
        return Ok(_productService.Get(product.Id));
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult Delete(Guid id)
    {
        _productService.Remove(id);
        return NoContent();
    }
}
