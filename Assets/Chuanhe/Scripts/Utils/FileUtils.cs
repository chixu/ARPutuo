using System;
using System.IO;
using UnityEngine;

public class FileUtils
{
	public static string GetPersistentPath(string str){
		return Path.Combine (Application.persistentDataPath, str);
	}

	public static void CopyDirectory (string sourceDirName, string destDirName, string[] ignores = null)
	{
		DirectoryInfo dir = new DirectoryInfo (sourceDirName);
		//DirectoryInfo[] dirs = dir.GetDirectories ();
		// If the source directory does not exist, throw an exception.

		// If the destination directory does not exist, create it.
		if (!Directory.Exists (destDirName)) {
			Directory.CreateDirectory (destDirName);
		}

		// Get the file contents of the directory to copy.
		FileInfo[] files = dir.GetFiles ();
		foreach (FileInfo file in files) {
			// Create the path to the new copy of the file.
			bool ignore = false;
			if (ignores != null) {
				for (int i = 0; i < ignores.Length; i++) {
					if(file.Name.EndsWith(ignores[i])){
						ignore = true;
						break;
					}
				}
			}
			if (!ignore) {
				string temppath = Path.Combine (destDirName, file.Name);
				// Copy the file.
				file.CopyTo (temppath, false);
			}
		}

		// If copySubDirs is true, copy the subdirectories.
		//if (copySubDirs) {

//			foreach (DirectoryInfo subdir in dirs) {
//				// Create the subdirectory.
//				string temppath = Path.Combine (destDirName, subdir.Name);
//
//				// Copy the subdirectories.
//				CopyDirectory (subdir.FullName, temppath);
//			}
		//}
	}
}
