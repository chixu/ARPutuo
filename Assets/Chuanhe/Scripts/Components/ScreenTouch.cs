using System.Collections;		
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScreenTouch : MonoBehaviour {
	[HideInInspector]
	public float doubleTouchDeltaX;
	[HideInInspector]
	public float doubleTouchDeltaY;
	[HideInInspector]
	public float doubleTouchDeltaDis;
//	[HideInInspector]
//	public float doubleTouchDeltaX;
//	[HideInInspector]
//	public float doubleTouchDeltaY;
	[HideInInspector]
	private Vector2 preTouch1Position;
	[HideInInspector]
	private Vector2 preTouch2Position;
	[HideInInspector]
	public bool enabled = true;
	[HideInInspector]
	public bool doubleTouched = false;
	private Vector2 touched1Position;
	private Vector2 touched2Position;
	public float doubleTouchedDis;
	public float doubleTouchChangedDis = 0;
	public Action onDoubleTouchBegin;
	public Action onDoubleTouchEnd;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (!enabled) {
			doubleTouchDeltaX = 0;
			doubleTouchDeltaY = 0;
			return;
		}
//		if (Input.touchCount == 2 && Input.GetTouch (0).phase == TouchPhase.Moved) {
//			Touch touch = Input.GetTouch (0);
//			doubleTouchDeltaX = (touch1.deltaPosition.x + touch2.deltaPosition.x) / 2;
//			doubleTouchDeltaY = (touch1.deltaPosition.y + touch2.deltaPosition.y) / 2;
//		} else {
//			touchDeltaX = 0;
//			touchDeltaY = 0;
//		}
		if (Input.touchCount == 2 && Input.GetTouch (0).phase == TouchPhase.Moved && Input.GetTouch (1).phase == TouchPhase.Moved) {
			Touch touch1 = Input.GetTouch (0);
			Touch touch2 = Input.GetTouch (1);
			doubleTouchDeltaX = (touch1.deltaPosition.x + touch2.deltaPosition.x) / 2;
			doubleTouchDeltaY = (touch1.deltaPosition.y + touch2.deltaPosition.y) / 2;
			if (!doubleTouched) {
				touched1Position = touch1.position;
				touched2Position = touch2.position;
				doubleTouchedDis =  (touched1Position - touched2Position).magnitude;
				if (onDoubleTouchBegin != null)
					onDoubleTouchBegin.Invoke ();
			}
			if (doubleTouched) {
				doubleTouchDeltaDis = (touch1.position - touch2.position).magnitude - (preTouch1Position - preTouch2Position).magnitude;
				doubleTouchChangedDis = (touch1.position - touch2.position).magnitude - doubleTouchedDis; 
			}
			preTouch1Position = touch1.position;
			preTouch2Position = touch2.position;
			doubleTouched = true;

		} else {
			doubleTouchDeltaX = 0;
			doubleTouchDeltaY = 0;
			doubleTouchDeltaDis = 0;
			doubleTouched = false;
			doubleTouchChangedDis = 0;
		}
	}
}
