using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class UIMsg : MonoBehaviour {

	public Text text;
	public Button left;
	public Button right;

	UnityAction<bool> action;

	void Start() {
		//text = transform.FindChild("Text").GetComponent<Text>();
		//left = transform.FindChild("ButtonLeft").GetComponent<Button>();
		//right = transform.FindChild("ButtonRight").GetComponent<Button>();
	}

	public enum Style {
		Ok = 1,
		Cancel = 2,
		OkCancel = 3,
	}

	public void Open(string text, Style style, UnityAction<bool> action) {
		this.text.text = text;
		this.action = action;
		left.gameObject.SetActive((style & Style.Cancel) != 0);
		right.gameObject.SetActive((style & Style.Ok) != 0);
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
