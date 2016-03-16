using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEditor;
using System.Text;

[Serializable]
public class EditEnumText {
	public string name;
	public List<string> values = new List<string>();
}

public class EditEnumFile {
	public List<EditEnumText> all;

	const string header = "//generated. do not edit.";
	const string ns = "namespace Schema {";
	const string start = "\tpublic enum ";
	const string va = "\t\t";
	const string end = "\t}";
	const string ens = "}";

	public EditEnumFile() {
		this.all = new List<EditEnumText>();
	}
	public EditEnumFile(List<EditEnumText> all) {
		this.all = all;
	}

	public void ClearEnums() {
		all.Clear();
	}

	public void LoadEnums(string[] lines) {
		if (lines == null || lines.Length == 0) {
			return;
		}
		Assert.AreEqual(lines[0], header);
		if (lines[0] != header) {
			return;
		}
		EditEnumText e = null;
		foreach (string line in lines) {
			if (line.StartsWith("//")) {
				continue;
			} else if (line == ns) {
				continue;
			} else if (line.StartsWith(start)) {
				e = new EditEnumText();
				e.name = line.TrimEnd('{', ' ').Substring(start.Length);
			} else if (line.StartsWith(va)) {
				string value = line.TrimEnd(',', ' ').Substring(va.Length);
				e.values.Add(value);
			} else if (line == end) {
				if (e != null) {
					all.Add(e);
				}
			} else if (line == ens) {
				continue;
			} else if (line == "") {
				continue;
			} else {
				Assert.IsTrue(false, "enexpected line: " + line);
			}
		}
	}

	public string[] SaveEnums() {
		List<string> lines = new List<string>();
		lines.Add(header);
		lines.Add(ns);
		foreach (EditEnumText e in all) {
			lines.Add(start + e.name + "{");
			foreach (string value in e.values) {
				lines.Add(va + value + ",");
			}
			lines.Add(end);
		}
		lines.Add(ens);
		return lines.ToArray();
	}
}

public class EditEnumBase : ScriptableWizard {
	public List<EditEnumText> all = new List<EditEnumText>();

	private string path;

	protected void Load(string path) {
		this.path = path;
		all.Clear();
		AssetDatabase.Refresh();
		TextAsset text = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets" + path);
		string[] lines = text.text.Split('\n');
		new EditEnumFile(all).LoadEnums(lines);
	}

	void Save() {
		string[] lines = new EditEnumFile(all).SaveEnums();
		File.WriteAllLines(Application.dataPath + path, lines);
		AssetDatabase.Refresh();
	}

	void OnWizardCreate() {
		Save();
	}

	void OnWizardOtherButton() {
		Save();
	}
}

public class EditEnumEnum : EditEnumBase {
	[MenuItem("Revenge/Edit Enum", priority = 1)]
	static void ShowEditEnum() {
		DisplayWizard<EditEnumEnum>("Edit Enum", "Save&Close", "Save").Load("/Scripts/Schema/AllEditEnum.cs");
	}
}
public class EditEnumID : EditEnumBase {
	[MenuItem("Revenge/Edit Schema ID", priority = 2)]
	static void ShowEditSchemaID() {
		DisplayWizard<EditEnumID>("Edit Schema ID", "Save&Close", "Save").Load("/Scripts/Schema/AllEditID.cs");
	}
}


