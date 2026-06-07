using Core.DataAccess;
using Entities.Concrete;

namespace DataAccess.Abstract
{
    public interface IServiceRequestDal : IEntityRepository<ServiceRequest>
    {
    }
}
