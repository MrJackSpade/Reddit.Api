namespace Reddit.Api.Json.Exceptions
{
    public class DeserializationException : Exception
    {
        public DeserializationException()
        {
        }

        public DeserializationException(string? message) : base(message)
        {
        }

        public DeserializationException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}