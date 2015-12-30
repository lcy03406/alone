using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIInventory : MonoBehaviour {

	public Transform panel;
	public GameObject itemPrefab;
	public PlayInventory inv;
	public Game game;

	// Use this for initialization
	void Start () {
		//TODO remove the testing code
		inv = new PlayInventory ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void RefreshItems () {
	}

	public void AddItem () {
		PlayItem item = new PlayItem ();
		item.a = Schema.Item.GetA (Schema.Item.ID.Armor);
		inv.items.Add(item);

		GameObject go = Instantiate(itemPrefab);
		Toggle toggle = go.GetComponent<Toggle>();
		Image image = toggle.targetGraphic as Image;
		go.transform.FindChild ("Label").GetComponent<Text>().text = item.a.s.name;
		//itemPrefab.transform.FindChild ("Label").GetComponent<Text>().text = item.a.s.name;
		image.sprite = item.a.s.sprite.s.sprite;
		go.transform.SetParent (panel);
	}
}
