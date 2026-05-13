public class RequestParameters
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;

    private const int maxPageSize = 50;
    public int PageSizeLimit
    {
        get => PageSize;
        set => PageSize = (value > maxPageSize) ? maxPageSize : value;
    }
}