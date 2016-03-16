using UnityEngine;
using UnityEditor;

using Play;

public class MenuGrid {
	[MenuItem("Revenge/Reset Grid", priority = 101)]
	private static void CreateGrid() {
		string prefabPath = "Assets/Prefabs/Grid.prefab";
		GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
		Mesh mesh = GenerateMesh();
		AssetDatabase.AddObjectToAsset(mesh, prefabPath);
		prefab.GetComponent<MeshFilter>().sharedMesh = mesh;
		Texture texture = Resources.Load<Texture>("Sprites/tileset");
		prefab.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = texture;
		AssetDatabase.SaveAssets();
	}

	private static Mesh GenerateMesh() {
		int xSize = World.GRID_SIZE;
		int ySize = World.GRID_SIZE;

		Mesh mesh = new Mesh();
		mesh.name = "MeshGrid_" + new Coord(xSize, ySize);

		Vector3[] vertices = new Vector3[xSize * ySize * 4];
		int[] triangles = new int[xSize * ySize * 6];
		int v = 0, t = 0;
		for (int y = 0; y < ySize; y++) {
			for (int x = 0; x < xSize; x++) {
				triangles[t++] = v;
				triangles[t++] = v + 1;
				triangles[t++] = v + 2;
				triangles[t++] = v + 2;
				triangles[t++] = v + 1;
				triangles[t++] = v + 3;
				vertices[v++] = new Vector3(x, y);
				vertices[v++] = new Vector3(x, y + 1);
				vertices[v++] = new Vector3(x + 1, y);
				vertices[v++] = new Vector3(x + 1, y + 1);
			}
		}
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.uv = GenerateUV();
		return mesh;
	}

	public static Vector2[] GenerateUV() {
		int xSize = World.GRID_SIZE;
		int ySize = World.GRID_SIZE;
		int v = 0;
		Vector2[] one = new Vector2[] {
			new Vector2(69/(float)512, 479/(float)512),
			new Vector2(69/(float)512, (479+32)/(float)512),
			new Vector2((69+32)/(float)512, 479/(float)512),
			new Vector2((69+32)/(float)512, (479+32)/(float)512),
		};
		Vector2[] uv = new Vector2[xSize * ySize * 4];
		for (int y = 0; y < ySize; y++) {
			for (int x = 0; x < xSize; x++) {
				uv[v++] = one[0];
				uv[v++] = one[1];
				uv[v++] = one[2];
				uv[v++] = one[3];
			}
		}
		return uv;
	}
}

