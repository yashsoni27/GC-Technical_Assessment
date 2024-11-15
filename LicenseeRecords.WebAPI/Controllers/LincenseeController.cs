using LicenseeRecords.WebAPI.Models;
using LicenseeRecords.WebAPI.Services;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]/[action]")]
[ApiController]
public class LicenseeController : ControllerBase
{
    private readonly AccountService _accountService;
    
    public LicenseeController(AccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpGet]
    public ActionResult<List<Account>> GetAccounts()
    {
        return _accountService.GetAccounts();
    }
    
    [HttpGet("{id}")]
    public ActionResult<Account> GetAccountByID(int id)
    {
        var account = _accountService.GetAccountByID(id);
        if (account == null)
        {
            return NotFound();
        }
        return account;
    }

    [HttpPost]
    public IActionResult AddAccount(Account account)
    {
        _accountService.AddAccount(account);
        return CreatedAtAction(nameof(GetAccountByID), new { id = account.AccountId }, account);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateAccount(int id, Account account)
    {
        if (id != account.AccountId) return BadRequest();
        _accountService.UpdateAccount(account);
        return Ok("Account Updated Successfully!");
    }

    //[Route("DeleteAccount")]
    [HttpDelete("{id}")]
    public IActionResult DeleteAccount(int id)
    {
        _accountService.DeleteAccount(id);
        return Ok("Account Updated Successfully!");
    }
}