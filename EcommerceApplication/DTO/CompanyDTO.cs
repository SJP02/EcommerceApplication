using EcommerceApplication.Models;

namespace EcommerceApplication.DTO
{
    public class CompanyDTO
    {
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public int PhoneNumber { get; set; }
        public List<ProductDTO> ProductList { get; set; }=new List<ProductDTO>();
    }
}
