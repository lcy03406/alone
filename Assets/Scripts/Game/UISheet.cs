//utf-8。
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

using Stat = Play.Attrs.Stat;
using System.Text;

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
		if (update_time < Game.game.world_update_time) {
			Menu();
		}
	}

	public void Menu() {
		update_time = Game.game.world_update_time;
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
		foreach (KeyValuePair<Edit.AStat, Stat.St> pair in stat.ints) {
			t += pair.Key.ToString() + ": " + pair.Value.value;
			int cap = pair.Value.cap;
			if (cap > 0)
				t += " / " + cap;
			t += "\n";
		}
        text.text = t;
		gameObject.SetActive(true);
	}
	public void ShowLog() {
		List<string> logs = Game.game.world.logs;
		int begin = logs.Count - 10;
		if (begin < 0)
			begin = 0;
		StringBuilder sb = new StringBuilder();
		for (int i = begin; i < logs.Count; ++i) {
			sb.AppendLine(logs[i]);
		}
		text.text = sb.ToString();
		gameObject.SetActive(true);
	}
	public void ShowInventory() {
		UIGame.ui.OnKeyDown(KeyCode.I); //TODO
	}
}
