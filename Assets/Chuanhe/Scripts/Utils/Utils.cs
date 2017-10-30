using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour {


	public static List<T> ReorderArray<T>(List<T> list){
		List<T> res = new List<T> ();
		List<int> idxes = IntArray(list.Count);
		for (int i = 0; i < list.Count; i++) {
			int idx = UnityEngine.Random.Range (0, idxes.Count - 1);
			res.Add (list [idxes[idx]]);
			idxes.RemoveAt (idx);
		}
		return res;
	}

	public static List<int> IntArray(int num){
		List<int> res = new List<int> ();
		for (int i = 0; i < num; i++)
			res.Add (i);
		return res;
	}

	public static List<int> RandomIntArray(int num){

		return ReorderArray<int> (IntArray (num));
	}

	public static string ReplaceKeyword (string str, Dictionary<string, string> dict)
	{
		foreach (string key in dict.Keys) {
			if (str.Contains ("{%" + key + "%}"))
				str = str.Replace ("{%" + key + "%}", dict [key]);
		}
		return str;
	}

	public static string ApplyRandomVersion (string str){
		return str + "?v=" + UnityEngine.Random.Range(100000,999999).ToString ();
	}

	//-40 mod 90 = 50
	public static float ModPositive(float a, float b){
		int i = 0;
		i = Mathf.FloorToInt (a / b);
		return a - b * i;
	}
}
