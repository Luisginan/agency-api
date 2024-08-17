namespace Agency.AppointmentModule.Dto;

public class AppointmentRequestDto
{
    public DateTime Date { get; set; }
    public string Description { get; set; } = string.Empty;
    public int AgencyId { get; set; }
    public int CustomerId { get; set; }
}