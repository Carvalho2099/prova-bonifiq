namespace ProvaPub.Models
{    
        public class PaginatedList<T>
        {
            public List<T> Items { get; set; }
            public int TotalCount { get; set; }
            public bool HasNext { get; set; }
        }

        public class CustomerList : PaginatedList<Customer>
        {

        }
        public class ProductList : PaginatedList<Product>
        {

        }
    
}
