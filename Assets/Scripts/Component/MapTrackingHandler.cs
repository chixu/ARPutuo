/*==============================================================================
Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.
==============================================================================*/

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Vuforia;
using UnityEngine.UI;

/// <summary>
/// A custom handler that implements the ITrackableEventHandler interface.
/// </summary>
public class MapTrackingHandler : CustomTrackableHandlerBase
{
	[HideInInspector]
	public GameObject quad;
	[HideInInspector]
	public List<GameObject> signs = new List<GameObject>();
	public List<Texture> mapPaths = new List<Texture>();
	public List<Button> pathButtons = new List<Button>();

	public override void Start(){
		base.Start();
		Transform cont = transform.GetChild (0);
		quad = cont.GetChild (0).gameObject;

		for (int i = 0; i < cont.childCount; i++) {
			if (cont.GetChild (i).gameObject.name.StartsWith ("item"))
				signs.Add (cont.GetChild (i).gameObject);
		}
		for (int i = 0; i < signs.Count; i++) {
			Button btn = signs [i].AddComponent<Button> ();
		}
		pathButtons = new List<Button>(GetComponentsInChildren<Button> ());
		for (int i = 0; i < pathButtons.Count; i++) {
			Button btn = pathButtons [i];
			pathButtons [i].onClick.AddListener (()=>OnPathButtonClicked(btn));
		}
		ShowButtons (false);
	}

	public override void TrackingFoundSuccess(){
		base.TrackingFoundSuccess ();
		ShowSigns ();
		ShowButtons ();
	}

	public override void TrackingLostSuccess(){
		base.TrackingLostSuccess ();
		ShowButtons (false);
	}

	public void OnPathButtonClicked(Button btn){
//		int idx = -1;
//		for (int i = 0; i < pathButtons.Count; i++) {
//			if (pathButtons [i] == btn)
//				idx = i;
//		}
//		Debug.Log (idx.ToString ());
//		if (currentTrackObject != null) {
//			if (currentTrackObject is MapTrackingHandler) {
//				MapTrackingHandler cur = (MapTrackingHandler)currentTrackObject;
//				Debug.Log (cur.quad.GetComponent<MeshRenderer> ().material.mainTexture.name);
//				cur.quad.GetComponent<MeshRenderer> ().material.mainTexture = mapPaths [idx + 1];
//			}
//		}
	}

	public void ShowButtons(bool v = true){
		for (int i = 0; i < pathButtons.Count; i++) {
			pathButtons [i].gameObject.SetActive(v);
		}
	}

	public void ShowSigns(){
		for (int i = 0; i < signs.Count; i++) {
			signs [i].transform.GetChild (0).gameObject.SetActive (true);
			for(int j=1; j<signs[i].transform.childCount; j++){
				signs [i].transform.GetChild (j).gameObject.SetActive (false);
			}
		}
	}

	void Update ()
	{
		for (int i = 0; i < signs.Count; i++) {
			if (!signs [i].transform.GetChild (0).gameObject.activeSelf)
				signs [i].transform.Rotate (0, 0, 1);
		}
		if (Input.GetMouseButtonDown (0)) { // if left button pressed...
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit, 100f)) {
				GameObject obj = hit.collider.gameObject;
				if (obj.name == "Sign") {
					obj.SetActive (false);
					GameObject model = obj.transform.parent.GetChild (1).gameObject;
					model.SetActive (true);
				}
			}
		}
	}
}

