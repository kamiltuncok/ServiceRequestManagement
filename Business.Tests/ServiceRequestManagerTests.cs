using Business.Concrete;
using Entities.Concrete;
using Entities.DTOs;
using DataAccess.Abstract;
using Moq;
using System.Linq.Expressions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Business.Tests
{
    public class ServiceRequestManagerTests
    {
        [Fact]
        public async Task AddAsync_WhenNewRequestCreated_StatusIsAutomaticallyAssignedAsNew()
        {

            var mockDal = new Mock<IServiceRequestDal>();
            var manager = new ServiceRequestManager(mockDal.Object);
            var createDto = new ServiceRequestCreateDto
            {
                CustomerName = "Ahmet Yılmaz",
                DeviceName = "Laptop",
                Description = "Ekran kırık"
            };

            ServiceRequest capturedRequest = null;
            mockDal.Setup(m => m.AddAsync(It.IsAny<ServiceRequest>()))
                   .Callback<ServiceRequest>(req => capturedRequest = req)
                   .Returns(Task.CompletedTask);


            var result = await manager.AddAsync(createDto);

            Assert.True(result.Success);
            Assert.NotNull(capturedRequest);
            Assert.Equal("New", capturedRequest.Status);
        }

        [Fact]
        public async Task AddAsync_WhenNewRequestCreated_CreatedDateIsAutomaticallyAssigned()
        {

            var mockDal = new Mock<IServiceRequestDal>();
            var manager = new ServiceRequestManager(mockDal.Object);
            var createDto = new ServiceRequestCreateDto
            {
                CustomerName = "Ayşe Kaya",
                DeviceName = "Akıllı Telefon",
                Description = "Batarya değişimi"
            };

            ServiceRequest capturedRequest = null;
            mockDal.Setup(m => m.AddAsync(It.IsAny<ServiceRequest>()))
                   .Callback<ServiceRequest>(req => capturedRequest = req)
                   .Returns(Task.CompletedTask);


            var beforeExecution = DateTime.Now;
            var result = await manager.AddAsync(createDto);
            var afterExecution = DateTime.Now;


            Assert.True(result.Success);
            Assert.NotNull(capturedRequest);
            Assert.NotEqual(default(DateTime), capturedRequest.CreatedDate);

            Assert.True(capturedRequest.CreatedDate >= beforeExecution.AddSeconds(-1) && capturedRequest.CreatedDate <= afterExecution.AddSeconds(1));
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenUpdateIsSuccessful_StatusIsUpdatedToNewValue()
        {

            var mockDal = new Mock<IServiceRequestDal>();
            var manager = new ServiceRequestManager(mockDal.Object);
            
            var existingRequest = new ServiceRequest
            {
                Id = 1,
                CustomerName = "Mehmet Demir",
                DeviceName = "Tablet",
                Description = "Şarj almıyor",
                Status = "New",
                CreatedDate = DateTime.Now.AddDays(-1)
            };

            mockDal.Setup(m => m.GetAsync(It.IsAny<Expression<Func<ServiceRequest, bool>>>()))
                   .ReturnsAsync(existingRequest);

            ServiceRequest capturedRequest = null;
            mockDal.Setup(m => m.UpdateAsync(It.IsAny<ServiceRequest>()))
                   .Callback<ServiceRequest>(req => capturedRequest = req)
                   .Returns(Task.CompletedTask);

            var expectedStatus = "In Progress";


            var result = await manager.UpdateStatusAsync(1, expectedStatus);

            Assert.True(result.Success);
            Assert.NotNull(capturedRequest);
            Assert.Equal(expectedStatus, capturedRequest.Status);
            Assert.Equal(expectedStatus, existingRequest.Status);
        }
    }
}
