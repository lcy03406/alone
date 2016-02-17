//utf-8。
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

public class UISheet : MonoBehaviour {
	public Dropdown dropdown;
	public Text text;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Menu() {
		switch (dropdown.value) {
			case 0:
				Hide();
				break;
			case 1:
				ShowHelp();
				break;
			case 2:
				ShowWorld();
				break;
			case 3:
				ShowChar();
				break;
			case 4:
				ShowSkill();
				break;
		}
	}

	public void Hide() {
		text.text = "";
		gameObject.SetActive(false);
	}
	public void ShowHelp() {
		text.text =
@"WASD = Move
. = Rest
Enter = Interact
Ctrl = Attack
M = Make
I = Inventory
";
		gameObject.SetActive(true);
	}
	public void ShowWorld() {
		Play.World.Param param = Game.game.world.param;
        text.text = string.Format(@"Time: {0}", param.time);
		gameObject.SetActive(true);
	}
	public void ShowChar() {
		string t = "";
		Play.Creature.Stat stat = Game.game.player.GetAttr<Play.Creature.Stat>();
		foreach (KeyValuePair<Play.Creature.Stat.ID, int> pair in stat.ints) {
			t += pair.Key.ToString() + ": " + pair.Value;
			int cap = stat.Cap(pair.Key);
			if (cap > 0)
				t += " / " + cap;
			t += "\n";
		}
        text.text = t;
		gameObject.SetActive(true);
	}
	public void ShowSkill() {
		text.text = "";
		gameObject.SetActive(true);
	}

	public void Open (string[] texts, UnityAction<int, string> action) {
		gameObject.SetActive (true);
	}

}
