using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class Scroll : MonoBehaviour {
	public string direction = "v";
	public float from;
	public float to;
	private GameObject mask;
	private GameObject scroll;
	private float origMaskMax;
	private RectTransform maskRect;
	private RectTransform scrollRect;
	public float scrollSpeed;
	private bool playing;
	// Use this for initialization
	void Start () {
		mask = gameObject.GetChildByName ("Mask");
		scroll = gameObject.GetChildByName ("Scroll");
		scrollRect = scroll.GetComponent<RectTransform> ();
		maskRect = mask.GetComponent<RectTransform> ();
		if (direction == "v") {
			origMaskMax = maskRect.offsetMin.y;
		} else {
			origMaskMax = maskRect.offsetMax.x;
		}
		Logger.Log (mask.GetComponent<RectTransform> ().offsetMin.y.ToString());
		Reset ();
		Play ();
	}

	public void Reset(){
		if (direction == "v") {
			maskRect.offsetMin = new Vector2 (maskRect.offsetMin.x, origMaskMax + from - to);
			scrollRect.localPosition = scrollRect.localPosition.SetY (this.from);
		} else {
			maskRect.offsetMax = new Vector2 (origMaskMax + from - to, maskRect.offsetMax.y);
			scrollRect.localPosition = scrollRect.localPosition.SetX (this.from);
		}
		
	}

	public void Play(){
		Reset ();
		playing = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (!playing)
			return;
		if (direction == "v") {
			float newY = scrollRect.localPosition.y - scrollSpeed * Time.deltaTime;
			if (newY < to) {
				playing = false;
			} else {
				scrollRect.localPosition = scrollRect.localPosition.SetY (newY);
				maskRect.offsetMin = new Vector2 (maskRect.offsetMin.x, maskRect.offsetMin.y - scrollSpeed * Time.deltaTime);

			}
		} else {
			float newX = scrollRect.localPosition.x + scrollSpeed * Time.deltaTime;
			if (newX > to) {
				playing = false;
			} else {
				scrollRect.localPosition = scrollRect.localPosition.SetX (newX);
				maskRect.offsetMax = new Vector2 (maskRect.offsetMax.x + scrollSpeed * Time.deltaTime, maskRect.offsetMax.y);
			}
		}
	}
}
