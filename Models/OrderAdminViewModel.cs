namespace ABCRetailers_Cameron_Chetty_CLDV6212_POE_P3.Models
{
    public class OrderAdminViewModel
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string UserEmail { get; set; }
        public string? Status { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
