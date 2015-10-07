using System;

public class WorldEntity {

	[Serializable]
	public class Data {
		public WUID id;
		public Coord c;
		public Direction dir;
		public int acstep;
		public int actime;
		public PlayStat stat;
		public PlayInventory inv;
		public PlayAct act;
		public PlayAI ai;
	}

	public static Data Create(World world, Scheme.Creature.ID race) {
		Data d = new Data ();
		d.id = world.NextWUID ();
		d.stat = new PlayStat ();
		d.stat.hp = 3;
		d.inv = new PlayInventory ();
		d.act = null;
		d.ai = new PlayAIHuman ();
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
		isPlayer = true;
	}

	public WorldEntity (World world_, WorldGrid g_, Data d_) : this(world_, d_) {
		//this.grid = g;
		isPlayer = false;
		d.ai.ent = this;
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
				PlayAct act = d.ai.NextAct ();
				if (act.Can (this)) {
					d.act = act;
					d.acstep = -1;
				}
			}
			if (d.act == null)
				break;
			d.acstep++;
			PlayAct.Step step = d.act.GetStep(d.acstep);
			if (step == null) {
				d.act = null;
			} else {
				d.actime = time + step.Time(this);
				step.Do(this);
			}
		}
	}
	
	public bool TryAct(PlayAct act) {
		if (d.act != null)
			return false;
		if (!act.Can(this))
			return false;
		d.act = act;
		d.acstep = -1;
		return true;
	}

	private bool Can () {
		//TODO
		return d.act == null;
	}

	public bool CmdMove (Direction to) {
		if (to == Direction.None || to == Direction.Center) {
			return false;
		}
		if (!Can ())
			return false;
		PlayAct act;
		if (to == d.dir) {
			act = new PlayActMove (to);
		} else {
			act = new PlayActDir (to);
		}
		if (!TryAct (act))
			return false;
		//TODO
		UpdateAct (world.param.time);
		return true;
	}

	public bool CmdAttack () {
		if (d.dir == Direction.None || d.dir == Direction.Center) {
			return false;
		}
		if (!Can ())
			return false;
		PlayAct act = new PlayActAttack ();
		if (!TryAct (act))
			return false;
		//TODO
		UpdateAct (world.param.time);
		return true;
	}

	public void BeAttack () {
		if (d.stat == null)
			return;
		d.stat.hp --;
	}
}
	