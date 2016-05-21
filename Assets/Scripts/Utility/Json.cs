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
			ser.Converters.Add(new StringSchemaConverter());
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
					throw new GameResourceException(string.Format("expect string, got {0} \"{1}\"", reader.TokenType, reader.Value));
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

		public class StringSchemaConverter : JsonConverter {
			public override bool CanRead { get { return true; } }
			public override bool CanWrite { get { return true; } }

			public override bool CanConvert(Type objectType) {
				return typeof(Edit.Meta).IsAssignableFrom(objectType);
			}

			public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
				Type valueType = objectType;
				int value = 0;
				if (reader.TokenType == JsonToken.String) {
					string str = (string)reader.Value;
					string[] part = str.Split('@');
					if (part.Length > 2) {
						throw new GameResourceException(string.Format("expect string with one '@', got \"{0}\"", reader.Value));
					} else if (part.Length == 2) {
						Type type = TypeHelper.GetType(part[1]);
						if (!objectType.IsAssignableFrom(type)) {
							throw new GameResourceException(string.Format("expect type string {0}, got \"{1}\"", objectType, reader.Value));
						}
						valueType = type;
					}
					value = (int)Convert.ChangeType(part[0], typeof(int));
				} else if (reader.TokenType == JsonToken.Integer) {
					value = (int)reader.Value;
				} else {
					throw new GameResourceException(string.Format("expect string or int, got {0} \"{1}\"", reader.TokenType, reader.Value));
				}
				if (value <= 0) {
					throw new GameResourceException(string.Format("expect id, got \"{0}\"", reader.Value));
				}
				return Edit.All.all.GetOrAddEmpty(value, valueType);
			}

			public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
				Edit.Meta v = (Edit.Meta)value;
				JToken t = JToken.FromObject(v.ID);
				t.WriteTo(writer);
			}
		}
	}
}
