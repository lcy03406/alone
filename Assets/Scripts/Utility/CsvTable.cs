//utf-8。
using System;
using System.IO;
using System.Collections.Generic;

namespace Utility {

	public class CsvTable {
		public interface Loader {
			object Allocate(Type type, int id);
		}

		public static void Load(TextReader text, Type type, Loader loader) {
			CsvParser parser = new CsvParser(text);
			Header header = LoadHeader(parser, type);
			while (true) {
				object t = LoadRecord(parser, header, type, loader);
				if (t == null)
					return;
			}
		}
		private class Header {
			public List<string> name;
			public List<PathField> xpath;
			public List<Type> type;
			public List<string> prefix;
		}
		private static Header LoadHeader(CsvParser parser, Type type) {
			Header header = new Header();
			header.name = parser.ReadRecord();
			List<string> pathstr = parser.ReadRecord();
			List<string> typestr = parser.ReadRecord();
			header.prefix = new List<string>();
			int count = header.name.Count;
			if (pathstr.Count != count) {
				throw new GameResourceException(string.Format("header path count {0} != {1}", pathstr.Count, count));
			}
			if (typestr.Count != count) {
				throw new GameResourceException(string.Format("header type count {0} != {1}", typestr.Count, count));
			}
			if (pathstr[0] != "ID" || typestr[0] != "int") {
				throw new GameResourceException(string.Format("header 1st column is not int ID but {0} {1}", pathstr[0], typestr[0]));
			}
			header.xpath = new List<PathField>();
			try {
				for (int i = 0; i < count; ++i) {
					if (SkipColumn(header.name[i])) {
						header.xpath.Add(null);
						continue;
					}
					string spath = pathstr[i];
					header.xpath.Add(new PathField(type, spath));
				}
			} catch (Exception e) {
				throw new InvalidDataException(string.Format("fail parse header path column {0}", header.type.Count), e);
			}
			header.type = new List<Type>();
			for (int i = 0; i < count; ++i) {
				if (SkipColumn(header.name[i])) {
					header.type.Add(null);
					header.prefix.Add(null);
					continue;
				}
				string stype = typestr[i];
				PathField path = header.xpath[i];
				Type fieldType = path.ValueType();
				if (fieldType == typeof(Type)) {
					header.type.Add(fieldType);
					header.prefix.Add(stype);
				} else {
					Type headerType = TypeHelper.GetType(stype);
					if (headerType == null) {
						throw new GameResourceException(string.Format("invalid type {0} in column {1}", stype, i));
					}
					if (!fieldType.IsAssignableFrom(headerType)) {
						throw new GameResourceException(string.Format("incompitable type {0} as {1} in column {2}", headerType, fieldType, i));
					}
					header.type.Add(headerType);
					header.prefix.Add(null);
				}
			}
			return header;
		}
        private static object LoadRecord(CsvParser parser, Header header, Type type, Loader loader) {
			List<string> row = parser.ReadRecord();
			if (row == null)
				return null;
			if (row.Count != header.name.Count) {
				throw new GameResourceException(string.Format("field count {0} != {1} in line {2}", row.Count, header.name.Count, parser.Line));
			}
			int id = int.Parse(row[0]);
			object record = loader.Allocate(type, id);
			int i = 0;
			try {
				for (i = 0; i < header.name.Count; i++) {
					string text = row[i];
					if (SkipColumn(header.name[i]))
						continue;
					string prefix = header.prefix[i];
					if (prefix != null)
						text = prefix + text;
					Field path = header.xpath[i];
					Type valueType = path.ValueType();
					Type headerType = header.type[i];
					object value = JsonHelper.ReadObject(text, headerType);
					path.Set(record, value);
				}
			} catch (Exception e) {
				throw new InvalidDataException(string.Format("fail load {0} in line {1} column {2}", row[i], parser.Line, i+1), e);
			}
			return record;
		}

		private static bool SkipColumn(string v) {
			return v == null || v == "" || v[0] == '~';
		}
	}
}
