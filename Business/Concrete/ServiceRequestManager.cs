using Business.Abstract;
using Business.ValidationRules.FluentValidation;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;

namespace Business.Concrete
{
    public class ServiceRequestManager : IServiceRequestService
    {
        private readonly IServiceRequestDal _serviceRequestDal;

        public ServiceRequestManager(IServiceRequestDal serviceRequestDal)
        {
            _serviceRequestDal = serviceRequestDal;
        }

        public async Task<IResult> AddAsync(ServiceRequestCreateDto serviceRequestDto)
        {
            var serviceRequest = new ServiceRequest
            {
                CustomerName = serviceRequestDto.CustomerName,
                DeviceName = serviceRequestDto.DeviceName,
                Description = serviceRequestDto.Description,
                Status = "New",
                CreatedDate = DateTime.Now
            };

            try
            {
                ValidationTool.Validate(new ServiceRequestValidator(), serviceRequest);
            }
            catch (FluentValidation.ValidationException ex)
            {
                return new ErrorResult(string.Join(" ", ex.Errors.Select(e => e.ErrorMessage)));
            }

            await _serviceRequestDal.AddAsync(serviceRequest);
            return new SuccessResult("Servis talebi başarıyla oluşturuldu.");
        }

        public async Task<IResult> DeleteAsync(int id)
        {
            var entity = await _serviceRequestDal.GetAsync(r => r.Id == id);
            if (entity == null)
            {
                return new ErrorResult("Servis talebi bulunamadı.");
            }

            await _serviceRequestDal.DeleteAsync(entity);
            return new SuccessResult("Servis talebi başarıyla silindi.");
        }

        public async Task<IDataResult<List<ServiceRequest>>> GetAllAsync(string? searchTerm = null, string? status = null)
        {
            var data = await _serviceRequestDal.GetAllAsync(r =>
                (string.IsNullOrEmpty(searchTerm) || 
                 r.CustomerName.ToLower().Contains(searchTerm.ToLower()) || 
                 r.DeviceName.ToLower().Contains(searchTerm.ToLower())) &&
                (string.IsNullOrEmpty(status) || r.Status == status)
            );

            return new SuccessDataResult<List<ServiceRequest>>(data, "Veriler başarıyla getirildi.");
        }

        public async Task<IDataResult<ServiceRequest>> GetByIdAsync(int id)
        {
            var entity = await _serviceRequestDal.GetAsync(r => r.Id == id);
            if (entity == null)
            {
                return new ErrorDataResult<ServiceRequest>("Servis talebi bulunamadı.");
            }

            return new SuccessDataResult<ServiceRequest>(entity);
        }

        public async Task<IResult> UpdateStatusAsync(int id, string newStatus)
        {
            var entity = await _serviceRequestDal.GetAsync(r => r.Id == id);
            if (entity == null)
            {
                return new ErrorResult("Servis talebi bulunamadı.");
            }

            entity.Status = newStatus;
            await _serviceRequestDal.UpdateAsync(entity);
            return new SuccessResult("Servis talebi durumu başarıyla güncellendi.");
        }
    }
}
