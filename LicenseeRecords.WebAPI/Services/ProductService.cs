using LicenseeRecords.WebAPI.Models;

namespace LicenseeRecords.WebAPI.Services
{
    public class ProductService
    {
        private readonly JsonFileService<Product> _jsonFileService;

        public ProductService(JsonFileService<Product> jsonFileService)
        {
            _jsonFileService = jsonFileService;
        }
        public List<Product> GetProducts() => _jsonFileService.ReadJson();

        public Product GetProductByID(int id) => _jsonFileService.ReadJson().FirstOrDefault(p => p.ProductId == id);

        public void AddProduct(Product product)
        {
            var products = _jsonFileService.ReadJson();
            int newId = products.Any()
                ? products.Max(p => p.ProductId) + 1
                : 1;
            product.ProductId = newId;

            products.Add(product);
            _jsonFileService.WriteJson(products);
        }

        public void UpdateProduct(Product product)
        {
            var products = _jsonFileService.ReadJson();
            var index = products.FindIndex(p => p.ProductId == product.ProductId);
            if (index != -1)
            {
                products[index] = product;
                _jsonFileService.WriteJson(products);
            }
        }

        public void DeleteProduct(int id)
        {
            var products = _jsonFileService.ReadJson();
            var product = products.FirstOrDefault(p => p.ProductId == id);
            if (product != null)
            {
                products.Remove(product);
                _jsonFileService.WriteJson(products);
            }
        }
    }
}
