//utf-8。
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

using Play;

public class UIGame : MonoBehaviour {
	public static UIGame ui;

	public static UIMenu menu;
	public static UIInventory inv;

	void Awake () {
		ui = this;
		menu = this.GetComponentInChildren<UIMenu>(true);
		menu.gameObject.SetActive(false);
		inv = this.GetComponentInChildren<UIInventory>(true);
		inv.gameObject.SetActive(false);
	}

	void Update () {
		Play.Entity player = Game.game.player;
		if (player == null)
			return;
		Play.Creature.Ctrl ctrl = player.GetAttr<Play.Creature.Ctrl> ();
		if (ctrl == null)
			return;
		if (Input.anyKeyDown) {
			if (Input.GetKeyDown(KeyCode.Escape)) {
				if (menu.isActiveAndEnabled)
					menu.Close();
				else if (inv.isActiveAndEnabled)
					inv.Close();
			} else if (Input.GetKeyDown(KeyCode.I)) {
				if (!inv.isActiveAndEnabled) {
					Inv i = player.GetAttr<Inv>();
					inv.Open(i);
				}
			} else if (menu.isActiveAndEnabled) {

			} else if (Input.GetKeyDown(KeyCode.LeftControl)) {
				ctrl.CmdAttack();
				return;
			} else if (Input.GetKeyDown(KeyCode.Period)) {
				//ctrl.CmdWait ();
			} else if (Input.GetKeyDown(KeyCode.Return)) {
				if (!menu.isActiveAndEnabled) {
					Play.Entity dst = ctrl.ListDst();
					List<Schema.Iact.A> iacts = ctrl.ListIact(dst);
					List<string> opts = iacts.ConvertAll(iact => iact.id.ToString());
					menu.Open(opts.ToArray(), delegate (int idx, string name) {
						Schema.Iact.A iact = iacts[idx];
						ctrl.CmdIact(iact, dst.id);
					});
				}
			} else if (Input.GetKeyDown(KeyCode.M)) {
				if (!menu.isActiveAndEnabled) {
					List<Schema.Make.A> makes = ctrl.ListMake();
					List<string> opts = makes.ConvertAll(make => make.id.ToString());
					menu.Open(opts.ToArray(), delegate (int idx, string name) {
						Schema.Make.A make = makes[idx];
						ctrl.CmdMake(make);
					});
				}
			} else {
				int dx = 0;
				int dy = 0;
				if (Input.GetKeyDown(KeyCode.A)) {
					dx--;
				}
				if (Input.GetKeyDown(KeyCode.S)) {
					dy--;
				}
				if (Input.GetKeyDown(KeyCode.D)) {
					dx++;
				}
				if (Input.GetKeyDown(KeyCode.W)) {
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
}
