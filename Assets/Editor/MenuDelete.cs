using System;
using System.IO;
using UnityEngine;
using UnityEditor;

using Play;

public class MenuDelete {
	[MenuItem("Revenge/Delete Save", priority = 102)]
	private static void DeleteSave() {
		ClearFolder(Application.persistentDataPath + "/save/");
	}

	private static void ClearFolder(string FolderName) {
		DirectoryInfo dir = new DirectoryInfo(FolderName);

		foreach (FileInfo fi in dir.GetFiles()) {
			fi.Delete();
		}

		foreach (DirectoryInfo di in dir.GetDirectories()) {
			ClearFolder(di.FullName);
			di.Delete();
		}
	}
}

