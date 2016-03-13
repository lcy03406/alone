using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEditor;
using System.Text;

[Serializable]
public class EditEnum {
	public string name;
	public List<string> values = new List<string>();
}

public class EnumText {
	public List<EditEnum> all = new List<EditEnum>();

	const string header = "//generated. do not edit.";
	const string ns = "namespace Schema {";
	const string start = "\tpublic enum ";
	const string va = "\t\t";
	const string end = "\t}";
	const string ens = "}";

	public void Clear() {
		all.Clear();
	}

	public void Load(string[] lines) {
		if (lines == null || lines.Length == 0) {
			return;
		}
		Assert.AreEqual(lines[0], header);
		if (lines[0] != header) {
			return;
		}
		EditEnum e = null;
		foreach (string line in lines) {
			if (line.StartsWith("//")) {
				continue;
			} else if (line == ns) {
				continue;
			} else if (line.StartsWith(start)) {
				e = new EditEnum();
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

	public string[] Save() {
		List<string> lines = new List<string>();
		lines.Add(header);
		lines.Add(ns);
		foreach (EditEnum e in all) {
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

public class EnumEditor : ScriptableWizard {
	EnumText data = new EnumText();
	[MenuItem("Revenge/Edit Schema ID", priority = 2)]
	static void ShowEditSchemaID() {
		DisplayWizard<EnumEditor>("Schema", "Save&Close", "Save").Load("/Scripts/Schema/AllEditID.cs");
	}

	[MenuItem("Revenge/Edit Enum", priority = 1)]
	static void ShowEditEnum() {
		DisplayWizard<EnumEditor>("Enum", "Save&Close", "Save").Load("/Scripts/Schema/AllEnum.cs");
	}

	private string path;

	void Load(string path) {
		this.path = path;
		data.Clear();
		AssetDatabase.Refresh();
		TextAsset text = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets" + path);
		string[] lines = text.text.Split('\n');
		data.Load(lines);
	}

	void Save() {
		string[] lines = data.Save();
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

