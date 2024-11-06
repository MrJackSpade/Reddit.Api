namespace Reddit.Api.Exceptions
{
    public class InvalidCredentialsException : DisplayException
    {
        public InvalidCredentialsException() : base("You must be logged in to perform this action")
        {
        }
    }
}
