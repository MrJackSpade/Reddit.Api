using System.Reflection;

namespace Reddit.Api.Json.Exceptions
{
    public class MisconfiguredPropertyException(PropertyInfo propertyInfo, string message) : DeserializationException(message)
    {
        public PropertyInfo PropertyInfo { get; private set; } = propertyInfo;
    }
}