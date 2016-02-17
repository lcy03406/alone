//utf-8。
using System;
using System.Collections.Generic;
using UnityEngine;

using Play;

public class UIGame : MonoBehaviour {
	public static UIGame ui;

	public static UISheet sheet;
	public static UIMenu menu;
	public static UIInventory inv;

	void Awake () {
		ui = this;
		sheet = this.GetComponentInChildren<UISheet>(true);
		sheet.gameObject.SetActive(false);
		menu = this.GetComponentInChildren<UIMenu>(true);
		menu.gameObject.SetActive(false);
		inv = this.GetComponentInChildren<UIInventory>(true);
		inv.gameObject.SetActive(false);
	}

	private static KeyCode[] keys = { KeyCode.Escape, KeyCode.Return, KeyCode.I, KeyCode.M,
		KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D,
		KeyCode.Period, KeyCode.LeftControl };

	public void Update () {
		if (Input.anyKeyDown) {
			foreach (KeyCode key in keys) {
				if (Input.GetKeyDown(key)) {
					OnKeyDown(key);
				}
			}
		}
	}

	public void OnKeyDown(string skey) {
		KeyCode key = (KeyCode)Enum.Parse(typeof(KeyCode), skey);
		OnKeyDown(key);
	}

	public void OnKeyDown(KeyCode key) {
		Play.Entity player = Game.game.player;
		if (player == null)
			return;
		Play.Creature.Ctrl ctrl = player.GetAttr<Play.Creature.Ctrl>();
		if (ctrl == null)
			return;
		if (key == KeyCode.Escape) {
			if (menu.isActiveAndEnabled)
				menu.Close();
			else if (inv.isActiveAndEnabled)
				inv.Close();
		} else if (key == KeyCode.I) {
			if (!inv.isActiveAndEnabled) {
				Inv i = player.GetAttr<Inv>();
				inv.Open(i);
			}
		} else if (menu.isActiveAndEnabled) {

		} else if (key == KeyCode.LeftControl) {
			ctrl.CmdAttack();
			return;
		} else if (key == KeyCode.Period) {
			Schema.Iact.A iact = Schema.Iact.GetA(Schema.Iact.ID.Rest);
			ctrl.CmdIact(iact, WUID.None);
		} else if (key == KeyCode.Return) {
			if (!menu.isActiveAndEnabled) {
				Play.Entity dst = ctrl.ListDst();
				List<Schema.Iact.A> iacts = ctrl.ListIact(dst);
				List<string> opts = iacts.ConvertAll(iact => iact.id.ToString());
				menu.Open(opts.ToArray(), delegate (int idx, string name) {
					Schema.Iact.A iact = iacts[idx];
					ctrl.CmdIact(iact, dst.id);
				});
			}
		} else if (key == KeyCode.M) {
			if (!menu.isActiveAndEnabled) {
				List<Schema.Iact.A> makes = ctrl.ListMake();
				List<string> opts = makes.ConvertAll(make => make.id.ToString());
				menu.Open(opts.ToArray(), delegate (int idx, string name) {
					Schema.Iact.A make = makes[idx];
					ctrl.CmdIact(make, WUID.None);
				});
			}
		} else {
			int dx = 0;
			int dy = 0;
			if (key == KeyCode.A) {
				dx--;
			}
			if (key == KeyCode.S) {
				dy--;
			}
			if (key == KeyCode.D) {
				dx++;
			}
			if (key == KeyCode.W) {
				dy++;
			}
			if (dx != 0 || dy != 0) {
				Direction to = new Coord(dx, dy).ToDirection();
				ctrl.CmdMove(to);
				return;
			}
		}
	}
}
