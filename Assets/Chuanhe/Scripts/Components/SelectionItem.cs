using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public static class SelectionItemState{
	public static string Active = "active";
	public static string Pending = "pending";
}

public class SelectionItem : MonoBehaviour {
	public string type;
	public string title;
	public string id;
	//public Text title;
	//public Text description;
	public string helpLink;
	public Image image;
	public GameObject btnInfo;
	private Action<SelectionItem> onClickHandler;
	public string nextSceneName;
	public string state;
	public string size;
	// Use this for initialization


	public void SetOnClick(Action<SelectionItem> handler){
		onClickHandler = handler;
	}
		
//	public void OnClick(){
//		StartCoroutine (OnClickHandler ());
//	}

	public void OnClick(){
		if (onClickHandler != null)
			onClickHandler.Invoke (this);
	}


	public void OnInfoClick(){
		Application.OpenURL (helpLink);
	}
	//public void Init
}
