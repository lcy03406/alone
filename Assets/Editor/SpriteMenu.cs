using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SpriteMenu {
	static string[] tilePaths = {
		"TexturePacker/Blocks/",
		"TexturePacker/Characters/",
	};
	const string tilesetPath = "/Resources/Sprites/tileset.png";
	[MenuItem("Revenge/Pack Sprites", priority = 100)]
	private static void PackSprites() {
		List<Texture2D> texs = new List<Texture2D>();
		List<string> names = new List<string>();
		foreach (string tilePath in tilePaths) {
			string[] filenames = Directory.GetFiles(tilePath, "*.png");
			foreach (string fullname in filenames) {
				string name = Path.GetFileNameWithoutExtension(fullname);
				names.Add(name);
				byte[] bytes = File.ReadAllBytes(fullname);
				Texture2D tex = new Texture2D(64, 64);
				tex.LoadImage(bytes);
				texs.Add(tex);
			}
		}
		Texture2D all = new Texture2D(1024, 1024);
		Rect[] rects = all.PackTextures(texs.ToArray(), 2);
		all.name = "tileset";
		all.Apply();
		File.WriteAllBytes(Application.dataPath + tilesetPath, all.EncodeToPNG());

		TextureImporter imp = AssetImporter.GetAtPath("Assets" + tilesetPath) as TextureImporter;
		imp.isReadable = true;
		List<SpriteMetaData> metas = new List<SpriteMetaData>();
		for (int i = 0; i < rects.Length; ++i) {
			SpriteMetaData meta = new SpriteMetaData();
			meta.name = names[i];
			meta.rect = rects[i];
			meta.rect.xMin *= all.width;
			meta.rect.xMax *= all.width;
			meta.rect.yMin *= all.height;
			meta.rect.yMax *= all.height;
			//meta.pivot = pivot;
			meta.alignment = 0;
			metas.Add(meta);
		}
		imp.spritesheet = metas.ToArray();
		imp.spriteImportMode = SpriteImportMode.Multiple;
		imp.spritePixelsPerUnit = rects[0].width * all.width;
		AssetDatabase.ImportAsset("Assets" + tilesetPath, ImportAssetOptions.ForceUpdate);

		EditEnum enun = new EditEnum();
		enun.name = "SpriteID";
		enun.values = names;
		EnumText text = new EnumText();
		text.all.Add(enun);
		string[] lines = text.SaveEnums();
		const string textPath = "/Scripts/Schema/SpriteID.cs";
		File.WriteAllLines(Application.dataPath + textPath, lines);

		AssetDatabase.Refresh();
	}
}

