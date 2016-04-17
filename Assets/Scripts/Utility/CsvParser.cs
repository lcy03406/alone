//utf-8。
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Utility {
	public class CsvParser : IDisposable {
		private const int Comment = '#';
		private const int Quote = '"';
		private const int Delimiter = ',';

		private TextReader reader;
		private int line = 0;
		private int column = 0;

		public CsvParser(TextReader reader) {
			this.reader = reader;
		}

		public List<string> ReadRecord() {
			SkipBlankLines();
			if (reader.Peek() == -1)
				return null;
			List<string> record = new List<string>();
			while (true) {
				string field = ReadField();
				if (field == null)
					break;
				record.Add(field);
			}
			line++;
			return record;
		}

		private void SkipBlankLines() {
			while (true) {
				int a = reader.Peek();
				if (a == -1)
					return;
				if (a == Comment || a == '\n' || a == '\r')
					reader.ReadLine();
			}
		}

		private string ReadField() {
			StringBuilder sb = new StringBuilder();
			int a = reader.Read();
			if (a == -1)
				return null;
			if (a == Quote) {
				if (ReadTo(sb, Quote) != Quote)
					throw new InvalidDataException("unclosed quote in " + Where());
				while (reader.Peek() == Quote) {
					if (ReadTo(sb, Quote) != Quote)
						throw new InvalidDataException("unclosed quote in " + Where());
				}
				int fin = reader.Read();
				if (fin != Delimiter && fin != -1)
					throw new InvalidDataException("unquoted quote in " + Where());
			} else {
				sb.Append(a);
				ReadTo(sb, Delimiter);
			}
			column++;
			return sb.ToString();
		}

		private int ReadTo(StringBuilder sb, int stop) {
			while (true) {
				int a = reader.Read();
				if (a == -1)
					return -1;
				else if (a == stop)
					return stop;
				else
					sb.Append(a);
			}
		}

		private string Where() {
			return string.Format("line {0}, column {1}", line, column);
		}

		public void Dispose() {
			((IDisposable)this.reader).Dispose();
		}
	}
}
