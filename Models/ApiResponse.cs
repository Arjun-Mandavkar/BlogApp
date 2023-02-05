namespace BlogApp.Models
{
    public class ApiResponse
    {
        public IEnumerable<IResponseData> Data { get; set; }
        public bool IsSuccess { get; set; }
    }
}
