using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json;
using System;
using System.Buffers;
using System.IO;

namespace Blazui.Community.MvcCore
{
    public class KJsonOutputFormatter : NewtonsoftJsonOutputFormatter
    {
        public KJsonOutputFormatter(JsonSerializerSettings serializerSettings) : base(serializerSettings, ArrayPool<char>.Shared, new MvcOptions())
        {
        }

        public new JsonSerializerSettings SerializerSettings => base.SerializerSettings;

        protected override JsonWriter CreateJsonWriter(TextWriter writer)
        {
            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }
            var jsonWriter = new NullJsonWriter(writer)
            {
                //ArrayPool = ArrayPool<char>.Shared,//不知道咋写。。。貌似不写也没问题
                CloseOutput = false,
                AutoCompleteOnClose = false
            };
            return jsonWriter;
        }
    }

    public class NullJsonWriter : JsonTextWriter
    {
        public NullJsonWriter(TextWriter textWriter) : base(textWriter)
        {
        }

        public override void WriteNull()
        {
            this.WriteValue(string.Empty);
        }
    }
}