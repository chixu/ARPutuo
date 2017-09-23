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

using System.Xml.Linq;
/// <summary>
/// A custom handler that implements the ITrackableEventHandler interface.
/// </summary>
public class MapScanScene : ScanSceneController
{
//	protected override void Awake ()
//	{
//		Debug.Log ("MapScanScene");
//		base.Awake ();
//		Debug.Log ("dsafsdfs");
//	}

	void InstantiateMapObject(XElement info, TrackableBehaviour tb, out UnityEngine.Object asset){
		GameObject buttonCanvasPrefab = null, buttonPrefab = null;
		string prefabName = Xml.Attribute (info, "prefab");
		asset = loadedAssets.ContainsKey (prefabName) ? loadedAssets [prefabName] : new GameObject ();
		CustomTrackableEventHandler cte = tb.gameObject.GetComponent<CustomTrackableEventHandler> ();
		GameObject obj = InstantiateObject (tb, asset);
	}

	protected  override IEnumerator LoadDataSet(){
		ClearAndLoadDataSet ();
		IEnumerable<TrackableBehaviour> tbs = Vuforia.TrackerManager.Instance.GetStateManager ().GetTrackableBehaviours ();
		GameObject buttonCanvasPrefab = null, buttonPrefab = null;// = Resources.Load("Prefab/ScanButtonCanvas") as GameObject;
		foreach (TrackableBehaviour tb in tbs) {

			Logger.Log (tb.TrackableName, "purple");
			tb.gameObject.name = tb.TrackableName;
			XElement info = Xml.GetChildByAttribute (itemInfos, "id", tb.TrackableName);
			if (info == null)
				continue;
			string objType = Xml.Attribute (info, "type");
			tb.gameObject.AddComponent<DefaultTrackableEventHandler> ();
			tb.gameObject.AddComponent<MapTrackingHandler> ();
			tb.gameObject.AddComponent<TurnOffBehaviour> ();
			MapTrackingHandler mth = tb.gameObject.GetComponent<MapTrackingHandler> ();
			UnityEngine.Object asset = null;
			if (objType == "map") {
				InstantiateMapObject(info, tb, out asset);
			}
		}
		ObjectTracker objectTracker = Vuforia.TrackerManager.Instance.GetTracker<ObjectTracker> ();
		if (!objectTracker.Start ()) {
			Debug.Log ("<color=yellow>Tracker Failed to Start.</color>");
		}
		yield return null;
	}
}

