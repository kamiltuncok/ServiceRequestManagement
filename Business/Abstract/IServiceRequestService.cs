using Core.Utilities.Results;
using Entities.Concrete;
using Entities.DTOs;

namespace Business.Abstract
{
    public interface IServiceRequestService
    {
        Task<IDataResult<List<ServiceRequest>>> GetAllAsync(string? searchTerm = null, string? status = null);
        Task<IDataResult<ServiceRequest>> GetByIdAsync(int id);
        Task<IResult> AddAsync(ServiceRequestCreateDto serviceRequestDto);
        Task<IResult> UpdateStatusAsync(int id, string newStatus);
        Task<IResult> DeleteAsync(int id);
    }
}
