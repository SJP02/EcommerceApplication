using System.Text.Json.Serialization;

namespace EcommerceApplication.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public int CompanyId { get; set; }
        public int? CategoryId { get; set; }  // ? Add this
        
        [JsonIgnore]
        public Company? ProductCompany { get; set; }
        
        [JsonIgnore]
        public Category? ProductCategory { get; set; }  // ? Add this
        public string? ImageUrl { get; set; }
    }
}
