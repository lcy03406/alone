using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEditor;

public class MenuSprite {
	const int SPRITE_SIZE = 64;
	static Dictionary<string, string> tilePaths = new Dictionary<string, string> {
		{ "block_", "TexturePacker/Blocks/" },
		{ "char_", "TexturePacker/Characters/" },
	};
	const string tilesetPath = "/Resources/Sprites/tileset.png";
	[MenuItem("Revenge/Pack Sprites", priority = 100)]
	private static void PackSprites() {
		List<Texture2D> texs = new List<Texture2D>();
		List<string> names = new List<string>();
		foreach (KeyValuePair<string, string> pair in tilePaths) {
			string prefix = pair.Key;
			string path = pair.Value;
			string[] filenames = Directory.GetFiles(path, "*.png");
			foreach (string fullname in filenames) {
				string name = Path.GetFileNameWithoutExtension(fullname);
				if (name.Length == 0)
					continue;
				name = prefix + name;
				names.Add(name);
				byte[] bytes = File.ReadAllBytes(fullname);
				Texture2D tex = new Texture2D(SPRITE_SIZE, SPRITE_SIZE);
				tex.LoadImage(bytes);
				Assert.AreEqual(tex.width, SPRITE_SIZE, string.Format("tile {0} width is not {1}", fullname, SPRITE_SIZE));
				Assert.AreEqual(tex.height, SPRITE_SIZE, string.Format("tile {0} height is not {1}", fullname, SPRITE_SIZE));
				texs.Add(tex);
			}
		}
		int total = names.Count;
		const int span = SPRITE_SIZE + 3;
		int allsize = 1024;
		int line = allsize / span;
		while (total > line * line) {
			allsize *= 2;
			line = allsize / span;
		}
		Texture2D all = new Texture2D(allsize, allsize);
		List<SpriteMetaData> metas = new List<SpriteMetaData>();
		Color32 back = new Color32(255, 128, 128, 255);
		for (int i=0, y = 0; y < line && i < total; ++y) {
			for (int x = 0; x < line && i < total; ++x, ++i) {
				string name = names[i];
				Texture2D tex = texs[i];
				Color32[] pixels = tex.GetPixels32();
				int px = x * span + 1;
				int py = y * span + 1;
				all.SetPixels32(px, py, SPRITE_SIZE, SPRITE_SIZE, pixels);
				for (int bx = px; bx < px + SPRITE_SIZE; ++bx) {
					int by = py;
					all.SetPixel(bx, by - 1, all.GetPixel(bx, by));
					//all.SetPixel(bx, by - 2, back);
					by = py + SPRITE_SIZE - 1;
					all.SetPixel(bx, by + 1, all.GetPixel(bx, by));
					all.SetPixel(bx, by + 2, back);
				}
				for (int by = py - 1; by < py + SPRITE_SIZE + 1; ++by) {
					int bx = px;
					all.SetPixel(bx - 1, by, all.GetPixel(bx, by));
					//all.SetPixel(bx - 2, by, back);
					bx = px + SPRITE_SIZE - 1;
					all.SetPixel(bx + 1, by, all.GetPixel(bx, by));
					all.SetPixel(bx + 2, by, back);
				}
				SpriteMetaData meta = new SpriteMetaData();
				meta.name = name;
				meta.rect.xMin = px;
				meta.rect.yMin = py;
				meta.rect.xMax = px + SPRITE_SIZE;
				meta.rect.yMax = py + SPRITE_SIZE;
				meta.alignment = 0;
				metas.Add(meta);
			}
		}
		all.name = "tileset";
		all.Apply();
		File.WriteAllBytes(Application.dataPath + tilesetPath, all.EncodeToPNG());
		TextureImporter imp = AssetImporter.GetAtPath("Assets" + tilesetPath) as TextureImporter;
		imp.isReadable = true;
		imp.spritesheet = metas.ToArray();
		imp.spriteImportMode = SpriteImportMode.Multiple;
		imp.spritePixelsPerUnit = SPRITE_SIZE;
		AssetDatabase.ImportAsset("Assets" + tilesetPath, ImportAssetOptions.ForceUpdate);

		names.Sort();
		EditEnumText enun = new EditEnumText();
		enun.name = "SpriteID";
		enun.values = names;
		EditEnumFile text = new EditEnumFile();
		text.all.Add(enun);
		string[] lines = text.SaveEnums();
		const string textPath = "/Scripts/Schema/SpriteID.cs";
		File.WriteAllLines(Application.dataPath + textPath, lines);

		AssetDatabase.Refresh();
	}
}

