//utf-8。
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

using Play;
using Play.Attrs;

public class UIInventory : MonoBehaviour {

	public GameObject itemPrefab;

	private int update_time = 0;
	private Inv inv = null;
	private Transform panel;
	private List<GameObject> items = new List<GameObject>();

	void Awake () {
		panel = transform.FindChild("ItemPanel").FindChild("ItemList");
	}

	// Use this for initialization
	void Start () {		
	}

	// Update is called once per frame
	void Update() {
		if (update_time < Game.game.world_update_time) {
			Inv i = inv;
			Close();
			Open(i);
		}
	}

	public void Open (Inv inv) {
		this.inv = inv;
		update_time = Game.game.world_update_time;
        gameObject.SetActive(true);
		foreach (Item item in inv.items) {
			AddItem (item);
		}
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
		inv = null;
	}
}
