using System;
using System.Collections.Generic;

namespace Schema {
	public sealed class Floor : SchemaBase<FloorID, Floor> {
		public readonly Sprite.A sprite;
        private Floor (Schema.SpriteID spid) {
			this.sprite = Sprite.GetA (spid);
		}
		public static void AddAll(List<EditFloor> edits) {
			foreach (EditFloor edit in edits) {
				Add(edit.id, new Floor(edit.sprite));
			}
		}
	}
}

