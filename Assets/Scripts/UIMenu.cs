using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

public class UIMenu : MonoBehaviour {

	public GameObject buttonPrefab;

	private List<GameObject> buttons = new List<GameObject> ();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public Button AddButton (string text) {
		GameObject go = Instantiate (buttonPrefab);
		go.transform.FindChild ("Label").GetComponent<Text> ().text = text;
		go.transform.SetParent (transform);
		buttons.Add (go);
		Button b = go.GetComponent<Button> ();
		return b;
	}

	public void Open (string[] texts, UnityAction<int, string> action) {
		for (int i = 0; i < texts.Length; ++i) {
			string text = texts[i];
			AddButton (text).onClick.AddListener (delegate () {
				Close ();
				action (i, text);
			});
		}
	}

	public void Close () {
		foreach (GameObject go in buttons) {
			Destroy (go);
		}
		buttons.Clear ();
	}
}
