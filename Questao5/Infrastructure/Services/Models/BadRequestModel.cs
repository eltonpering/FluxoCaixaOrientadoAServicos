namespace Questao5.Infrastructure.Services.Models
{
    public class BadRequestModel
    {
        public string TraceId { get; set; }
        public string Instance { get; set; }
        public BadRequestErrorModel Error { get; set; }
        public List<BadRequestErrorModel> Errors { get; set; } = new List<BadRequestErrorModel>();
    }
}