public class ImportHuman {
	const int SPRITE_SIZE = 16;
	const string tilesetPath = "/Resources/Sprites/human.png";
	private static SpriteMetaData Def(int y, int x, string name, int addx) {
		int px = addx + x * (SPRITE_SIZE + 1);
		int py = (11-y) * (SPRITE_SIZE + 1);
		SpriteMetaData meta = new SpriteMetaData();
		meta.name = name;
		meta.rect.xMin = px;
		meta.rect.yMin = py;
		meta.rect.xMax = px + SPRITE_SIZE;
		meta.rect.yMax = py + SPRITE_SIZE;
		meta.alignment = 0;
		return meta;
	}
	private static SpriteMetaData Def0(int y, int x, string name) {
		return Def(y, x, name, 0);
	}
	private static SpriteMetaData Def1(int y, int x, string name) {
		return Def(y, x, name, 1);
	}
	private static void AddDef(List<SpriteMetaData> metas, string prefix, 
		int x0, int y0, int n1, int x1, int y1, int n2, int x2) {
		for (int i2 = 0, i = 0; i2 < n2; ++i2) {
			for (int i1 = 0; i1 < n1; ++i1) {
				int y = y0 + i1 / x1 + i2 / x2 * y1;
				int x = x0 + i1 % x1 + i2 % x2 * x1;
				metas.Add(Def1(y, x, prefix + i));
			}
		}
	}
	[MenuItem("Revenge/Import Human Sprites", priority = 200)]
	private static void PackSprites() {
		List<SpriteMetaData> metas = new List<SpriteMetaData>();
		//naked
		metas.Add(Def0(0, 0, "body_white_nomouth"));
		metas.Add(Def0(0, 1, "body_white_withmouth"));
		metas.Add(Def0(1, 0, "body_yellow_nomouth"));
		metas.Add(Def0(1, 1, "body_yellow_withmouth"));
		metas.Add(Def0(2, 0, "body_brown_nomouth"));
		metas.Add(Def0(2, 1, "body_brown_withmouth"));
		metas.Add(Def0(3, 0, "body_green_nomouth"));
		metas.Add(Def0(3, 1, "body_green_withmouth"));
		//pants
		AddDef(metas, "pants", 3, 0, 4, 1, 4, 2, 1);
		//shoes
		AddDef(metas, "pants", 4, 0, 4, 1, 4, 2, 1);
		//bigshoes
		AddDef(metas, "pants", 3, 4, 2, 2, 1, 2, 1);
		//shirt
		AddDef(metas, "shirt", 6, 0, 20, 4, 5, 6, 3);
		//hair
		AddDef(metas, "hair", 19, 0, 12, 4, 3, 5, 2);
		//beard
		AddDef(metas, "beard", 19, 3, 4, 4, 1, 4, 2);
		//hat
		AddDef(metas, "beard", 28, 0, 36, 4, 9, 1, 1);
		//shield
		AddDef(metas, "shield", 33, 0, 10, 4, 3, 6, 2);
		//melee
		AddDef(metas, "melee", 42, 0, 10, 5, 2, 10, 2);
		//bow
		AddDef(metas, "bow", 52, 0, 10, 2, 5, 1, 1);
		TextureImporter imp = AssetImporter.GetAtPath("Assets" + tilesetPath) as TextureImporter;
		imp.isReadable = true;
		imp.spritesheet = metas.ToArray();
		imp.spriteImportMode = SpriteImportMode.Multiple;
		imp.spritePixelsPerUnit = SPRITE_SIZE;
		AssetDatabase.ImportAsset("Assets" + tilesetPath, ImportAssetOptions.ForceUpdate);
/*
		names.Sort();
		EditEnumText enun = new EditEnumText();
		enun.name = "CharSpriteID";
		enun.values = names;
		EditEnumFile text = new EditEnumFile();
		text.all.Add(enun);
		string[] lines = text.SaveEnums();
		const string textPath = "/Scripts/Schema/CharSpriteID.cs";
		File.WriteAllLines(Application.dataPath + textPath, lines);
*/
		AssetDatabase.Refresh();
	}
}

