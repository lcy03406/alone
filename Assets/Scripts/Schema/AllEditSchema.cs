using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using UnityEngine;

namespace Schema {
	[Serializable]
	public class EditBiomeCave {
		public string name;
		public BiomeID id;
		public FloorID floor;
		public EntityID entr;
		public EntityID exit;
		public EntityID block;
		public EntityID border;
	}

	[Serializable]
	public class EditEntityBoulder {
		public string name;
		public EntityID id;
		public SpriteID sprite;
		public ItemID item;
		public int hp;
		public int defence;
	}

	[Serializable]
	public class EditEntityCreature {
		public string name;
		public EntityID id;
		public SpriteID sprite;
		public int hp;
		public int stamina;
		public int attack;
		public int defence;
	}

	[Serializable]
	public class EditEntityTree {
		public string name;
		public EntityID id;
		public SpriteID sprite;
		public ItemID fruit;
		public int fruitCount;
	}

	[Serializable]
	public class EditEntityWorkshop {
		public string name;
		public EntityID id;
		public SpriteID sprite;
		public List<ActionID> actions;
	}

	[Serializable]
	public class EditFloor {
		public string name;
		public FloorID id;
		public SpriteID sprite;
	}

	[Serializable]
	public class EditIactAttack {
		public string name;
		public ActionID id;
		public int stamina;
		public int time1;
		public int time2;
	}

	[Serializable]
	public class EditIactRest {
		public string name;
		public ActionID id;
		public int stamina;
		public int time;
	}

	[Serializable]
	public class EditIactTravel {
		public string name;
		public ActionID id;
		public int stamina;
		public int time;
		public int to;
	}


	[Serializable]
	public class EditItem {
		public string name;
		public ItemID id;
		public SpriteID sprite;
		public string desc;
	}

	[Serializable]
	public class EditAll {
		public List<EditBiomeCave> biome;
		public List<EditEntityBoulder> boulder;
		public List<EditEntityCreature> creature;
		public List<EditEntityTree> trees;
		public List<EditEntityWorkshop> workshop;
		public List<EditFloor> floor;
		public List<EditIactAttack> attack;
		public List<EditIactRest> rest;
		public List<EditIactTravel> travel;
		public List<EditItem> item;

		public static JsonSerializer Ser() {
			JsonSerializer ser = new JsonSerializer();
			ser.Converters.Add(new StringEnumConverter());
			ser.TypeNameHandling = TypeNameHandling.Auto;
			ser.TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple;
			ser.Formatting = Formatting.Indented;
			return ser;
		}
	}
}
