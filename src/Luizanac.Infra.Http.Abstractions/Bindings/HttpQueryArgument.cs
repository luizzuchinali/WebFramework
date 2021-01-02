using System;

namespace Luizanac.Infra.Http.Abstractions.Bindings
{
    public class HttpQueryArgument
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public HttpQueryArgument(string key, string value)
        {
            Key = key ?? throw new ArgumentNullException(nameof(key));
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }


    }
}