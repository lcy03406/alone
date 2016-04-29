using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEditor;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Reflection;
using Vexe.Runtime.Types;
using Vexe.Editor.Editors;

public enum TestTag {
	Test1,
	Test2,
}

[Serializable]
public abstract class Test {
	public int test_data;
}

[Serializable]
public class Test1 : Test {
	public int test_data1;
}
[Serializable]
public class Test2 : Test {
	public string test_data2;
	public Test1 test1_data;
	public Test test;
}

[Serializable]
public class TestData {
	public int _tag;
	public Test test;
	public Test1 test1;
	public Test2 test2;

	public TestData() {
		_tag = 101;
	}
}
/*
[CustomPropertyDrawer(typeof(TestData))]
public class TestDataDrawer : PropertyDrawer {
	string[] options = null;
	public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
		if (options == null) {
			BuildOptions(property);
		}
		float lineHeight = EditorGUIUtility.singleLineHeight;
		SerializedProperty tag = property.FindPropertyRelative("_tag");
		SerializedProperty field = property.FindPropertyRelative(options[tag.intValue]);
		return EditorGUI.GetPropertyHeight(field, label) + lineHeight;
	}
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
		float lineHeight = EditorGUIUtility.singleLineHeight;
		EditorGUI.BeginProperty(position, label, property);
		Rect tagRect = new Rect(position.x, position.y, position.width, lineHeight);
		Rect fieldRect = new Rect(position.x, position.y + lineHeight, position.width, position.height - lineHeight);
		SerializedProperty tag = property.FindPropertyRelative("_tag");
		tag.intValue = EditorGUI.Popup(tagRect, tag.intValue, options);
		string name = options[tag.intValue];
		SerializedProperty field = property.FindPropertyRelative(name);
		EditorGUI.PropertyField(fieldRect, field, label, true);
		EditorGUI.EndProperty();
	}
	private void BuildOptions(SerializedProperty property) {
		Type tp = Helper.GetTargetObjectOfProperty(property).GetType();
		FieldInfo[] props = tp.GetFields();
		List<string> list = new List<string>();
		foreach (FieldInfo prop in props) {
			string name = prop.Name;
			if (name[0] != '_') {
				list.Add(name);
			}
		}
		options = list.ToArray();
	}
}


[Serializable]
public class TestAll {
	public TestData testData;
}
public class EditTestSchema : ScriptableWizard {
	public TestAll all;
	[MenuItem("Revenge/Edit Test Schema", priority = 3)]
	static void ShowWindow() {
		DisplayWizard<EditTestSchema>("Schema", "Save&Close", "Save").Load();
	}

	static JsonSerializer ser = Schema.EditAll.Ser();

	bool loaded = false;

	void Load() {
		loaded = false;
		AssetDatabase.Refresh();
		TextAsset text = Resources.Load<TextAsset>("test_schema");
		using (StringReader sr = new StringReader(text.text))
		using (JsonReader r = new JsonTextReader(sr)) {
			try {
				all = ser.Deserialize<TestAll>(r);
				loaded = true;
			} catch (Exception) {
				loaded = false;
				Close();
			}
		}
	}

	const string path = "/Resources/test_schema.txt";
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
*/


[Serializable]
public class TestAll : BaseScriptableObject {
	public TestData testData;
}
/*
public class EditTestSchema : EditorWindow {
	TestAll all;
	Editor editor;
	[MenuItem("Revenge/Edit Test Schema", priority = 3)]
	static void ShowWindow() {
		GetWindow<EditTestSchema>().Show();
	}

	void OnEnable() {
		Load();
		editor = Editor.CreateEditor(all);
	}

	void OnGUI() {
		if (editor) {
			editor.OnInspectorGUI();
		}
	}
}
*/
public class EditTestSchema : ScriptableWizard {
	TestAll all;
	Editor editor;
	[MenuItem("Revenge/Edit Test Schema", priority = 3)]
	static void ShowWindow() {
		DisplayWizard<EditTestSchema>("Schema", "Save&Close", "Save").Load();
	}

	static JsonSerializer ser = Schema.EditAll.Ser();

	bool loaded = false;

	void Load() {
		loaded = false;
		AssetDatabase.Refresh();
		TextAsset text = Resources.Load<TextAsset>("test_schema");
		using (StringReader sr = new StringReader(text.text))
		using (JsonReader r = new JsonTextReader(sr)) {
			try {
				all = CreateInstance<TestAll>();
				all.testData = ser.Deserialize<TestData>(r);
				editor = Editor.CreateEditor(all);
				BaseEditor b = editor as BaseEditor;
				b.ShowScriptHeader = false;
				loaded = true;
			} catch (Exception) {
				loaded = false;
				Close();
			}
		}
	}

	const string path = "/Resources/test_schema.txt";
	void Save() {
		if (!loaded)
			return;
		using (StringWriter sw = new StringWriter())
		using (JsonWriter w = new JsonTextWriter(sw)) {
			ser.Serialize(w, all.testData);
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

	protected override bool DrawWizardGUI() {
		editor.OnInspectorGUI();
		return false;
	}
}
[Serializable]
public class CsvAll : BaseScriptableObject {
	public List<DropTable> dropTable = new List<DropTable>();
}
[Serializable]
public class DropTable {
	public int ID;
	public string Name;
	public string ItemName;
	public string ItemDes;
	[Serializable]
	public struct DropItem {
		public int ID;
		public int Num;
	}
	public List<DropItem> Item;
}

public class CsvTest : ScriptableWizard {
	public CsvAll all;
	Editor editor;
	[MenuItem("Revenge/Test Csv Loader", priority = 199)]
	static void TestCsv() {
		DisplayWizard<CsvTest>("Test", "Save&Close", "Save").Load();
	}
	private void Load() {
		TextAsset text = Resources.Load<TextAsset>("test_csv");
		using (StringReader reader = new StringReader(text.text)) {
			all = CreateInstance<CsvAll>();
			Utility.Table.Load(reader, all.dropTable);
			editor = Editor.CreateEditor(all);
			BaseEditor b = editor as BaseEditor;
			b.ShowScriptHeader = false;
		}
	}
	protected override bool DrawWizardGUI() {
		editor.OnInspectorGUI();
		return false;
	}
}
