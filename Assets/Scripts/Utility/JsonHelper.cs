//utf-8。
using System;
using IList = System.Collections.IList;
using IDictionary = System.Collections.IDictionary;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace Utility {

	public static class JsonHelper {
		public static object ReadObject(string text, Type type) {
			if (type == typeof(string)) {
				return text;
			}
            try {
				using (StringReader sr = new StringReader(text))
				using (JsonTextReader reader = new JsonTextReader(sr)) {
					return Ser().Deserialize(reader, type);
				}
			} catch (Exception e) {
				throw new InvalidDataException(string.Format("connot convert \"{0}\" to type {1}", text, type), e);
			}
			throw new NotImplementedException();
		}

		public static JsonSerializer Ser() {
			JsonSerializer ser = new JsonSerializer();
			ser.Converters.Add(new StringEnumConverter());
			ser.Converters.Add(new StringTypeConverter());
			ser.Converters.Add(new IntSchemaConverter());
			ser.MissingMemberHandling = MissingMemberHandling.Error;
			ser.TypeNameHandling = TypeNameHandling.Auto;
			ser.TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple;
			ser.Formatting = Formatting.Indented;
			//ser.Error += Ser_Error;
			return ser;
		}

		public class StringTypeConverter : JsonConverter {
			public override bool CanRead { get { return true; } }
			public override bool CanWrite { get { return true; } }

			public override bool CanConvert(Type objectType) {
				return objectType == typeof(Type);
			}

			public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
				if (reader.TokenType != JsonToken.String) {
					throw new InvalidDataException(string.Format("expect string, got {0} \"{1}\"", reader.TokenType, reader.Value));
				}
				string str = (string)reader.Value;
				return TypeHelper.GetType(str);
			}

			public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
				Type v = (Type)value;
				JToken t = JToken.FromObject(v.Name);
				t.WriteTo(writer);
			}
		}

		public class IntSchemaConverter : JsonConverter {
			public override bool CanRead { get { return true; } }
			public override bool CanWrite { get { return true; } }

			public override bool CanConvert(Type objectType) {
				return typeof(Schema.All.Data).IsAssignableFrom(objectType);
			}

			public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
				if (reader.TokenType != JsonToken.Integer) {
					throw new InvalidDataException(string.Format("expect integer, got {0} \"{1}\"", reader.TokenType, reader.Value));
				}
				int value = (int) Convert.ChangeType(reader.Value, typeof(int));
				return Schema.All.all.GetOrAddEmpty(value, objectType);
			}

			public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
				Schema.All.Data v = (Schema.All.Data)value;
				JToken t = JToken.FromObject(v.ID);
				t.WriteTo(writer);
			}
		}
	}
}
