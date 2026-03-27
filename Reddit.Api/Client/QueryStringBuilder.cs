using Reddit.Api.Models;
using System.Collections.Specialized;
using System.Web;

namespace Reddit.Api.Client
{
    public class QueryStringBuilder
    {
        private readonly NameValueCollection _params = HttpUtility.ParseQueryString("");

        public QueryStringBuilder Add(string key, string? value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                _params[key] = value;
            }

            return this;
        }

        public QueryStringBuilder Add(string key, int? value)
        {
            if (value.HasValue)
            {
                _params[key] = value.Value.ToString();
            }

            return this;
        }

        public QueryStringBuilder Add(string key, bool? value)
        {
            if (value.HasValue)
            {
                _params[key] = value.Value.ToString().ToLowerInvariant();
            }

            return this;
        }

        public QueryStringBuilder Add(string key, JsonBool value)
        {
            if (!value.IsNull)
            {
                _params[key] = value.ToString();
            }

            return this;
        }

        /// <summary>
        /// Adds the key with a fixed value if the condition is true.
        /// Useful for patterns like show=all where the value isn't derived from the parameter.
        /// </summary>
        public QueryStringBuilder AddIf(bool condition, string key, string value)
        {
            if (condition)
            {
                _params[key] = value;
            }

            return this;
        }

        public string Build()
        {
            string query = _params.ToString() ?? "";
            return query.Length > 0 ? $"?{query}" : "";
        }
    }
}
