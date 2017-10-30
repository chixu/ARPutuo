using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectExtension {

	public static void RegisterClickEvent(this GameObject obj){
		obj.AddComponent<OnClick> ();
		Collider collider = obj.GetComponent<Collider> ();
		if (collider == null)
			obj.AddComponent<Collider> ();
	}


	public static GameObject GetChildByName(this GameObject o, string name){
		for (int i = 0; i < o.transform.childCount; i++) {
			GameObject obj = o.transform.GetChild(i).gameObject;
			if(obj.name == name)
				return obj;
		}
		return null;
	}
}
