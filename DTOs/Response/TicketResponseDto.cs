namespace template_csharp_dotnet.DTOs.Response
{
    public class TicketResponseDto
    {
        public string TicketId { get; set; }
        public bool IsValid { get; set; }
        public string EventName { get; set; }
        public DateTime EventDate { get; set; }
    }
}
