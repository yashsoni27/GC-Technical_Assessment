using LicenseeRecords.WebAPI.Models;
using LicenseeRecords.WebAPI.Services;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]/[action]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly ProductService _productService;

    public ProductController(ProductService productService)
    {
        _productService = productService;
    }
    
    [HttpGet]
    public ActionResult<List<Product>> GetProducts()
    {
        return _productService.GetProducts();
    }

    [HttpGet("{id}")]
    public ActionResult<Product> GetProductByID(int id)
    {
        var product = _productService.GetProductByID(id);
        if (product == null)
        {
            return NotFound();
        }
        return product;
    }

    [HttpPost]
    public IActionResult AddProduct(Product product)
    {
        _productService.AddProduct(product);
        return CreatedAtAction(nameof(GetProductByID), new { id = product.ProductId }, product);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteProduct(int id)
    {
        _productService.DeleteProduct(id);
        return Ok("Product Updated Successfully!");
    }

}