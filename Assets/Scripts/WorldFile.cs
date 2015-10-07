using System;
using System.IO;
using System.Collections.Generic;
using Formater = System.Runtime.Serialization.Formatters.Binary.BinaryFormatter;


public class WorldFile {
	string pathBase;
	string pathBackup;
	string pathCurrent;
	string pathData;

	public void LoadWorld (string path, string name) {
		pathBase = path + name + "/";
		pathBackup = pathBase + ".backup/";
		pathCurrent = pathBase + ".current/";
		pathData = pathBase + "data/";
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

	public WorldGrid.Data LoadGrid (Coord g) {
		string binname = string.Format ("/grid_" + g + ".bin");
		string filename = pathCurrent + binname;
		if (!File.Exists (filename)) {
			filename = pathData + binname;
			if (!File.Exists (filename)) {
				return null;
			}
		}
		FileStream fs = File.Open (filename, FileMode.Open);
		Formater f = new Formater();
		WorldGrid.Data data = f.Deserialize (fs) as WorldGrid.Data;
		fs.Close ();
		return data;
	}

	public void SaveGrid (Coord g, WorldGrid grid) {
		string binname = string.Format ("/grid_" + g + ".bin");
		Directory.CreateDirectory (pathCurrent);
		string filename = pathCurrent + binname;
		FileStream fs = File.Open (filename, FileMode.Create);
		Formater f = new Formater ();
		WorldGrid.Data data = grid.Save ();
		f.Serialize (fs, data);
		fs.Close ();
	}

	public WorldEntity.Data LoadPlayer () {
		string filename = pathData + string.Format ("/player.bin");
		if (!File.Exists (filename))
			return null;
		FileStream fs = File.Open (filename, FileMode.Open);
		Formater f = new Formater();
		WorldEntity.Data e = f.Deserialize (fs) as WorldEntity.Data;
		fs.Close ();
		return e;
	}
	
	public void SavePlayer (WorldEntity player) {
		Directory.CreateDirectory (pathCurrent);
		string filename = pathCurrent + string.Format ("/player.bin");
		FileStream fs = File.Open (filename, FileMode.Create);
		Formater f = new Formater ();
		WorldEntity.Data data = player.Save ();
		f.Serialize (fs, data);
		fs.Close ();
	}

	public World.Param LoadParam () {
		string filename = pathData + string.Format ("/param.bin");
		if (!File.Exists (filename))
			return null;
		FileStream fs = File.Open (filename, FileMode.Open);
		Formater f = new Formater();
		World.Param param = f.Deserialize (fs) as World.Param;
		fs.Close ();
		return param;
	}

	public void SaveParam (World.Param param) {
		Directory.CreateDirectory (pathCurrent);
		string filename = pathCurrent + string.Format ("/param.bin");
		FileStream fs = File.Open (filename, FileMode.Create);
		Formater f = new Formater ();
		f.Serialize (fs, param);
		fs.Close ();
	}
}
