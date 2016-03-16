using System;
using System.IO;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEditor;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

public class EditSchema : ScriptableWizard {
	public Schema.EditAll all;
	[MenuItem("Revenge/Edit Schema", priority = 3)]
	static void ShowWindow() {
		DisplayWizard<EditSchema>("Schema", "Save&Close", "Save").Load();
	}

	static JsonSerializer ser = Schema.EditAll.Ser();

	bool loaded = false;

	void Load() {
		loaded = false;
		AssetDatabase.Refresh();
		TextAsset text = Resources.Load<TextAsset>("schema");
		using (StringReader sr = new StringReader(text.text))
		using (JsonReader r = new JsonTextReader(sr)) {
			try {
				all = ser.Deserialize<Schema.EditAll>(r);
				loaded = true;
			} catch (Exception) {
				loaded = false;
				Close();
			}
		}
	}

	const string path = "/Resources/schema.txt";
	void Save() {
		if (!loaded)
			return;
		using (StringWriter sw = new StringWriter())
		using (JsonWriter w = new JsonTextWriter(sw)) {
			ser.Serialize(w, all);
			File.WriteAllText(Application.dataPath + path, sw.ToString());
		}
		AssetDatabase.Refresh();
	}

	void OnWizardCreate() {
		Save();
	}

	void OnWizardOtherButton() {
		Save();
	}
}

