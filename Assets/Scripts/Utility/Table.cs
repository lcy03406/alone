//utf-8。
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Utility {

	public class Table {
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
		private static void LoadRecord(CsvParser parser, Header header, object record) {
			List<string> row = parser.ReadRecord();
			for (int i = 0; i < header.name.Count; i++) {
				LoadField(row[i], record, header.xpath[i], header.type[i]);
			}
		}
		private static void LoadField(string text, object record, string xpath, string type) {
			object value = Activator.CreateInstance(record.GetType().Assembly.FullName, type);
			string[] elements = xpath.Split('.');
			foreach (string element in elements) {
				int index = element.IndexOf('[');
				if (index >= 0) {
					var elementName = element.Substring(0, index);
					var pos = element.Substring(index + 1, element.Length - index - 2);
					var i = System.Convert.ToInt32(pos);
					obj = GetValue_Imp(obj, elementName, i);
				} else {
					obj = GetValue_Imp(obj, element);
				}
			}
		}

		private static object GetValue_Imp(object source, string name) {
			if (source == null)
				return null;
			var type = source.GetType();

			while (type != null) {
				var f = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
				if (f != null)
					return f.GetValue(source);

				var p = type.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
				if (p != null)
					return p.GetValue(source, null);

				type = type.BaseType;
			}
			return null;
		}

		private static object GetValue_Imp(object source, string name, int index) {
			var enumerable = GetValue_Imp(source, name) as System.Collections.IEnumerable;
			if (enumerable == null) return null;
			var enm = enumerable.GetEnumerator();
			//while (index-- >= 0)
			//    enm.MoveNext();
			//return enm.Current;

			for (int i = 0; i <= index; i++) {
				if (!enm.MoveNext()) return null;
			}
			return enm.Current;
		}
	}
}
