using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class SelectionItem : MonoBehaviour {
	public string type;
	public Text title;
	public Text description;
	public string helpLink;
	public Image image;
	public GameObject btnInfo;
	private Action<SelectionItem> onClickHandler;
	public string nextSceneName;
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
