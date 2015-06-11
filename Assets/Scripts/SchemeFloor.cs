using System;
using UnityEngine;

public partial class Scheme {
	
	public class Floor {
		public enum ID {
			None,
			Grass,
			Dirt,
			Ocean,
			
			Size,
		}
		public Sprite sprite;
	}

	private Floor[] floors = new Floor[(int)Floor.ID.Size];
	
	public Floor GetFloor (Floor.ID id) {
		return floors [(int)id];
	}

	public Floor SetFloor (Floor.ID id, SchemeSpriteID spid) {
		Floor floor = new Floor ();
		floor.sprite =  GetSprite (spid);
		floors[(int)id] = floor;
		return floor;
	}

	public void LoadFloors () {
		SetFloor(Floor.ID.Grass, SchemeSpriteID.a_grass);
		SetFloor(Floor.ID.Dirt, SchemeSpriteID.a_dirt);
		SetFloor(Floor.ID.Ocean, SchemeSpriteID.a_ocean);
	}
}