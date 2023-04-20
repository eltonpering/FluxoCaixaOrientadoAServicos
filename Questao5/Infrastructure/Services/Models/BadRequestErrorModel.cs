namespace Questao5.Infrastructure.Services.Models
{
    public class BadRequestErrorModel
    {
        public string Type { get; set; }
        public string Error { get; set; }
        public string Detail { get; set; }
        public string Property { get; set; }
    }
}
