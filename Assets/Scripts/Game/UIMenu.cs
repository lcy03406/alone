//utf-8。
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

	private static KeyCode[] keys = {
		KeyCode.Alpha1,
		KeyCode.Alpha2,
		KeyCode.Alpha3,
		KeyCode.Alpha4,
		KeyCode.Alpha5,
		KeyCode.Alpha6,
		KeyCode.Alpha7,
		KeyCode.Alpha8,
		KeyCode.Alpha9,
	};

	// Update is called once per frame
	void Update () {
		if (Input.anyKeyDown) {
			for (int i = 0; i < keys.Length; ++i) {
				if (Input.GetKeyDown(keys[i])) {
					OnKeyDown(i);
				}
			}
		}
	}

	void OnKeyDown(int index) {
		if (index < buttons.Count) {
			Button button = buttons[index].GetComponent<Button>();
			button.onClick.Invoke();
		}
	}

	public Button AddButton (string text) {
		GameObject go = Instantiate (buttonPrefab);
		go.name = "Menu/" + text;
		go.transform.FindChild ("Text").GetComponent<Text> ().text = text;
		go.transform.SetParent (transform);
		buttons.Add (go);
		Button b = go.GetComponent<Button> ();
		return b;
	}

	public void Open (string[] texts, UnityAction<int, string> action) {
		gameObject.SetActive (true);
		for (int i = 0; i < texts.Length; ++i) {
			int idx = i;
			string text = texts[i];
			AddButton (text).onClick.AddListener (delegate () {
				Close ();
				action (idx, text);
			});
		}
		AddButton ("Cancel").onClick.AddListener (delegate () {
			Close ();
		});
	}

    public void Close () {
		gameObject.SetActive(false);
		foreach (GameObject go in buttons) {
			Destroy (go);
		}
		buttons.Clear ();
	}
}
