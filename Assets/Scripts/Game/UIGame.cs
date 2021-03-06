//utf-8。
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Play;
using Play.Acts;
using Play.Attrs;

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

	private static KeyCode[] keys = {
		KeyCode.Escape, KeyCode.Return, KeyCode.Tab,
		KeyCode.I, KeyCode.M,
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
		if (Game.game.player == null)
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
			string[] acts = { "Rest", "Make", "Interact" };
			UnityAction[] actions = { Rest, Make, Interact };
			menu.Open(acts, delegate (int idx, string name) {
				actions[idx]();
			});
		} else if (key == KeyCode.Period) {
			Rest();
		} else if (key == KeyCode.M) {
			Make();
		} else if (key == KeyCode.Return) {
			Interact();
		} else {
			Move(key);
		}
	}

	private void EnqueAct(Act act) {
		Entity player = Game.game.player;
		Ctrl ctrl = player.GetAttr<Ctrl>();
		ctrl.Enque(act);
	}

	static EntitySelect SelectDst = new EntitySelect() { iact_dst = 1 };
	public List<Entity> ListDst() {
		Entity player = Game.game.player;
		Pos pos = player.GetAttr<Pos>();
		Coord c = pos.c.Step(pos.dir);
		return player.layer.SearchEntity(c, SelectDst);
	}

	private void Rest() {
		if (!menu.isActiveAndEnabled) {
			Schema.Iact.A iact = Schema.Iact.GetA(Schema.ActionID.Rest);
			EnqueAct(new ActIact(iact, null));
		}
	}

	private bool OpenMenuDst(bool auto) {
		List<Entity> dsts = ListDst();
		if (dsts.Count == 0) {
			return false;
		}
		if (dsts.Count == 1) {
			OpenMenuIact(dsts[0], auto);
		} else {
			MenuDst mn = new MenuDst();
			mn.dsts = dsts;
			mn.auto = auto;
			mn.Open();
		}
		return true;
	}

	private class MenuDst {
		public List<Entity> dsts;
		public bool auto;
		int idx;
		private void MenuCall(int idx, string name) {
			this.idx = idx;
			Entity dst = dsts[idx];
			ui.OpenMenuIact(dst, auto);
		}
		public void Open() {
			List<string> opts = new List<string>();
			foreach (Entity ent in dsts) {
				opts.Add(ent.GetName());
			}
			menu.Open(opts.ToArray(), MenuCall);
		}
	}

	private void OpenMenuIact(Entity dst, bool auto) {
		Entity player = Game.game.player;
		Play.Attrs.Core dstcore = dst.GetAttr<Play.Attrs.Core>();
		if (auto) {
			Schema.Iact.A aact = dstcore.GetIactAuto(player);
			if (aact != null) {
				OpenMsgIact(aact, dst);
				return;
			}
		}
		List<Schema.Iact.A> iacts = dstcore.ListIact(player);
		if (iacts.Count > 0) {
			MenuIact mn = new MenuIact();
			mn.iacts = iacts;
            mn.dst = dst;
			mn.Open();
		}
	}

	private class MenuIact {
		public List<Schema.Iact.A> iacts;
		public Entity dst;
		int idx;
		private void MenuCall(int idx, string name) {
			this.idx = idx;
            Schema.Iact.A iact = iacts[idx];
			ui.OpenMsgIact(iact, dst);
		}
		public void Open() {
			List<string> opts = new List<string>();
			foreach (Schema.Iact.A make in iacts) {
				if (make.s != null) {
					opts.Add(make.s.name);
				}
			}
			menu.Open(opts.ToArray(), MenuCall);
		}
	}

	private void OpenMsgIact(Schema.Iact.A iact, Entity dst) {
		if (iact.s.cat == Schema.ActionCategoryID.Make || iact.s.cat == Schema.ActionCategoryID.Build) {
			MsgIact m = new MsgIact();
			m.iact = iact;
			m.dst = dst;
			m.Open();
		} else {
			EnqueAct(new ActIact(iact, dst));
		}
	}

	private class MsgIact {
		public Schema.Iact.A iact;
		public Play.Entity dst;
		private void MsgCall(bool yes) {
			if (yes) {
				ui.EnqueAct(new ActIact(iact, dst));
			}
		}
		public void Open() {
			Ctx ctx = new Ctx(Game.game.player.layer, Game.game.player);
			UIMsg.Style st = iact.Can(ctx) ? UIMsg.Style.OkCancel : UIMsg.Style.Cancel;
			msg.Open(iact.Display(), st, MsgCall);
		}
	}

	private void Make() {
		Entity player = Game.game.player;
		if (!menu.isActiveAndEnabled) {
			MenuIact mn = new MenuIact();
			Core core = player.GetAttr<Core>();
			mn.iacts = core.ListMake();
			mn.Open();
		}
	}

	private void Interact() {
		if (!menu.isActiveAndEnabled) {
			if (!OpenMenuDst(false)) {
				Make();
			}
		}
	}

	private void Move(KeyCode key) {
		Entity player = Game.game.player;
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
				Pos pos = player.GetAttr<Pos>();
				Coord toc = pos.c.Step(to);
				if (to != pos.dir) {
					EnqueAct(new ActIact(Schema.Iact.GetA(Schema.ActionID.Dir), toc));
				} else {
					if (player.layer.CanMoveTo(toc)) {
						//auto pick
						List<Entity> dsts = ListDst();
						foreach (Entity dst in dsts) {
							Core dstcore = dst.GetAttr<Core>();
							Schema.Iact.A aact = dstcore.GetIactAuto(player);
							if (aact != null) {
								if (aact.s.cat == Schema.ActionCategoryID.Pick && aact.s.time1 + aact.s.time2 == 0) {
									EnqueAct(new ActIact(aact, dst));
								}
							}
						}
						EnqueAct(new ActIact(Schema.Iact.GetA(Schema.ActionID.Move), toc));
					} else {
						OpenMenuDst(true);
					}
				}
			}
		}
	}
}
