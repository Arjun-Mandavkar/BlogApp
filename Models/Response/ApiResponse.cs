namespace BlogApp.Models.Response
{
    //Return type of end points becomes ambegious if we use generic type here
    //Since one end point can return multiple types of data
    public class ApiResponse
    {
        public IEnumerable<IResponseData> Data { get; set; }
        public bool IsSuccess { get; set; }
    }
}
