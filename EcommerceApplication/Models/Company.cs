namespace EcommerceApplication.Models
{
    public class Company
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public int PhoneNumber { get; set; }
        public int EstablishedYear{get;set;}
        public List<Product> ProductList { get; set; } = new List<Product>();


    }
}
