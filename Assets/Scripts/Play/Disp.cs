//utf-8。
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;
using Play.Attrs;

namespace Play {
	public struct Disp {
		public Edit.Text text;
		public string[] para;
		public string Show() {
			if (text == null)
				return "";
			if (para == null || para.Length == 0)
				return text.text;
			return string.Format(text.text, para);
		}
	}
}

