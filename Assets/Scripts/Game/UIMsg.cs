using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIMsg : MonoBehaviour {

	Text text;
	Button left;
	Button right;

	UnityAction<bool> action;

	void Start() {
		text = transform.FindChild("Text").GetComponent<Text>();
		left = transform.FindChild("ButtonLeft").GetComponent<Button>();
		right = transform.FindChild("ButtonRight").GetComponent<Button>();
	}

	public void Open(string text, UnityAction<bool> action) {
		this.text.text = text;
		this.action = action;
		gameObject.SetActive(true);
		left.onClick.AddListener(No);
		right.onClick.AddListener(Yes);
	}

	private void Close() {
		gameObject.SetActive(false);
		left.onClick.RemoveAllListeners();
		right.onClick.RemoveAllListeners();
		action = null;
	}

	public void Yes() {
		UnityAction<bool> action = this.action;
		Close();
		if (action != null)
			action(true);
	}
	public void No() {
		UnityAction<bool> action = this.action;
		Close();
		if (action != null)
			action(false);
	}
}
