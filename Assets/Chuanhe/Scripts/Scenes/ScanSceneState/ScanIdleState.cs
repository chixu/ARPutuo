 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System;
using UnityEngine.Networking;
using Vuforia;
using UnityEngine.SceneManagement;
using System.Xml.Linq;

public class ScanIdleState: ScanSceneState
{
	public ScanIdleState(){
		name = "idle";
	}

	public override void OnEnter (Hashtable args = null)
	{
		//base.OnEnter (args);
		ScanSceneController.currentTrackableObject = null;
		scene.title.text = I18n.Translate (scene.sceneName+"_scan_title");
		scene.description.text = I18n.Translate (scene.sceneName+"_desc");
		scene.scanner.SetActive (true);
		//VideoController.instant.videoPlayer.gameObject.SetActive(false);
		scene.ShowDescription();
	}

	public override void OnBackClick(){
		ScanSceneController.instant.exited = true;
		OnExit ();
		SceneManager.LoadScene ("Selection");
	}

	public override void OnExit(){
		scene.scanner.SetActive (false);
		base.OnExit();
	}
}
