namespace Entities.DTOs
{
    public class ServiceRequestCreateDto
    {
        public string CustomerName { get; set; } = string.Empty;
        public string DeviceName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class ServiceRequestUpdateStatusDto
    {
        public string Status { get; set; } = string.Empty;
    }
}
