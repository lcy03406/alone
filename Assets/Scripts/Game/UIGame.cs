//utf-8。
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Play;

public class UIGame : MonoBehaviour {
	public static UIGame ui;

	public static UISheet sheet;
	public static UIMenu menu;
	public static UIInventory inv;
	public static UIMsg msg;

	void Awake() {
		ui = this;
		sheet = this.GetComponentInChildren<UISheet>(true);
		sheet.gameObject.SetActive(false);
		menu = this.GetComponentInChildren<UIMenu>(true);
		menu.gameObject.SetActive(false);
		inv = this.GetComponentInChildren<UIInventory>(true);
		inv.gameObject.SetActive(false);
		msg = this.GetComponentInChildren<UIMsg>(true);
		msg.gameObject.SetActive(false);
	}

	private static KeyCode[] keys = { KeyCode.Escape, KeyCode.Return, KeyCode.I, KeyCode.M,
		KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D,
		KeyCode.Period, KeyCode.LeftControl };

	public void Update() {
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
		if (GetCtrl() == null)
			return;
		if (key == KeyCode.Escape) {
			if (msg.isActiveAndEnabled)
				msg.No();
			if (menu.isActiveAndEnabled)
				menu.Close();
			else if (inv.isActiveAndEnabled)
				inv.Close();
		} else if (key == KeyCode.Return && msg.isActiveAndEnabled) {
			msg.Yes();
		} else if (key == KeyCode.I) {
			if (inv.isActiveAndEnabled) {
				inv.Close();
			} else {
				Play.Attrs.Inv i = Game.game.player.GetAttr<Play.Attrs.Inv>();
				inv.Open(i);
			}
		} else if (menu.isActiveAndEnabled) {

		} else if (key == KeyCode.Tab) {
			string[] acts = { "Rest", "Make", "Interact", "Attack" };
			UnityAction[] actions = { Rest, Make, Interact, Attack };
			menu.Open(acts, delegate (int idx, string name) {
				actions[idx]();
			});
		} else if (key == KeyCode.Period) {
			Rest();
		} else if (key == KeyCode.M) {
			Make();
		} else if (key == KeyCode.Return) {
			Interact();
		} else if (key == KeyCode.LeftControl) {
			Attack();
		} else {
			Move(key);
		}
	}

	private Play.Attrs.Ctrl GetCtrl() {
		Entity player = Game.game.player;
		if (player == null)
			return null;
		Play.Attrs.Ctrl ctrl = player.GetAttr<Play.Attrs.Ctrl>();
		return ctrl;
	}

	public void Rest() {
		Play.Attrs.Ctrl ctrl = GetCtrl();
		if (!menu.isActiveAndEnabled) {
			Schema.Iact.A iact = Schema.Iact.GetA(Schema.ActionID.Rest);
			ctrl.CmdIact(iact, WUID.None);
		}
	}

	class MakeMenu {
		public List<Schema.Iact.A> makes;
		int idx;
		public void MenuCall(int idx, string name) {
			this.idx = idx;
			Schema.Iact.A make = makes[idx];
			Ctx ctx = new Ctx(Game.game.player.layer, Game.game.player);
			UIMsg.Style st = make.Can(ctx) ? UIMsg.Style.OkCancel : UIMsg.Style.Cancel;
			msg.Open(make.Display(), st, MsgCall);
		}
		public void MsgCall(bool yes) {
			Schema.Iact.A make = makes[idx];
			if (yes) {
				ui.GetCtrl().CmdIact(make, WUID.None);
			}
		}
		public void Open() {
			List<string> opts = makes.ConvertAll(make => make.s.name);
			menu.Open(opts.ToArray(), MenuCall);
		}
	}
	
	public void Make() {
		Play.Attrs.Ctrl ctrl = GetCtrl();
		if (!menu.isActiveAndEnabled) {
			MakeMenu makeMenu = new MakeMenu();
			makeMenu.makes = ctrl.ListMake();
			makeMenu.Open();
		}
	}
	public void Interact() {
		Play.Attrs.Ctrl ctrl = GetCtrl();
		if (!menu.isActiveAndEnabled) {
			Play.Entity dst = ctrl.ListDst();
			if (dst == null)
				return;
			List<Schema.Iact.A> iacts = ctrl.ListIact(dst);
			List<string> opts = iacts.ConvertAll(iact => iact.s.name);
			menu.Open(opts.ToArray(), delegate (int idx, string name) {
				Schema.Iact.A iact = iacts[idx];
				ctrl.CmdIact(iact, dst.id);
			});
		}
	}

	public void Attack() {
		Play.Attrs.Ctrl ctrl = GetCtrl();
		if (!menu.isActiveAndEnabled) {
			ctrl.CmdAttack();
		}
	}
	public void Move(KeyCode key) {
		Play.Attrs.Ctrl ctrl = GetCtrl();
		if (!menu.isActiveAndEnabled) {
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
			}
		}
	}
}
