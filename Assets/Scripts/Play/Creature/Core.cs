//utf-8。
using System;
using System.Collections.Generic;

namespace Play.Creature {
	[Serializable]
	public class Core : Play.Core {
		public Schema.Creature.A race;
		public int acstep;
		public int actime;
		public Act act;

		public static Entity CreateEntity (World world, Schema.Creature.A race) {
			Core core = new Core {
				race = race,
				acstep = 0,
				actime = 0,
				act = null,
			};
			Entity e = new Entity {
				id = world.NextWUID (),
			};
			e.SetAttr (core);
			e.SetAttr (new Show ());
			e.SetAttr(new Stat(race.s.born_stat));
			e.SetAttr (new Inv ());
			AI ai = (AI)Activator.CreateInstance (race.s.ai);
			e.SetAttr (ai);
			e.SetWorld (world);
			return e;
		}

		static Schema.Iact.A[] iacts = {
			//TODO
		};

		public override List<Schema.Iact.A> ListIact (Entity src) {
			List<Schema.Iact.A> list = new List<Schema.Iact.A> ();
			foreach (Schema.Iact.A iact in iacts) {
				Ctx ctx = new Ctx() {
					src = src,
					dst = ent,
				};
				if (iact.s.i.Can (ctx)) {
					list.Add (iact);
				}
			}
			return list;
		}

		public override int NextTick () {
			return actime;
		}

        public override void Tick (int time) {
			while (time >= actime) {
				AI ai = ent.GetAttr<AI> ();
				if (act == null && ai != null) {
					Act t = ai.NextAct ();
					if (t != null && t.Can (ent)) {
						this.act = t;
						acstep = -1;
					}
				}
				if (act == null)
					break;
				acstep++;
				Act.Step step = act.GetStep (acstep);
				if (step == null) {
					act = null;
				} else {
					actime = time + step.Time (ent);
					step.Do (ent);
				}
			}
		}

		public List<Schema.Iact.A> ListMake() {
			foreach (Schema.Iact.A a in race.s.know_make) {
			}
			return new List<Schema.Iact.A>(race.s.know_make);
		}
	}
}
