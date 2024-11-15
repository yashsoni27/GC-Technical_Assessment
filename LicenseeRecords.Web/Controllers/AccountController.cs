using Microsoft.AspNetCore.Mvc;
using LicenseeRecords.WebAPI.Services;
using LicenseeRecords.WebAPI.Models;

public class AccountController : Controller
{
    private readonly AccountService _accountService;
    private readonly ProductService _productService;
    public AccountController(AccountService accountService, ProductService productService)
    {
        _accountService = accountService;
        _productService = productService;
    }

    public IActionResult Index()
    {
        var accounts = _accountService.GetAccounts();
        return View(accounts);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Account account, List<int>? SelectedProducts)
    {
        if (ModelState.IsValid)
        {
            var accounts = _accountService.GetAccounts();
            account.AccountId = accounts.Any() ? accounts.Max(a => a.AccountId) + 1 : 1;

            if (SelectedProducts != null && SelectedProducts.Any())
            {
                var maxLicenceId = accounts
                .SelectMany(a => a.ProductLicence)
                .DefaultIfEmpty()
                .Max(pl => pl?.LicenceId ?? 0);

                account.ProductLicence = SelectedProducts.Select(productId => new ProductLicence
                {
                    LicenceId = ++maxLicenceId,
                    Product = _productService.GetProductByID(productId),
                    LicenceStatus = "Active",
                    LicenceFromDate = DateTime.Now,
                    LicenceToDate = null
                }).ToList();
            }

            _accountService.AddAccount(account);
            return RedirectToAction("Index", "Home");
        }
        return View(account);
    }

    public IActionResult Edit(int id)
    {
        var account = _accountService.GetAccountByID(id);
        if (account == null)
        {
            return NotFound();
        }
        return View(account);
    }

    [HttpPost]
    public IActionResult Edit(Account account, List<int>? SelectedProducts)
    {
        if (ModelState.IsValid)
        {
            var existingAccount = _accountService.GetAccountByID(account.AccountId);
            if (existingAccount == null)
            {
                return NotFound();
            }

            // Update basic account info
            existingAccount.AccountName = account.AccountName;
            existingAccount.AccountStatus = account.AccountStatus;

            if (SelectedProducts != null && SelectedProducts.Any())
            {
                // Get all existing licenses (both active and inactive)
                var existingLicenses = existingAccount.ProductLicence.ToList();

                // Find products that need new licenses (not present in any existing license)
                var existingProductIds = existingLicenses
                    .Select(l => l.Product.ProductId)
                    .ToList();
                var completelyNewProductIds = SelectedProducts
                    .Except(existingProductIds)
                    .ToList();

                // Reactivate inactive licenses if their products are selected
                foreach (var license in existingLicenses)
                {
                    if (SelectedProducts.Contains(license.Product.ProductId))
                    {
                        license.LicenceStatus = "Active";
                        license.LicenceToDate = null;
                    }
                    else
                    {
                        license.LicenceStatus = "Inactive";
                        license.LicenceToDate = DateTime.Now;
                    }
                }

                // Only create new licenses for completely new products
                if (completelyNewProductIds.Any())
                {
                    var accounts = _accountService.GetAccounts();
                    var maxLicenceId = accounts
                        .SelectMany(a => a.ProductLicence)
                        .DefaultIfEmpty()
                        .Max(pl => pl?.LicenceId ?? 0);

                    var newLicences = completelyNewProductIds.Select(productId => new ProductLicence
                    {
                        LicenceId = ++maxLicenceId,
                        Product = _productService.GetProductByID(productId),
                        LicenceStatus = "Active",
                        LicenceFromDate = DateTime.Now,
                        LicenceToDate = null
                    }).ToList();

                    existingAccount.ProductLicence.AddRange(newLicences);
                }
            }
            else
            {
                foreach (var license in existingAccount.ProductLicence)
                {
                    license.LicenceStatus = "Inactive";
                    license.LicenceToDate = DateTime.Now;
                }
            }

            _accountService.UpdateAccount(existingAccount);
            return RedirectToAction("Index", "Home");
        }
        return View(account);
    }

    public IActionResult Delete(int id)
    {
        var account = _accountService.GetAccountByID(id);
        if (account == null)
        {
            return NotFound();
        }
        return View(account);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeleteConfirmed(int id)
    {
        _accountService.DeleteAccount(id);
        return RedirectToAction("Index", "Home");
    }
}
