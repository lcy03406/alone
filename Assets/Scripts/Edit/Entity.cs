//utf-8。
using System;
using System.Collections.Generic;


namespace Edit {
	//TODO
	public sealed partial class Entity : Data {
		public readonly Text name;
		public readonly Stage start_stage;
		public readonly Play.Attrs.Stat stat;
		public readonly Play.Attrs.Part part;
		public readonly Play.AttrCreate attr;
	}
	public sealed class Stage : Data {
		public Tile tile;
		public Animat idle_ani;
		public List<int> src_spell_cat;
		public List<Spell> dst_spell;
		public Spell dst_auto_spell;

		public readonly string name;
		public readonly int render_layer;
		public readonly bool blockade;
		public readonly Play.Effect start_ef;
		public readonly Play.Effect tick_ef;
		public readonly Play.Effect finish_ef;
	}
}

