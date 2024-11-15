using LicenseeRecords.WebAPI.Models;

namespace LicenseeRecords.WebAPI.Services
{
    public class AccountService
    {
        private readonly JsonFileService<Account> _jsonFileService;

        public AccountService(JsonFileService<Account> jsonFileService)
        {
            _jsonFileService = jsonFileService;
        }
        public List<Account> GetAccounts() => _jsonFileService.ReadJson();

        public Account GetAccountByID(int id) => _jsonFileService.ReadJson().FirstOrDefault(a => a.AccountId == id);

        public void AddAccount(Account account) 
        { 
            var accounts = _jsonFileService.ReadJson();
            accounts.Add(account);
            _jsonFileService.WriteJson(accounts);
        }

        public void UpdateAccount(Account account)
        {
            var accounts = _jsonFileService.ReadJson();
            var index = accounts.FindIndex(a => a.AccountId == account.AccountId);
            if (index != -1)
            {
                accounts[index] = account;
                _jsonFileService.WriteJson(accounts);
            }
        }

        public void DeleteAccount(int id)
        {
            var accounts = _jsonFileService.ReadJson();
            var account = accounts.FirstOrDefault(a => a.AccountId == id);
            if(account != null)
            {
                accounts.Remove(account);
                _jsonFileService.WriteJson(accounts);
            }
        }
    }
}
