using System;
using System.Threading.Tasks;

namespace OneTree.Assessment.Core.IService
{
    public interface IProductService : IService<Domain.Entities.Product, Dtos.Product>
    {
        Task<Dtos.Product> CreateAsync(Dtos.ProductToCreate productCreate);
        Task<Dtos.Product> ModifyAsync(Dtos.ProductToUpdate productUpdate);
        Task RemoveAsync(Guid id);
    }
}
