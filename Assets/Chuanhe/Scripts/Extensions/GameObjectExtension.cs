using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;

public static class GameObjectExtension {

	public static void RegisterClickEvent(this GameObject obj){
		obj.AddComponent<OnClick> ();
		Collider collider = obj.GetComponent<Collider> ();
		if (collider == null)
			obj.AddComponent<Collider> ();
	}

	public static void ShowChildByName(this GameObject o, string name){
		o.SetActive (name != "null");
		for (int i = 0; i < o.transform.childCount; i++) {
			GameObject obj = o.transform.GetChild(i).gameObject;
			obj.SetActive (obj.name == name);
		}
	}

	public static void RegisterUIClickEvent(this GameObject obj, UnityAction callback){
		Button btn = obj.AddComponent<Button> ();
		btn.onClick.AddListener (callback);
		//Collider collider = obj.GetComponent<Collider> ();
		//if (collider == null)
		//	obj.AddComponent<Collider> ();
	}

	public static GameObject GetChildByName(this GameObject o, string name){
		for (int i = 0; i < o.transform.childCount; i++) {
			GameObject obj = o.transform.GetChild(i).gameObject;
			if(obj.name == name)
				return obj;
		}
		return null;
	}

	public static GameObject GetChildByNameInChildren(this GameObject o, string name){
		for (int i = 0; i < o.transform.childCount; i++) {
			GameObject obj = o.transform.GetChild(i).gameObject;
			if (obj.name == name)
				return obj;
			else {
				obj = obj.GetChildByNameInChildren (name);
				if (obj != null)
					return obj;
			}
		}
		return null;
	}
}
