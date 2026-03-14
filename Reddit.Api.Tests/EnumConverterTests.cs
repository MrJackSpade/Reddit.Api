using Reddit.Api.Converters;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Reddit.Api.Tests
{
    [TestClass]
    public class EnumConverterTests
    {
        [TestMethod]
        public void AllEnumsUsingEmptyStringEnumConverter_HaveNullAndEmptyValues()
        {
            // Find all enum types in the Reddit.Api assembly that use EmptyStringEnumConverter
            var redditApiAssembly = typeof(EmptyStringEnumConverter<>).Assembly;
            var enumsWithConverter = redditApiAssembly
                .GetTypes()
                .Where(t => t.IsEnum)
                .Where(t =>
                {
                    var converterAttr = t.GetCustomAttribute<JsonConverterAttribute>();
                    if (converterAttr?.ConverterType == null)
                    {
                        return false;
                    }

                    // Check if it's EmptyStringEnumConverter<T>
                    var converterType = converterAttr.ConverterType;
                    return converterType.IsGenericType &&
                           converterType.GetGenericTypeDefinition() == typeof(EmptyStringEnumConverter<>);
                })
                .ToList();

            Assert.IsTrue(enumsWithConverter.Count > 0, "Expected to find at least one enum using EmptyStringEnumConverter");

            var errors = new List<string>();

            foreach (var enumType in enumsWithConverter)
            {
                var enumNames = Enum.GetNames(enumType);

                // Check for Null value
                if (!enumNames.Contains("Null", StringComparer.OrdinalIgnoreCase))
                {
                    errors.Add($"{enumType.Name} is missing a 'Null' enum value");
                }

                // Check for Empty value
                if (!enumNames.Contains("Empty", StringComparer.OrdinalIgnoreCase))
                {
                    errors.Add($"{enumType.Name} is missing an 'Empty' enum value");
                }
            }

            if (errors.Count > 0)
            {
                Assert.Fail(
                    $"The following enums using EmptyStringEnumConverter are missing required values:\n" +
                    string.Join("\n", errors));
            }
        }

        [TestMethod]
        public void EmptyStringEnumConverter_DeserializesNull()
        {
            var json = "null";
            var result = System.Text.Json.JsonSerializer.Deserialize<Models.Enums.DistinguishedKind>(json);
            Assert.AreEqual(Models.Enums.DistinguishedKind.Null, result);
        }

        [TestMethod]
        public void EmptyStringEnumConverter_DeserializesEmptyString()
        {
            var json = "\"\"";
            var result = System.Text.Json.JsonSerializer.Deserialize<Models.Enums.DistinguishedKind>(json);
            Assert.AreEqual(Models.Enums.DistinguishedKind.Empty, result);
        }

        [TestMethod]
        public void EmptyStringEnumConverter_DeserializesNamedValue()
        {
            var json = "\"moderator\"";
            var result = System.Text.Json.JsonSerializer.Deserialize<Models.Enums.DistinguishedKind>(json);
            Assert.AreEqual(Models.Enums.DistinguishedKind.Moderator, result);
        }

        [TestMethod]
        public void EmptyStringEnumConverter_SerializesNull()
        {
            var result = System.Text.Json.JsonSerializer.Serialize(Models.Enums.DistinguishedKind.Null);
            Assert.AreEqual("null", result);
        }

        [TestMethod]
        public void EmptyStringEnumConverter_SerializesEmpty()
        {
            var result = System.Text.Json.JsonSerializer.Serialize(Models.Enums.DistinguishedKind.Empty);
            Assert.AreEqual("\"\"", result);
        }

        [TestMethod]
        public void EmptyStringEnumConverter_SerializesNamedValue()
        {
            var result = System.Text.Json.JsonSerializer.Serialize(Models.Enums.DistinguishedKind.Moderator);
            Assert.AreEqual("\"moderator\"", result);
        }
    }
}
