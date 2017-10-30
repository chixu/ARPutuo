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
using DG.Tweening;

public class ScanMenuVideoState:ScanVideoState{
	private PopMenuItem item;
	//private bool exited;

	public ScanMenuVideoState(){
		name = "menuvideo";
	}

	public override void OnEnter (Hashtable args = null)
	{
		item = args ["item"] as PopMenuItem;
		//string path = ScanSceneController.currentTrackableObject.GetComponent<CustomTrackableEventHandler>().videoPath;
		ScanSceneController.instant.subtitle.Play(item.subtitlePath);
		item.gameObject.SetActive (true);
		item.transform.DOLocalMove (new Vector3 (0, item.origPosition.y / 10, 0), 0.3f).SetEase (Ease.OutQuad);
		item.transform.DOScale (item.origScale * 2, 0.3f).SetEase (Ease.OutQuad).OnComplete (PlayVideo);
	}

	private void PlayVideo(){
        //scene.mediaPlayer.OpenVideoFromFile(MediaPlayer.FileLocation.AbsolutePathOrURL, item.videoPath, true); 
        //item.meshRenderer.material = null;// ScanSceneController.instant.videoMaterial;
		//scene.description.gameObject.SetActive (false);
        //VideoController.instant.OpenAndPlay(item.videoPath);
        //VideoController.instant._videoSeekSlider.gameObject.SetActive(true);
        //scene.mediaPlayer.Rewind(false);
        //scene.mediaPlayer.Play ();
		TrackStart (item.name, "Video");
		GameObject curr = item.gameObject;
		string path = item.videoPath;
		RegisterClick (curr, "menu4");
		VideoController.instant.Play(curr, path);
    }

	public override void OnExit(){
		//base.OnExit ();
		TrackEnd (item.name, "Video");
		OnClick click = item.gameObject.GetComponent<OnClick> ();
		if (click != null) {
			click.OnClickHandler = null;
		}
		VideoController.instant.Stop ();
	}

	public override void OnBackClick(){
		scene.videoPanel.Hide ();
		scene.SetState("menu4", new Hashtable(){{"showImmediate", true}});
	}

}
