namespace Core.Specifications
{
    public class ProductSpecParams
    {
        const int MaxPageSize = 50;
        public string Sort { get; set; }
        public int? TypeId { get; set; }
        public int? BrandId { get; set; }
        private string search;
        public string Search
        {
            get => search;
            set => search = value.ToLower();
        }
        public int PageIndex { get; set; } = 1;
        private int pageSize = 6;
        public int PageSize
        {
            get => pageSize;
            set => pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

    }
}