//utf-8。
using System.Collections.Generic;

namespace Schema {
	public sealed class Entity : SchemaBase<Entity.ID, Entity> {
		public readonly Sprite.A sprite;
		public readonly string name;
		public readonly Iact.A[] makes;
		public readonly Iact.A[] iacts;

		Entity(SpriteID sprite,
			string name,
			Iact.A[] makes,
			Iact.A[] iacts)
		{
			this.sprite = Sprite.GetA(sprite);
			this.name = name;
			this.makes = makes;
			this.iacts = iacts;
        }

		public enum ID {
			Human,
			Tree_Pine,
			Workshop_Campfire,
		}
		static public void Init() {
			InitHuman();
			InitTree();
			InitWorkshop();
		}

		static Iact.A[] human_makes = {
			Iact.GetA(Iact.ID.Make_Cross),
			Iact.GetA(Iact.ID.Build_Campfire),
		};
		static Iact.A[] human_iacts = {
		};
		static void InitHuman() {
			Add(ID.Human, new Entity(
				sprite: SpriteID.c_human_young,
				name: "Human",
				makes: human_makes,
				iacts: human_iacts
			));
		}

		static Iact.A[] tree_makes = {
		};
		static Iact.A[] tree_iacts = {
			Iact.GetA (Iact.ID.Tree_PickBranch),
			Iact.GetA (Iact.ID.Tree_PickFruit),
		};
		static void InitTree() {
			Add(ID.Tree_Pine, new Entity(
				sprite: SpriteID.b_tree_pine,
				name: "Pine Tree",
				makes: tree_makes,
				iacts: tree_iacts
			));
		}

		static Iact.A[] workshop_makes = {
		};
		static Iact.A[] workshop_iacts = {
		};
		static void InitWorkshop() {
			Add(ID.Workshop_Campfire, new Entity(
				sprite: SpriteID.b_volcano,
				name: "Campfire",
				makes: workshop_makes,
				iacts: workshop_iacts
			));
		}
	}
}

