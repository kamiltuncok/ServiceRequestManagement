using Core.Entities.Abstract;

namespace Entities.Concrete
{
    public class ServiceRequest : IEntity
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string DeviceName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
    }
}
