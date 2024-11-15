using LicenseeRecords.WebAPI.Models;
using LicenseeRecords.WebAPI.Services;
using Microsoft.AspNetCore.Mvc;


public class ProductController : Controller
{
    private readonly ProductService _productService;

    public ProductController(ProductService productService)
    {
        _productService = productService;
    }

    public IActionResult Index()
    {
        var products = _productService.GetProducts();
        return View(products);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Product product)
    {
        if (ModelState.IsValid)
        {
            _productService.AddProduct(product);
            return RedirectToAction("Index", "Home");
        }
        return View(product);
    }

    //public IActionResult Edit(int id)
    //{
    //    var product = _productService.GetProductByID(id);
    //    if (product == null)
    //    {
    //        return NotFound();
    //    }
    //    return View(product);
    //}

    //[HttpPost]
    //public IActionResult Edit(Product product)
    //{
    //    if (ModelState.IsValid)
    //    {
    //        _productService.UpdateProduct(product);
    //        return RedirectToAction(nameof(Index));
    //    }
    //    return View(product);
    //}

    public IActionResult Delete(int id)
    {
        var product = _productService.GetProductByID(id);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeleteConfirmed(int id)
    {
        _productService.DeleteProduct(id);
        return RedirectToAction("Index", "Home");
    }

}

