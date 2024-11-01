using Microsoft.AspNetCore.Identity;

namespace ABCRetailers_Cameron_Chetty_CLDV6212_POE_P3.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
    }
}
