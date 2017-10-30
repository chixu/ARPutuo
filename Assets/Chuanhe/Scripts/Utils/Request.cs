using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

public class Request
{
	public static WWW current;
	public static string RemoteUrl = "http://192.168.0.105:8080/web";
	//public static Action<float> downloadProgressHandler;

	//	public static IEnumerator ReadRemote (string str, Action<string> handler)
	//	{
	//		string url = RemoteUrl+"/" +str;
	//		Logger.Log ("loading " + url);
	//		UnityWebRequest www = UnityWebRequest.Get (Utils.ApplyRandomVersion (url));
	//		yield return www.Send ();
	//		if (www.isError) {
	//			Logger.Log ("unabled to load " + url);
	//		} else {
	//			Logger.Log ("Loaded successfully");
	//			handler.Invoke (www.downloadHandler.text);
	//		}
	//	}


	public static IEnumerator ReadRemote (string str, Action<string> handler, Action failedHandler = null)
	{
		string url = RemoteUrl + "/" + str;
		Logger.Log ("loading " + url);
		WWW www = new WWW (Utils.ApplyRandomVersion (url));
		current = www;
		yield return www;
		if (!String.IsNullOrEmpty (www.error)) {
			Logger.Log ("unabled to load " + url);
			if (failedHandler != null)
				failedHandler.Invoke ();
		} else {
			Logger.Log ("Loaded successfully");
			handler.Invoke (www.text);
		}
	}


	public static IEnumerator ReadStreaming (string str, Action<string> handler)
	{
		yield return Read (GetStreamingPath(str), handler);
	}

	public static IEnumerator ReadPersistent (string str, Action<string> handler)
	{
		yield return Read (GetPersistentPath(str), handler);
	}

	public static string ResolvePath(string path, bool addFilePrefix = true){
		if (!addFilePrefix)
			return path;
		string str = "file:///" + path;
		str = str.Replace ("file:////", "file:///");
		return str;
	}

	public static string GetStreamingPath(string str){
		if (string.IsNullOrEmpty(str))
			return "";
		return ResolvePath(Application.streamingAssetsPath + "/" + str);
	}

	public static string GetPersistentPath(string str){
		if (string.IsNullOrEmpty(str))
			return "";
		return ResolvePath(Application.persistentDataPath + "/" + str);
	}

	public static IEnumerator Read (string str, Action<string> handler)
	{
		Logger.Log ("loading " + str);
		WWW www = new WWW (str);
		current = www;
		yield return www;
		if (!String.IsNullOrEmpty (www.error)) {
			Logger.Log ("unabled to load " + str);
		} else {
			Logger.Log ("Loaded successfully");
			handler.Invoke (www.text);
		}
	}

	public static IEnumerator DownloadFile (string src, string dest, bool absolute = false, Action<float> progressHandler = null)
	{
		//
		src = absolute ? src : Utils.ApplyRandomVersion (RemoteUrl + "/" + src);
		Logger.Log ("Downloading " + src + " to " + dest);
		WWW www = new WWW (src);
		current = www;
		while (current!=null && current.progress < 1) {
			if (progressHandler != null)
				progressHandler.Invoke (www.progress);
			yield return null;
		}
		yield return current;
		dest = absolute ? dest : Path.Combine (Application.persistentDataPath, dest);
		if (!Directory.Exists (Path.GetDirectoryName (dest)))
			Directory.CreateDirectory (Path.GetDirectoryName (dest));
		try{
			File.WriteAllBytes (dest, www.bytes);
		}catch{

		}
		Logger.Log ("Downloaded " + src + " to " + dest);
	}

	public static void Cancel(){
		
		if (current != null) {
			//Logger.Log (current.isDone.ToString (), "blue");
			current.Dispose ();
			current = null;
			//Logger.Log (current.isDone.ToString (), "blue");
		}
	}
}
