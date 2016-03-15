using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using UnityEngine.Assertions;

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
	public class EditIactMove {
		public string name;
		public ActionID id;
		public int stamina;
		public int time1;
		public int time2;
	}

	[Serializable]
	public class EditIactAttack {
		public string name;
		public ActionID id;
		public int stamina;
		public int time1;
		public int time2;
		public int mulDamage;
		public int addDamage;
	}

	[Serializable]
	public class SomeItem {
		public ItemID id;
		public int count;
	}

	[Serializable]
	public class EditIactBuild {
		public string name;
		public ActionID id;
		public int stamina;
		public int time;
		public List<SomeItem> tools;
		public List<SomeItem> reagents;
		public EntityID build;
	}

	[Serializable]
	public class EditIactMake {
		public string name;
		public ActionID id;
		public int stamina;
		public int time;
		public List<SomeItem> tools;
		public List<SomeItem> reagents;
		public List<SomeItem> products;
	}

	[Serializable]
	public class EditIactPick {
		public string name;
		public ActionID id;
		public int stamina;
		public int time;
		public List<SomeItem> tools;
		public List<SomeItem> reagents;
		public List<SomeItem> byproducts;
		public PartID part;
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
		public List<EditBiomeCave> biome = new List<EditBiomeCave>();
		public List<EditEntityBoulder> boulder = new List<EditEntityBoulder>();
		public List<EditEntityCreature> creature = new List<EditEntityCreature>();
		public List<EditEntityTree> trees = new List<EditEntityTree>();
		public List<EditEntityWorkshop> workshop = new List<EditEntityWorkshop>();
		public List<EditFloor> floor = new List<EditFloor>();
		public List<EditIactMove> move = new List<EditIactMove>();
		public List<EditIactAttack> attack = new List<EditIactAttack>();
		public List<EditIactBuild> build = new List<EditIactBuild>();
		public List<EditIactMake> make = new List<EditIactMake>();
		public List<EditIactPick> pick = new List<EditIactPick>();
		public List<EditIactRest> rest = new List<EditIactRest>();
		public List<EditIactTravel> travel = new List<EditIactTravel>();
		public List<EditItem> item = new List<EditItem>();

		public static JsonSerializer Ser() {
			JsonSerializer ser = new JsonSerializer();
			ser.Converters.Add(new StringEnumConverter());
			ser.MissingMemberHandling = MissingMemberHandling.Error;
			ser.TypeNameHandling = TypeNameHandling.Auto;
			ser.TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple;
			ser.Formatting = Formatting.Indented;
			ser.Error += Ser_Error;
			return ser;
		}

		private static void Ser_Error(object sender, Newtonsoft.Json.Serialization.ErrorEventArgs e) {
			Assert.IsTrue(false, string.Format("JSON ERROR {0} : {1}", e.CurrentObject.ToString(), e.ErrorContext.Error.Message));
			throw e.ErrorContext.Error;
		}
	}
}
