namespace Cardapio.API.Pagination
{
    public class QueryStringParameters
    {
         const int maxPageSize = 50;
        public int PageNumber { get; set; } = 1;

        private int _pagiSize = 10;
        public int PageSize 
        { 
            get
            {
                return _pagiSize;
            } 
            set
            {
                _pagiSize = (value > maxPageSize) ? maxPageSize : value;
            } 
        }
    }
}