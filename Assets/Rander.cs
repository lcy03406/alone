using UnityEngine;
using System.Collections.Generic;

public class Rander : MonoBehaviour {

	private const int resolution = 256;
	// Use this for initialization
	void Start() {
		Texture2D texture = NoiseTex.FillTexture(resolution);
		//Texture2D texture = BoxTex.FillTexture(resolution);
		texture.name = "Procedural Texture";
		Sprite sp = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
		GetComponent<SpriteRenderer>().sprite = sp;
	}
	// Update is called once per frame
	void Update() {

	}
}

public static class NoiseTex {
	public static Texture2D FillTexture(int resolution) {
		Texture2D texture = new Texture2D(resolution, resolution, TextureFormat.RGB24, true);
		for (int y = 0; y < resolution; y++) {
			for (int x = 0; x < resolution; x++) {
				int f = FillPixel(x, y);
				texture.SetPixel(x, y, new Color(f, f, f));
			}
		}
		texture.Apply();
		return texture;
	}
	private static int FillPixel(int x, int y) {
		int i1 = SimpleNoise(x / 20, y / 20);
		int i2 = SimpleNoise(x / 16, y / 16);
		int ix1 = SimpleNoise(x / 13, y / 2);
		int ix2 = SimpleNoise(x / 33, y / 2);
		int iy1 = SimpleNoise(x / 2, y / 13);
		int iy2 = SimpleNoise(x / 2, y / 33);
		int ixy1 = SimpleNoise(x / 2, y / 2);
		//return (i1 + i2) >= 320 || (ix + iy) >= 256 ? 1 : 0;
		int i = (i1 + i2) / 2;
		int ix = (ix1 + ix2) / 2;
		int iy = (iy1 + iy2) / 2;
		return i >=  200 || (ix >= 170 || iy >= 170) && ixy1 > 32 ? 1 : 0;
	}

	private static int[] hash = {
		151,160,137, 91, 90, 15,131, 13,201, 95, 96, 53,194,233,  7,225,
		140, 36,103, 30, 69,142,  8, 99, 37,240, 21, 10, 23,190,  6,148,
		247,120,234, 75,  0, 26,197, 62, 94,252,219,203,117, 35, 11, 32,
		 57,177, 33, 88,237,149, 56, 87,174, 20,125,136,171,168, 68,175,
		 74,165, 71,134,139, 48, 27,166, 77,146,158,231, 83,111,229,122,
		 60,211,133,230,220,105, 92, 41, 55, 46,245, 40,244,102,143, 54,
		 65, 25, 63,161,  1,216, 80, 73,209, 76,132,187,208, 89, 18,169,
		200,196,135,130,116,188,159, 86,164,100,109,198,173,186,  3, 64,
		 52,217,226,250,124,123,  5,202, 38,147,118,126,255, 82, 85,212,
		207,206, 59,227, 47, 16, 58, 17,182,189, 28, 42,223,183,170,213,
		119,248,152,  2, 44,154,163, 70,221,153,101,155,167, 43,172,  9,
		129, 22, 39,253, 19, 98,108,110, 79,113,224,232,178,185,112,104,
		218,246, 97,228,251, 34,242,193,238,210,144, 12,191,179,162,241,
		 81, 51,145,235,249, 14,239,107, 49,192,214, 31,181,199,106,157,
		184, 84,204,176,115,121, 50, 45,127,  4,150,254,138,236,205, 93,
		222,114, 67, 29, 24, 72,243,141,128,195, 78, 66,215, 61,156,180
	};

	private const int hashMask = 255;

	public static int SimpleNoise(int x, int y) {
		//float f = Mathf.PerlinNoise((float)x / resolution * 16, (float)y / resolution * 16);
		int ix = x % hashMask;
		int iy = y % hashMask;
		int ixy = (hash[ix] + hash[iy]) % hashMask;
		return hash[ixy];
	}
}

public static class BoxTex {
	public static Texture2D FillTexture(int resolution) {
		Texture2D texture = new Texture2D(resolution, resolution, TextureFormat.RGB24, true);
		Dictionary<Vector2, Rect> rooms = new Dictionary<Vector2, Rect>();
		for (int y = 0; y < resolution; y++) {
			for (int x = 0; x < resolution; x++) {
				if (NoiseTex.SimpleNoise(x, y) >= 255) {
					rooms.Add(new Vector2(x, y), new Rect(x, y, 0, 0));
					texture.SetPixel(x, y, new Color(0, 255, 0));
				}
			}
		}
		texture.Apply();
		return texture;
	}
}
