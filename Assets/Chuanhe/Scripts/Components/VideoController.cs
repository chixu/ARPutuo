using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Video;

//-----------------------------------------------------------------------------
// Copyright 2015-2016 RenderHeads Ltd.  All rights reserverd.
//-----------------------------------------------------------------------------

public class VideoController : MonoBehaviour
{
	//public MediaPlayer	_mediaPlayer;
	public Slider videoSlider;
	//private bool _wasPlayingOnScrub;
	//private float _setVideoSeekSliderValue;
	public static VideoController instant;
	private string prevPath;
	public VideoPlayer videoPlayer;
	public GameObject videoContainer;
	public AudioSource audioSource;
	private bool isDown = false;
	//private bool isClick = false;
	private long _prevFrame;
	private long _nextFrame = -1;
	public int minVideoFrameUnit = 10;
	//		public void OnMuteChange()
	//		{
	//			if (_mediaPlayer)
	//			{
	//				_mediaPlayer.Control.MuteAudio(_MuteToggle.isOn);
	//			}
	//		}

	public void Play (GameObject obj, string path)
	{
		isDown = false;
		obj.SetActive (false);
		videoContainer = obj;
		//videoPlayer = obj.GetComponent<VideoPlayer>();
		StartCoroutine (LoadAndPlay (obj, path));
	}

	public void HideVideoContainer(){
		if (videoContainer != null)
			videoContainer.SetActive (false);
	}

	public void Stop ()
	{
		videoSlider.gameObject.SetActive (false);
		videoPlayer.targetMaterialRenderer = null;
		HideVideoContainer ();
		//bottomText.gameObject.SetActive (true);
		videoSlider.value = 0;
		videoPlayer.frame = 0;
		videoPlayer.Pause ();
	}

	private IEnumerator LoadAndPlay (GameObject obj, string path)
	{
		videoSlider.value = 0;
		if (path != prevPath) {
			//obj.AddComponent<VideoPlayer>();
			//videoPlayer = obj.GetComponent<VideoPlayer>();
			//videoPlayer.playOnAwake = false;
			//audioSource.playOnAwake = false;
			//audioSource.Pause();
			videoPlayer.source = VideoSource.Url;
			videoPlayer.url = path;

			//videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
			//videoPlayer.controlledAudioTrackCount = 1;
			//videoPlayer.EnableAudioTrack (0, true);
			//videoPlayer.SetTargetAudioSource (0, audioSource);
			//obj.AddComponent<AudioSource>();
			//AudioSource audio = obj.GetComponent<AudioSource>();
			//videoPlayer.SetTargetAudioSource (0, audio);
			videoPlayer.Prepare ();
			Logger.Log ("Preparing Video " + path);
			while (!videoPlayer.isPrepared) {
				yield return null;
			}
			videoPlayer.renderMode = VideoRenderMode.MaterialOverride;
			videoPlayer.targetMaterialRenderer = obj.GetComponent<MeshRenderer> ();
			audioSource.Play ();
			videoPlayer.Play ();
		} else {
			videoPlayer.renderMode = VideoRenderMode.MaterialOverride;
			VideoController.instant.videoPlayer.targetMaterialRenderer = obj.GetComponent<MeshRenderer> ();
			videoPlayer.Play ();
		}
		obj.gameObject.SetActive (true);
		ScanSceneController.instant.ShowVideoSlide ();
		prevPath = path;
	}

	//public void OpenAndPlay(string path){
	//	if (prevPath != path) {
	//		_mediaPlayer.OpenVideoFromFile (MediaPlayer.FileLocation.AbsolutePathOrURL, path, true);
	//		//item.meshRenderer.material = ScanSceneController.instant.videoMaterial;
	//	} else {
	//		_mediaPlayer.Rewind (false);
	//		_mediaPlayer.Play ();
	//	}
	//	prevPath = path;
	//	_videoSeekSlider.gameObject.SetActive(true);

	//}



	public void OnVideoSeekSlider ()
	{
		//if (_mediaPlayer && _videoSeekSlider && _videoSeekSlider.value != _setVideoSeekSliderValue) {
		//	_mediaPlayer.Control.Seek (_videoSeekSlider.value * _mediaPlayer.Info.GetDurationMs ());
		//}
		//if (!isDown)
		//	return;
		//isClick = true;
		//Logger.Log((videoSlider == null).ToString() + " " +  (videoPlayer == null).ToString());
		int frame = Mathf.FloorToInt (this.videoSlider.value * this.videoPlayer.frameCount);
		if (Mathf.Abs (videoPlayer.frame - frame) > minVideoFrameUnit * 2) {
			videoPlayer.frame = frame;
			_nextFrame = frame;
		}
	}

	public void OnVideoSliderDown ()
	{
		isDown = true;
		Logger.Log ("down", "blue");
	}

	public void OnVideoSliderUp ()
	{
		isDown = false;
		Logger.Log ("up", "blue");
	}

	void Awake ()
	{
		VideoController.instant = this;
		//if (_mediaPlayer) {
		//	_mediaPlayer.Events.AddListener (OnVideoEvent);
		//}
	}

	void Update ()
	{
		if (!videoSlider.gameObject.activeSelf)
			return;

		//Debug.Log("update");


		if (videoPlayer && !isDown && videoPlayer.frameCount != 0) {
			float value = (float)videoPlayer.frame / videoPlayer.frameCount;
			if (Mathf.Abs (videoPlayer.frame - _prevFrame) <= minVideoFrameUnit) {
				//Logger.Log ("Update " + videoPlayer.frame);
				//if (!frameChanged)


				if (_nextFrame != -1 && videoPlayer.frame > _nextFrame && videoPlayer.frame < _nextFrame + 2 * minVideoFrameUnit)
					_nextFrame = -1;
				else
					videoSlider.value = value;
			}
		}
		//Logger.Log ("Update ++++++ " + videoPlayer.frame.ToString() + " " +  _prevFrame + " " + videoSlider.value);
		_prevFrame = videoPlayer.frame;

	}

	// Callback function to handle events
	//public void OnVideoEvent (MediaPlayer mp, MediaPlayerEvent.EventType et, ErrorCode errorCode)
	//{
	//	switch (et) {
	//	case MediaPlayerEvent.EventType.ReadyToPlay:
	//		break;
	//	case MediaPlayerEvent.EventType.Started:
	//		break;
	//	case MediaPlayerEvent.EventType.FirstFrameReady:
	//		break;
	//	case MediaPlayerEvent.EventType.FinishedPlaying:
	//		break;
	//	}

	//	Debug.Log ("Event: " + et.ToString ());
	//}

}
