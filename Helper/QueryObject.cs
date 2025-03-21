namespace stockapplocation.Helper
{
    public class QueryObject
    {
        // Nullable types allow for missing values (e.g., optional in query parameters)
        public string? Symbol { get; set; } = null;
        public string? CompanyName { get; set; } = null;

        // Default null can be used to indicate no sorting, or a default sorting can be set
        public string? SortBy { get; set; } = null;

        // Fixed typo
        public bool IsDescending { get; set; } = false;

        // Pagination values
        public int limit { get; set; } = 100;
        public int offset { get; set; } = 0;

        // Embed field to include extra data in the query result, defaults to empty list
        public List<string> Embed { get; set; } = new List<string>();
    }
}
