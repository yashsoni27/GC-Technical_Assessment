using System.ComponentModel.DataAnnotations;
using static LicenseeRecords.WebAPI.Models.ProductLicence;

namespace LicenseeRecords.WebAPI.Models
{
    public class Account
    {
        public int AccountId { get; set; }

        [Required(ErrorMessage = "Account Name is required")]
        public string AccountName { get; set; }

        [Required(ErrorMessage = "Account Status is required")]
        public string AccountStatus { get; set; }
        public List<ProductLicence> ProductLicence { get; set; } = new();
    }
}
