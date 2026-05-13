namespace EcommerceApplication.DTO
{
    public class ProductFilterDTO
    {
        public string? Search { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? CategoryName { get; set; }
        public string? SortBy { get; set; } = "Id";
        public string? SortOrder { get; set; } = "asc";
        public int? MinQuantity { get; set; }
        public int? MaxQuantity { get; set; }
        public int? CompanyId { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;

    }
}