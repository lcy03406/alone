//utf-8。
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Utility {

	public class Table {
		public static void Load<T>(CsvParser parser, List<T> list) {
			Header header = LoadHeader(parser);
			T t;
			while (true) {
				t = (T) LoadRecord(parser, header, typeof(T));
				if (t == null)
					return;
				list.Add(t);
            }
        }
		private class Header {
			public List<string> name;
			public List<string> comment;
			public List<string> cs;
			public List<string> xpath;
			public List<string> type;
		}
		private static Header LoadHeader(CsvParser parser) {
			Header header = new Header();
			header.name = parser.ReadRecord();
			header.comment = parser.ReadRecord();
			header.cs = parser.ReadRecord();
			header.xpath = parser.ReadRecord();
			header.type = parser.ReadRecord();
			return header;
		}
		private static object LoadRecord(CsvParser parser, Header header, Type type) {
			List<string> row = parser.ReadRecord();
			if (row == null)
				return null;
			if (row.Count != header.name.Count) {
				throw new InvalidDataException(string.Format("field count {0} != {1} in line {2}", row.Count, header.name.Count, parser.Line));
			}
			object record = Activator.CreateInstance(type);
			for (int i = 0; i < header.name.Count; i++) {
				string text = row[i];
				if (text != "" && header.cs[i].Contains("s")) {
					LoadField(text, record, header.xpath[i], parser.Line, i + 1);
				}
			}
			return record;
		}
		private static void LoadField(string text, object record, string xpath, int line, int column) {
			object parent = record;
			string[] paths = xpath.Split('.');
			for (int i = 0; i < paths.Length; i++) {
				string path = paths[i];
				FieldInfo finfo;
				int index = path.IndexOf('[');
				if (index >= 0) {
					string elementName = path.Substring(0, index);
					string posName = path.Substring(index + 1, path.Length - index - 2);
					int pos = Convert.ToInt32(posName);
					finfo = GetField(parent.GetType(), elementName);
					if (finfo == null) {
						throw new InvalidDataException("no such field in " + xpath);
					}
					Type fieldType = finfo.FieldType;
                    if (fieldType.GetGenericTypeDefinition() != typeof(List<>)) {
						throw new InvalidDataException("type mismatch in " + xpath);
					}
					Type elementType = fieldType.GetGenericArguments()[0];
                    object field = finfo.GetValue(parent);
					if (field == null) {
						field = Activator.CreateInstance(fieldType);
						finfo.SetValue(parent, field);
                    }
                    IList list = field as IList;
					while (list.Count <= pos) {
						object newElement = Activator.CreateInstance(elementType);
						list.Add(newElement);
                    }
					if (i < paths.Length - 1) {
						parent = list[pos];
					} else {
						list[pos] = ConvertType(text, elementType, line, column);
                    }
				} else {
					finfo = GetField(parent.GetType(), path);
					if (finfo == null) {
						throw new InvalidDataException("no such field in " + xpath);
					}
					Type fieldType = finfo.FieldType;
					if (i < paths.Length - 1) {
						object field = finfo.GetValue(parent);
						if (field == null) {
							field = Activator.CreateInstance(fieldType);
							finfo.SetValue(parent, field);
                        }
						parent = field;
					} else {
						object value = ConvertType(text, fieldType, line, column);
						finfo.SetValue(parent, value);
					}
				}
			}
		}

		private static FieldInfo GetField(Type type, string name) {
			while (type != null) {
				FieldInfo f = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
				if (f != null)
					return f;
				type = type.BaseType;
			}
			return null;
		}

		private static object ConvertType(string text, Type type, int line, int column) {
			try {
				return Convert.ChangeType(text, type);
			} catch (Exception e) {
				throw new InvalidDataException(string.Format("connot convert \"{0}\" to type {1} in line {2}, column {3}", text, type, line, column), e);
			}
			throw new NotImplementedException();
		}

	}
}
