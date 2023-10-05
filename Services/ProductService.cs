using ProvaPub.Models;
using ProvaPub.Repository;

namespace ProvaPub.Services
{
	public class ProductService
    {
		TestDbContext _ctx;

		public ProductService(TestDbContext ctx)
		{
			_ctx = ctx;
		}

		public ProductList ListProducts(int page)
		{
			int pageSize = 10;
			int startPage = (page - 1) * pageSize;
			var products = _ctx.Products.Skip(startPage).Take(pageSize).ToList();
			int totalCount = _ctx.Products.Count();
			bool hasNext = startPage + pageSize < totalCount;

            return new ProductList() {  HasNext= hasNext, TotalCount = totalCount, Items = products };
		}

	}
}
