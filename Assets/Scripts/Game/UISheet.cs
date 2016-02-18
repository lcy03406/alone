//utf-8。
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

using Stat = Play.Attrs.Stat<Play.Stats.Creature>;

public class UISheet : MonoBehaviour {
	public Dropdown dropdown;
	public Text text;

	private int update_time = 0;
	private int show_tab = 0;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (update_time < Game.game.world.param.time) {
			Menu();
		}
	}

	public void Menu() {
		update_time = Game.game.world.param.time;
		int cur_tab = dropdown.value;
        switch (cur_tab) {
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
				ShowInventory();
				dropdown.value = show_tab;
                return;
		}
		show_tab = cur_tab;
    }

	public void Hide() {
		text.text = "";
		gameObject.SetActive(false);
	}
	public void ShowHelp() {
		text.text =
@"WASD = Move
Tab = Menu
. = Rest
M = Make
Enter = Interact
Ctrl = Attack
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
		Stat stat = Game.game.player.GetAttr<Stat>();
		foreach (KeyValuePair<Play.Stats.Creature, int> pair in stat.ints) {
			t += pair.Key.ToString() + ": " + pair.Value;
			int cap = stat.Cap(pair.Key);
			if (cap > 0)
				t += " / " + cap;
			t += "\n";
		}
        text.text = t;
		gameObject.SetActive(true);
	}
	public void ShowInventory() {
		UIGame.ui.OnKeyDown(KeyCode.I); //TODO
	}
}
