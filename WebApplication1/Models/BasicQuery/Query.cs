namespace WebApplication1.Models.BasicQuery
{
    public abstract class Query
    {
        public string? SearchWord { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? SortBy { get; set; }
        public SortDirection SortDirection { get; set; }
    }
}
