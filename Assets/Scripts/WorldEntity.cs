using System;

public class WorldEntity {

	[Serializable]
	public class Data {
		public WUID id;
		public Coord c;
		public Direction dir;
		public int acstep;
		public int actime;
		public Schema.Spec.A spec;
		public EntityStat stat;
		public PlayInventory inv;
		public EntityAct act;
		public PlayAI ai;
	}

	public static Data Create(World world, Schema.Spec.ID race) {
		//TODO
		Data d = new Data ();
		d.spec = Schema.Spec.GetA (race);
		d.id = world.NextWUID ();
		d.stat = new EntityStat ();
		d.stat.hp = 3;
		d.inv = new PlayInventory ();
		d.act = null;
		d.ai = (PlayAI) Activator.CreateInstance (d.spec.s.ai);
		return d;
	}

	public World world;
	//private WorldGrid grid;

	public Data d;

	public bool isPlayer = false;
	
	public WorldEntity (World world_, Data d_) {
		world = world_;
		//this.grid = null;
		d = d_;
		d.ai.ent = this;
		isPlayer = true;
	}

	public WorldEntity (World world_, WorldGrid g_, Data d_) : this(world_, d_) {
		//this.grid = g;
		isPlayer = false;
	}


	public Data Save () {
		return d;
	}

	public void Load () {
		if (d.act != null) {
			d.act.Load (this);
		}
	}

	public void Update (int time) {
		UpdateAct (time);
	}

	public int NextUpdateTime () {
		return d.actime;
	}

	public void UpdateAct (int time) {
		while (time >= d.actime) {
			if (d.act == null && d.ai != null) {
				EntityAct act = d.ai.NextAct ();
				if (act != null && act.Can (this)) {
					d.act = act;
					d.acstep = -1;
				}
			}
			if (d.act == null)
				break;
			d.acstep++;
			EntityAct.Step step = d.act.GetStep(d.acstep);
			if (step == null) {
				d.act = null;
			} else {
				d.actime = time + step.Time(this);
				step.Do(this);
			}
		}
	}

	public void BeAttack () {
		if (d.stat == null)
			return;
		d.stat.hp --;
	}
}
	