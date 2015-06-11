using System;
using UnityEngine;

public partial class Scheme {

	public enum StatMeter {
		HitPoint, //
		Stamina,  //for hard work and battle
		Vitality, //for mental work

		Food,
		Water,
		Sleep,
		Fun,

		Size
	}
	
	public enum StatPoint {
		Strength, //Attack
		Constitution, //or Toughness HitPoint
		Agility, //Defence
		Dexterity, //crafting speed/quality
		Endurance, //StaminaPoint
		//Willpower, //reduce pain
		Focus, //Accuracy
		Intelligence, //skill learning rate
		Creativity, //crafting quality
		Charisma, //magic
		//Wisdom, Perception

		Size
	}
	
	public class Gait {
		[Flags]
		public enum ID {
			None   = 0,
			Crawl  = 1<<0,
			Walk   = 1<<1,
			Run    = 1<<2,
			Sprint = 1<<3,
			Swim   = 1<<4,
			Fly    = 1<<5,
		}
		int timePrepare;
		int timeRecover;
		int stamina;
	}

	public class Attack {
		int timePrepare;
		int timeRecover;
		int stamina;
		int tohit;
		int damage;
	}

	public class Creature {
		public enum ID {
			Human,

			Size,
		}
		public Sprite sprite;
	}
	
	private Creature[] creatures = new Creature[(int)Creature.ID.Size];
	
	public Creature GetCreature (Creature.ID id) {
		return creatures [(int)id];
	}
	
	public Creature SetCreature (Creature.ID id, SchemeSpriteID spid) {
		Creature creature = new Creature ();
		creature.sprite =  GetSprite (spid);
		creatures[(int)id] = creature;
		return creature;
	}
	
	public void LoadCreatures () {
		SetCreature(Creature.ID.Human, SchemeSpriteID.a_grass);
	}
}
