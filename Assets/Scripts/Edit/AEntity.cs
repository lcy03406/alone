//utf-8。
using System;
using System.Collections.Generic;


namespace Edit {
	//TODO
	public sealed partial class AEntity : Meta {
		public Text name;
		public AStage start_stage;
		public Play.Attrs.Stat stat;
		public Play.Attrs.Part part;
		public Play.AttrCreate attr;
		public SortedList<AStage, AEntityStage> stages;
	}
	public sealed class AEntityStage : Meta {
		public ATile tile;
		public AAct dst_auto_act;
		public AMenu src_act;
		public AMenu dst_act;
	}

	public sealed class AStage : Meta {
		public Text name;
		public Animat idle_ani;
		public int render_layer;
		public bool blockade;
		public AEffect start_ef;
		public AEffect tick_ef;
		public AEffect finish_ef;
	}

	public sealed class AStat : Meta {
		public Text name;
		public bool hidden;
		public AStat max;
	}


	public enum Perform {
		Rest,
		Move,
		UseItem,
		Make,
		Build,
		SetTrap,

		Mine,
		PickItem,
		PickBranch,
		FellTree,
		Skin,
		Butcher,
		Attack,
		Talk,
		Give,
		Take,
		Ride,
		Repair,
	}
}

