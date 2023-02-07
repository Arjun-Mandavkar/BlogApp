namespace BlogApp.Models.Response
{
    public class ValidationResult
    {
        private IEnumerable<Message> _messages = new List<Message>();

        public bool Succeeded { get; set; } = true;

        public IEnumerable<Message> Messages => _messages;

        public static ValidationResult Success => new ValidationResult();
        
        public static ValidationResult Failed(params Message[] messages)
        {
            ValidationResult result = new ValidationResult { Succeeded = false };
            if (messages != null)
            {
                foreach (var m in messages)
                    result._messages.Append(m);
            }
            return result;
        }

    }
}
