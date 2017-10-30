using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class VideoPanelPlayer : MonoBehaviour {
	public GameObject playButton;
	public GameObject videoPlayer;
	//public bool isPlaying;
	public VideoPlayer player;

	public bool shown{ 
		get {
			return gameObject.activeSelf;
		}
	}

	public void Play(VideoPlayer _player){
		player = _player;
		playButton.SetActive (false);
		player.renderMode = VideoRenderMode.RenderTexture;
		videoPlayer.GetComponent<RawImage>().texture = player.texture;
	}

	public void OnClick(){
		if (player.isPlaying) {
			player.Pause ();
			playButton.SetActive (true);
		} else {
			player.Play ();
			playButton.SetActive (false);

		}
	}

	public void Show(){
		//shown = true;
		gameObject.SetActive (true);
	}

	public void Hide(){
		//shown = false;
		gameObject.SetActive (false);
	}
}
