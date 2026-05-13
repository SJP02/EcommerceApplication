namespace EcommerceApplication.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<Product> ProductList { get; set; } = new List<Product>();
    }
}