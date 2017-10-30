using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Vuforia;

public class PopMenuItem : MonoBehaviour {

	private float origY;
	public Vector3 origPosition;
	public Vector3 origScale;
	private bool isIdle;
	public float floatSpeed = 1;
	public float floatAmplitude = 0.1f;
	private float tick;
	public int index;
	public PopMenu menu;
	public CustomTrackableEventHandler trackableHandler;
	public MeshRenderer meshRenderer;
	public Material material;
	public GameObject threeDObject = null;
	public string videoPath;
	public string subtitlePath;
	public string name;

	public void Initiate(){
		origY = this.transform.localPosition.y;
		origPosition = this.transform.localPosition;
		origScale = this.transform.localScale;
		meshRenderer = GetComponent<MeshRenderer> ();
		material = meshRenderer.material;
		//transform = GetComponent<Transform> ();
	}

	public void Reset(){
		isIdle = false;
		gameObject.SetActive (true);
		transform.localPosition = origPosition;
		transform.localScale = origScale;
		if(meshRenderer)meshRenderer.material = material;
	}


	public void Idle(){
		isIdle = true;
		tick = 0;
	}

	public void StopIdle(){
		isIdle = false;
		//this.transform.localPosition = this.transform.localPosition.SetY (origY);
	}

	public void Zoom(){
		meshRenderer.material = material;
		transform.localScale = new Vector3(origScale.x * 0.3f, origScale.y * 0.3f,origScale.z * 0.3f);
		transform.localPosition = Vector3.zero;
		gameObject.SetActive (true);
		//menuItems [0].transform.DOMove (new Vector3(0,0,0),3);
		transform.DOScale (origScale,	0.5f);
		Vector3 moveTo = Vector3.zero;
//		if (i == 0) {
//			moveTo = new Vector3(-moveX,moveY,moveZ);
//		}else if (i == 1) {
//			moveTo = new Vector3(moveX,moveY,moveZ);
//		}else if (i == 2) {
//			moveTo = new Vector3(-moveX,moveY,-moveZ);
//		}else if (i == 3) {
//			moveTo = new Vector3(moveX,moveY,-moveZ);
//		}
		//menuItems [i].transform.DOMove (moveTo,startTween).SetEase(Ease.OutCubic).SetDelay(UnityEngine.Random.Range (0f, 0.5f)).OnComplete(menuItems [i].Idle);
		transform.DOLocalMove (origPosition,0.5f).SetEase(Ease.OutCubic).OnComplete(Idle);

	}


	void Update(){
		if (isIdle) {
			tick += Time.deltaTime * floatSpeed;
			this.transform.localPosition = this.transform.localPosition.SetY (origY + Mathf.Sin(tick)* floatAmplitude);
		}
	}

	void OnMouseDown(){

		Debug.Log(index.ToString() + " is HIT!!!");
		if (threeDObject == null) {
			//StopIdle ();
			//transform.DOLocalMove (new Vector3 (0, origPosition.y * 2, 0), 0.3f).SetEase (Ease.OutQuad);
			//transform.DOScale (origScale * 2, 0.3f).SetEase (Ease.OutQuad).OnComplete (PlayVideo);

			ScanSceneController.instant.SetState ("menuvideo", new Hashtable{{"item",this}});
		} else {

			ScanSceneController.instant.SetState ("menuobject", new Hashtable{{"item",this}});
			//SceneController.instant.ShowBackButtonOnly ();
			//threeDObject.SetActive (true);
			//menu.HideAllItems ();
		}
	}

	void PlayVideo(){
		//menu.HideAllItems (index);
		//trackableHandler.PlayVideo (videoPath);
		//meshRenderer.material = menu.playerMateral;
		//SceneController.instant.ShowTop ();
		//trackableMenu.playerPlane.SetActive (true);

		//ScanSceneController.instant.SetState ("menuvideo", new Hashtable{{"path",videoPath}});
	}
}
