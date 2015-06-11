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
		item.id = Scheme.Item.ID.Armor;
		inv.items.Add(item);

		GameObject go = Instantiate(itemPrefab);
		Toggle toggle = go.GetComponent<Toggle>();
		Image image = toggle.targetGraphic as Image;
		Scheme.Item si = game.world.scheme.GetItem(item.id);
		go.transform.FindChild ("Label").GetComponent<Text>().text = si.name;
		itemPrefab.transform.FindChild ("Label").GetComponent<Text>().text = si.name;
		image.sprite = si.sprite;
		go.transform.SetParent (panel);
	}
}
