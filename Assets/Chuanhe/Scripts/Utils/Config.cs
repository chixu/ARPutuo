//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using System.Xml.Linq;
//using UnityEngine;
//using System;
//
////public class ConfigStatus{
////	public const string updated = "updated";
////	public const string noupdate = "noupdate";
////	public const string failed = "failed";
////}
//
//public class Config
//{
//
//	private static XElement localConfig;
//	private static XElement remoteConfig;
//	public static bool forceBreak = false;
//	private static Action<int, int> fileLoadedHandler;
//	private static XElement tempConfig;
//	public static float fileSize;
//	//public static string status;
//
//	public static string GetPlatformName () {
//		if (Application.platform == RuntimePlatform.Android)
//			return "Android";
//		else if (Application.platform == RuntimePlatform.IPhonePlayer)
//			return "IOS";
//		else
//			return "Windows";
//	}
//
//	public static IEnumerator LoadConfig(string url, Action<int, int> loadedHandler = null, OKCancelPanel panel = null){
//		localConfig = null;
//		remoteConfig = null;
//		forceBreak = false;
//		fileLoadedHandler = loadedHandler;
//		fileSize = 0f;
//		Logger.Log (url);
//		yield return Request.ReadPersistent (url, str => Config.localConfig = XDocument.Parse(str).Root);
//		yield return Request.ReadRemote (url, str => Config.remoteConfig = XDocument.Parse(str).Root);
//		string platform = GetPlatformName ();
//
//		List<string> filenames = new List<string> ();
//		if (remoteConfig == null) {
//			Debug.Log ("remoteConfig = null");
//		} else if (localConfig == null ) {
//			Debug.Log ("remoteConfig != null");
//			fileSize = Xml.Float(remoteConfig.Element ("all"), "size");
//			var nodes = remoteConfig.Element ("all").Elements();
//			foreach (XElement node in nodes) {
//				filenames.Add (node.Value);
//			}
//		} else{
//			string localVersion = Xml.Version (localConfig);
//			string preVersion = Xml.Attribute (remoteConfig, "preversion");
//			string remoteVersion = Xml.Version (remoteConfig);
//			//float filesize = Xml.Float(remoteConfig, "size");
//			//if (localVersion != remoteVersion) {
//			if (true) {
//				fileSize = Xml.Float(remoteConfig.Element ("update"), "size");
//				var nodes = remoteConfig.Element ("update").Elements();
//				//List<string> updates = new List<string> ();
//				foreach (XElement node in nodes) {
//					filenames.Add (node.Value);
//				}
//				int idx = url.IndexOf ("/");
//				string path = idx == -1 ? url : url.Substring (0, idx);
//				while (localVersion != preVersion) {
//					Config.tempConfig = null;
//					yield return Request.ReadRemote (path+"/version/" + preVersion +".xml", str => Config.tempConfig = XDocument.Parse(str).Root);
//					if (tempConfig == null) {
//						var all = remoteConfig.Element ("all").Elements ();
//						filenames = new List<string> ();
//						foreach (XElement node in all) {
//							filenames.Add (node.Value);
//						}
//						break;
//					} else {
//						preVersion = Xml.Attribute (tempConfig, "preversion");
//						var updateNotes = tempConfig.Element ("update").Elements();
//						foreach (XElement node in updateNotes) {
//							Logger.Log (node.Value,"blue");
//							if(!filenames.Contains(node.Value))
//								filenames.Add (node.Value);
//						}
//					}
//				}
//			}
//		}
//		if (panel != null && Application.internetReachability != NetworkReachability.ReachableViaLocalAreaNetwork && fileSize > 3) {
//			//if (panel != null && filesize > 3) {
//			yield return panel.Show (string.Format( I18n.Translate ("not_in_wifi"), fileSize.ToString()+"M"));
//			if (panel.isCancel)
//				yield break;
//		}
//		yield return LoadFiles (filenames, url);
//	}
//
//	private static IEnumerator LoadFiles(List<string> names, string configurl){
//		if (names.Count == 0)
//			yield break;
//		for (int i = 0; i < names.Count; i++) {
//			Logger.Log (names [i], "green");
//		}
//		//var nodes = remoteConfig.Elements ();
//		string platform = GetPlatformName ();
//		int count = names.Count;
//		if (fileLoadedHandler != null && count>0) {
//			fileLoadedHandler.Invoke (0, count);
//		}
//		for (int i=0; i<count ; i++) {
//			string name = names [i];
//			yield return Request.DownloadFile (name.Replace ("{%platform%}", platform), name.Replace ("{%platform%}/", ""), false, x=>Logger.Log(x.ToString(), "yellow"));
//			if (forceBreak) {
//				yield break;
//			} else {
//				if (fileLoadedHandler != null) {
//					fileLoadedHandler.Invoke (i+1, count);
//				}
//			}
//		}
//		//Debug.Log ("remoteConfig: " + remoteConfig.ToString ());
//		File.WriteAllText (Path.Combine(Application.persistentDataPath,  configurl), remoteConfig.ToString());
//	}
//}
