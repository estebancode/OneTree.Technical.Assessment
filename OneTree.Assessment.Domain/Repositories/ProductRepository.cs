using OneTree.Assessment.Domain.Entities;
using OneTree.Assessment.Domain.IRepositories;

namespace OneTree.Assessment.Domain.Repositories
{
    public class ProductRepository : ERepository<Product>, IProductRepository
    {
        public ProductRepository(IQueryableUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
