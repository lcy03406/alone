//utf-8。
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

public class UISheet : MonoBehaviour {
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Open (string[] texts, UnityAction<int, string> action) {
		gameObject.SetActive (true);
	}

    public void Close () {
		gameObject.SetActive(false);
	}
}
