//utf-8。
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Utility {
	public class CsvParser {
		private const int Comment = '#';
		private const int Quote = '"';
		private const int Delimiter = ',';

		private TextReader reader;
		private int line = 0;
		private int column = 0;

		public int Line { get { return line; } }

		public CsvParser(TextReader reader) {
			this.reader = reader;
		}

		public bool IsEnd() {
			return reader.Peek() == -1;
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
			column = 0;
			return record;
		}

		private void SkipBlankLines() {
			while (true) {
				int a = reader.Peek();
				if (a == Comment || a == '\n' || a == '\r')
					reader.ReadLine();
				else
					return;
			}
		}

		private static bool FieldFinish(int c) {
			return (c == Delimiter || c == -1 || c == '\n' || c == '\r');
        }

		private string ReadField() {
			StringBuilder sb = new StringBuilder();
			int c = reader.Peek();
			if (c == -1 || c == '\n' || c == '\r') {
				return null;
			}
			if (c == Delimiter) {
				if (column == 0) {
					column++;
					return "";
				} else {
					reader.Read();
					c = reader.Peek();
					if (FieldFinish(c)) {
						column++;
						return "";
					}
				}
			} else if (column > 0) {
				//won't heppen
				throw new InvalidDataException("no delimiter in " + Where());
			}
			reader.Read();
			if (c == Quote) {
				if (ReadTo(sb, Quote) != Quote)
					throw new InvalidDataException("unclosed quote in " + Where());
				while (reader.Peek() == Quote) {
					sb.Append((char)Quote);
					reader.Read();
					if (ReadTo(sb, Quote) != Quote)
						throw new InvalidDataException("unclosed quote in " + Where());
				}
				int fin = reader.Peek();
				if (!FieldFinish(fin))
                    throw new InvalidDataException("unquoted quote in " + Where());
			} else {
				sb.Append((char)c);
				int next = -1;
				while (true) {
					next = reader.Peek();
					if (FieldFinish(next))
						break;
					sb.Append((char)next);
					reader.Read();
				}
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
				sb.Append((char)a);
			}
		}

		private string Where() {
			return string.Format("line {0}, column {1}", line + 1, column + 1);
		}
	}
}
