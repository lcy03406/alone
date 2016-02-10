//utf-8。
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

using Play;

public class UIInventory : MonoBehaviour {

	public GameObject itemPrefab;

	private Transform panel;
	private List<GameObject> items = new List<GameObject>();

	void Awake () {
		panel = transform.FindChild("ItemPanel");
	}

	// Use this for initialization
	void Start () {		
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void Open (Inventory inv) {
		foreach (Item item in inv.items) {
			AddItem (item);
		}
		gameObject.SetActive(true);
	}

	void AddItem (Item item) {
		GameObject go = Instantiate(itemPrefab);
		Toggle toggle = go.GetComponent<Toggle>();
		Image image = toggle.targetGraphic as Image;
		go.transform.FindChild ("Label").GetComponent<Text>().text = item.a.s.name;
		image.sprite = item.a.s.sprite.s.sprite;
		go.transform.SetParent (panel);
		items.Add(go);
    }

	public void Close() {
		gameObject.SetActive(false);
		foreach (GameObject go in items) {
			Destroy(go);
		}
		items.Clear();
	}
}
