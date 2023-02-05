namespace BlogApp.Models.Response
{
    public class ServiceResult
    {
        private IEnumerable<Message> _messages = new List<Message>();
        public bool Succeeded { get; set; } = true;
        public IEnumerable<Message> Messages => _messages;
        public static ServiceResult Success => new ServiceResult();
        public static ServiceResult Failed(params Message[] messages)
        {
            ServiceResult result = new ServiceResult { Succeeded = false};
            if(messages != null)
            {
                foreach(var m in messages)
                    result._messages.Append(m);
            }
            return result;
        }

    }
}
