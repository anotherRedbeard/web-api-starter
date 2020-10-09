namespace Models
{
#pragma warning disable CS1591
    public class UrlQuery
    {
        private const int MIN_PAGE_NUMBER = 1;
        private int _pageNumber = MIN_PAGE_NUMBER;
        public int? PageNumber 
        { 
            get
            {
                return _pageNumber;
            } 
            set
            {
                _pageNumber = value.HasValue ? value.Value : MIN_PAGE_NUMBER;
            }
        }
        public int? PageSize { get; set; }
        public bool IncludeCount { get; set; } = false;
    }
#pragma warning disable CS1591
}