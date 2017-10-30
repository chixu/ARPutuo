using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using UnityEngine.SceneManagement;
using UnityEngine;

public static class SceneManagerExtension
{
	private static Hashtable arguments;

	public static void LoadScene(string str, Hashtable arg = null){
		arguments = arg;
		SceneManager.LoadScene (str);
	}

	public static Hashtable GetSceneArguments(){
		return arguments;
	}

}
