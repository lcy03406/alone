//utf-8。
using System;
using System.IO;
using System.Runtime.Serialization.Formatters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using UnityEngine.Assertions;

namespace Play {
	class Resolver : DefaultContractResolver {
		public Resolver () {
			IgnoreSerializableAttribute = false;
			IgnoreSerializableInterface = false;
			//SerializeCompilerGeneratedMembers = true;
		}
		protected override JsonContract CreateContract (Type objectType) {
			//Assert.IsTrue (objectType.IsSealed, string.Format ("Not Sealed! {0}", objectType));
			Assert.IsTrue (objectType.IsSerializable, string.Format ("Not Serializable! {0}", objectType));
			JsonContract c = base.CreateContract (objectType);
			Assert.IsNotNull (c, string.Format ("No Contract! {0}", objectType));
			return c;
		}
	}


	public class WorldFile {
		string pathBase;
		string pathBackup;
		string pathCurrent;
		string pathData;
		JsonSerializer ser;

		public WorldFile () {
			ser = new JsonSerializer ();
			ser.Converters.Add (new StringEnumConverter ());
			ser.ContractResolver = new Resolver ();
			ser.TypeNameHandling = TypeNameHandling.Auto;
			ser.TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple;
			ser.Formatting = Formatting.Indented;
			ser.Error += Ser_Error;
		}

		private void Ser_Error (object sender, Newtonsoft.Json.Serialization.ErrorEventArgs e) {
			Assert.IsTrue(false, string.Format("JSON ERROR {0} : {1}", e.CurrentObject.ToString(), e.ErrorContext.Error.Message));
			throw e.ErrorContext.Error;
		}

		protected void SaveSome<T>(string name, T value) {
			string filename = pathCurrent + name + ".json";
			using (StreamWriter sw = new StreamWriter (filename))
			using (JsonWriter w = new JsonTextWriter (sw)/* { QuoteName = false }*/) {
				ser.Serialize (w, value);
			}
		}

		protected T LoadSome<T>(string name) {
			string filename = pathCurrent + name + ".json";
			if (!File.Exists (filename)) {
				filename = pathData + name + ".json";
				if (!File.Exists (filename))
					return default (T);
			}
			using (StreamReader sr = new StreamReader (filename))
			using (JsonReader r = new JsonTextReader (sr)) {
				T e = ser.Deserialize<T> (r);
				return e;
			}
		}

		public void LoadWorld (string path, string name) {
			pathBase = path + name + "/";
			pathBackup = pathBase + ".backup/";
			pathCurrent = pathBase + ".current/";
			pathData = pathBase + "data/";
			Directory.CreateDirectory (pathBase);
			Directory.CreateDirectory (pathBackup);
			Directory.CreateDirectory (pathCurrent);
			Directory.CreateDirectory (pathData);
		}


		public void SaveWorld () {
			Directory.CreateDirectory (pathData);
			if (Directory.Exists (pathBackup)) {
				Directory.Delete (pathBackup, true);
			}
			Directory.CreateDirectory (pathBackup);
			string[] names = Directory.GetFiles (pathCurrent);
			foreach (string fullname in names) {
				string filename = Path.GetFileName (fullname);
				string namefile = pathData + filename;
				if (File.Exists (namefile)) {
					File.Move (namefile, pathBackup + filename);
				}
				File.Move (pathCurrent + filename, namefile);
			}
		}

		public Grid.Data LoadGrid(int z, Coord g) {
			string name = "layer_" + z + "_grid_" + g;
			return LoadSome<Grid.Data> (name);
		}

		public void SaveGrid (int z, Coord g, Grid grid) {
			string name = "layer_" + z + "_grid_" + g;
			SaveSome (name, grid.Save ());
		}

		public Entity LoadPlayer () {
			return LoadSome<Entity> ("player");
		}

		public void SavePlayer (Entity player) {
			SaveSome ("player", player);
		}

		public World.Param LoadParam () {
			return LoadSome<World.Param> ("param");
		}

		public void SaveParam (World.Param param) {
			SaveSome ("param", param);
		}

		public Layer.Param LoadLayerParam(int z) {
			string name = "layer_" + z + "_param";
			return LoadSome<Layer.Param>(name);
		}

		public void SaveLayerParam(int z, Layer.Param param) {
			string name = "layer_" + z + "_param";
			SaveSome(name, param);
		}
	}
}
