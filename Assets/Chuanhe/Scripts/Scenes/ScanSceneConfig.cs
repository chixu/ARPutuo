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
using System.Reflection;

public class ScanSceneConfig : MonoBehaviour
{
	//public string remoteUrl = "";
	//public string fileName = "myassets.dlc";
	//public string configName = "config.json";
	//public Text text;
	//public MediaPlayer mediaPlayer;
	//public Material videoMaterial;
	//public GameObject buttonPanel;
	public VideoPanelPlayer videoPanel;
	public Text title;
	public Text description;
	public GameObject infoPanel;
	public GameObject planePrefab;
	public ScanSceneState state;
	//from prev scene
	public XElement data;
	//info panel
	public Text infoTitle;
	public Text infoTip1;
	public Text infoTip2;
	public Text infoTip3;
	private string sceneName;
	private string type;

	protected virtual void Awake ()
	{
		Debug.Log ("ScanSceneConfig Awake");
		StatusBar.Hide ();
		sceneName = SceneManagerExtension.GetSceneArguments () ["name"].ToString ();
		type = SceneManagerExtension.GetSceneArguments () ["type"].ToString ();
		data = SceneManagerExtension.GetSceneArguments () ["data"] as XElement;
		ReplaceSceneController ();
	}

	public void ReplaceSceneController(){
//		Debug.Log (type);
//		if (string.IsNullOrEmpty (type))
//			return;
		ScanSceneController scene = null;
//		if (type == "map")
//			scene = gameObject.AddComponent<MapScanScene> ();
//		else
			scene = gameObject.AddComponent<ScanSceneController> ();
		if (scene != null) {
			scene.videoPanel = videoPanel;
			scene.title = title;
			scene.description = description;
			scene.infoPanel = infoPanel;
			scene.planePrefab = planePrefab;
			scene.infoTitle = infoTitle;
			scene.infoTip1 = infoTip1;
			scene.infoTip2 = infoTip2;
			scene.infoTip3 = infoTip3;
		}
	}

	public void OnBackClick ()
	{
		ScanSceneController.instant.state.OnBackClick ();
	}

	public void OnInfoClick ()
	{
		ScanSceneController.instant.infoPanel.SetActive (!infoPanel.activeSelf);
	}

	public void OnInfoLinkClick ()
	{
		Application.OpenURL (Request.RemoteUrl + I18n.Translate(sceneName+"_infolink") );
	}

	public void OnInfoCloseClick ()
	{
		ScanSceneController.instant.infoPanel.SetActive (false);
	}
}
