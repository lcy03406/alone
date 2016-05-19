//utf-8。
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Play.Acts {
	[Serializable]
	internal class ActSpell : Act {
		internal Edit.Spell spell;
		public WUID dst;
		public Coord dstc;
		public int istep = -1;
		public ActSpell (Edit.Spell spell, WUID dst) {
			this.spell = spell;
			this.dst = dst;
		}
		public ActSpell(Edit.Spell spell, Coord dstc) {
			this.spell = spell;
			this.dstc = dstc;
		}
		public override string GetName() {
			return spell.name.text;
		}
		private Ctx GetCtx(Entity ent) {
			Ctx ctx = null;
			Entity ent_dst = ent.layer.FindEntity(dst);
			if (ent_dst == null) {
				ctx = new Ctx(ent.layer, ent, null, dstc);
			} else {
				ctx = new Ctx(ent.layer, ent, ent_dst);
			}
			return ctx;
		}
        public override bool Can (Entity ent) {
			Ctx ctx = GetCtx(ent);
			if (spell.phase.Count == 0)
				return true;
			Edit.Phase phase0 = spell.phase[0];
			if (phase0 == null)
				return true;
			bool can = phase0.effect.func.Can(ctx);
            return can;
		}
		public override int NextStep(Entity ent, List<string> logs) {
			istep++;
			if (istep >= spell.phase.Count) {
				return -1;
			}
			Ctx ctx = GetCtx(ent);
			Edit.Phase phase = spell.phase[istep];
			List<Entity> ents = DoShape(ctx, phase.shape);
			DoPhase(phase, ents, logs);
			return phase.time;
		}
		private List<Entity> DoShape(Ctx ctx, Edit.Shape shape) {
			//TODO
			return null;
		}

		private void DoPhase(Edit.Phase phase, List<Entity> ents, List<string> logs) {
			//TODO
		}
	}
}
