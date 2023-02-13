namespace BlogApp.Models.Response
{
    public class ServiceResult
    {
        private List<Message> _messages = new List<Message>();
        public bool Succeeded { get; set; } = true;
        public IEnumerable<Message> Messages => _messages;

        public static ServiceResult Success(params Message[] messages)
        {
            ServiceResult result = new ServiceResult();
            if (messages != null)
            {
                result._messages.AddRange(messages);
            }
            return result;
        }

        public static ServiceResult Failed(params Message[] messages)
        {
            ServiceResult result = new ServiceResult { Succeeded = false };
            if(messages != null)
            {
                result._messages.AddRange(messages);
            }
            return result;
        }
    }
}
