namespace Talabat.Core.Specifications
{
    public class ProductSpecificationParameter
    {
        private const int MaxPageSize = 10;
        private int pageSize = 5;

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value > MaxPageSize ? MaxPageSize : value; }
        }
        public int PageIndex { get; set; } = 1;

        private string search;

        public string Search
        {
            get { return search; }
            set { search = value.ToLower(); }
        }

        public string? sort { get; set; }
        public int? brandID { get; set; }
        public int? typeID { get; set; }

    }
}
