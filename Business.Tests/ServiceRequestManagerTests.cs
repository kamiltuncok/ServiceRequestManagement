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
            // Arrange
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

            // Act
            var result = await manager.AddAsync(createDto);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(capturedRequest);
            Assert.Equal("New", capturedRequest.Status);
        }

        [Fact]
        public async Task AddAsync_WhenNewRequestCreated_CreatedDateIsAutomaticallyAssigned()
        {
            // Arrange
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

            // Act
            var beforeExecution = DateTime.Now;
            var result = await manager.AddAsync(createDto);
            var afterExecution = DateTime.Now;

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(capturedRequest);
            Assert.NotEqual(default(DateTime), capturedRequest.CreatedDate);
            // Check if the CreatedDate is approximately current time (between start and end of execution)
            // Added 1 second tolerance for before/after comparison to avoid edge cases in fast executions
            Assert.True(capturedRequest.CreatedDate >= beforeExecution.AddSeconds(-1) && capturedRequest.CreatedDate <= afterExecution.AddSeconds(1));
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenUpdateIsSuccessful_StatusIsUpdatedToNewValue()
        {
            // Arrange
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

            // Setup GetAsync to return our existing request
            mockDal.Setup(m => m.GetAsync(It.IsAny<Expression<Func<ServiceRequest, bool>>>()))
                   .ReturnsAsync(existingRequest);

            ServiceRequest capturedRequest = null;
            // Setup UpdateAsync to capture the updated request
            mockDal.Setup(m => m.UpdateAsync(It.IsAny<ServiceRequest>()))
                   .Callback<ServiceRequest>(req => capturedRequest = req)
                   .Returns(Task.CompletedTask);

            var expectedStatus = "In Progress";

            // Act
            var result = await manager.UpdateStatusAsync(1, expectedStatus);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(capturedRequest);
            Assert.Equal(expectedStatus, capturedRequest.Status);
            // Also check if the object's status was changed
            Assert.Equal(expectedStatus, existingRequest.Status);
        }
    }
}
