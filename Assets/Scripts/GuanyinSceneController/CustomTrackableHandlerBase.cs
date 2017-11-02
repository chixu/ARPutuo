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

namespace Guanyin
{
	/// <summary>
	/// A custom handler that implements the ITrackableEventHandler interface.
	/// </summary>
	public class CustomTrackableHandlerBase : MonoBehaviour,
	ITrackableEventHandler
	{
		#region PRIVATE_MEMBER_VARIABLES

		private TrackableBehaviour mTrackableBehaviour;
		//public bool tracked;
		//private GameObject quad;
		//public bool tracked;
		//public int sphereIndex;

		#endregion // PRIVATE_MEMBER_VARIABLES



		#region UNTIY_MONOBEHAVIOUR_METHODS

		public virtual void Start ()
		{
			mTrackableBehaviour = GetComponent<TrackableBehaviour> ();
			if (mTrackableBehaviour) {
				mTrackableBehaviour.RegisterTrackableEventHandler (this);
			}
		}

		#endregion // UNTIY_MONOBEHAVIOUR_METHODS



		#region PUBLIC_METHODS

		/// <summary>
		/// Implementation of the ITrackableEventHandler function called when the
		/// tracking state changes.
		/// </summary>
		public virtual void OnTrackableStateChanged (
			TrackableBehaviour.Status previousStatus,
			TrackableBehaviour.Status newStatus)
		{
			if (newStatus == TrackableBehaviour.Status.DETECTED ||
			   newStatus == TrackableBehaviour.Status.TRACKED ||
			   newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED) {
				OnTrackingFound ();
			} else {
				OnTrackingLost ();
			}
		}

		#endregion // PUBLIC_METHODS



		#region PRIVATE_METHODS

		private void OnTrackingFoundDefault ()
		{
			Renderer[] rendererComponents = GetComponentsInChildren<Renderer> (true);
			Collider[] colliderComponents = GetComponentsInChildren<Collider> (true);

			// Enable rendering:
			foreach (Renderer component in rendererComponents) {
				component.enabled = true;
			}

			// Enable colliders:
			foreach (Collider component in colliderComponents) {
				component.enabled = true;
			}

			Debug.Log ("Trackable " + mTrackableBehaviour.TrackableName + " found");
		}


		private void OnTrackingLostDefault ()
		{
			Renderer[] rendererComponents = GetComponentsInChildren<Renderer> (true);
			Collider[] colliderComponents = GetComponentsInChildren<Collider> (true);

			// Disable rendering:
			foreach (Renderer component in rendererComponents) {
				component.enabled = false;
			}

			// Disable colliders:
			foreach (Collider component in colliderComponents) {
				component.enabled = false;
			}

			Debug.Log ("Trackable " + mTrackableBehaviour.TrackableName + " lost");
		}


//		public virtual void TrackingFoundSuccess ()
//		{
//		
//			Light[] lights = GetComponentsInChildren<Light> (true);
//			for (int i = 0; i < lights.Length; i++)
//				lights [i].gameObject.SetActive (true);
//			ScanSceneController.instant.PlayAnimation ();
//		}

		private void OnTrackingFound ()
		{
			//if (SceneController.instant.state == SceneState.VRFullScreen)
			//	return;
			if (ScanSceneController.instant.playing) {
				if (ScanSceneController.instant.state == ScanSceneState.AnimationAfterName) {
					ScanSceneController.instant.guanyinContainer.transform.SetParent(transform, false);
				}
				return;
			}
			//OnTrackingFoundDefault();
			for (int i = 0; i < transform.childCount; i++) {
				transform.GetChild (i).gameObject.SetActive (true);
			}
			ScanSceneController.currentTrackableObject = this.gameObject;
			ScanSceneController.instant.PlayAnimation ();
		}

//		public virtual void TrackingLostSuccess ()
//		{
//			Light[] lights = GetComponentsInChildren<Light> ();
//			foreach (Light l in lights)
//				l.gameObject.SetActive (false);
//			ScanSceneController.instant.Reset ();
//		}

		private void OnTrackingLost ()
		{
			if (ScanSceneController.instant.playing) {
				if (ScanSceneController.instant.state == ScanSceneState.AnimationAfterName) {
					ScanSceneController.instant.guanyinContainer.transform.SetParent(ScanSceneController.instant.cameraObjectContainer.transform, false);
				}
				return;
			}
			for (int i = 0; i < transform.childCount; i++) {
				transform.GetChild (i).gameObject.SetActive (false);
			}
			ScanSceneController.currentTrackableObject = null;
			ScanSceneController.instant.Reset ();
		}

		public virtual void Update ()
		{
//		if (!(SceneController.instant.state == SceneState.VR))
//			return;
//		if (Input.GetMouseButtonDown (0)) {
//			Ray ray = SceneController.instant.activeCamera.ScreenPointToRay (Input.mousePosition);
//			RaycastHit rayhit;
//			if (Physics.Raycast (ray, out rayhit)) {
//				if (rayhit.transform.gameObject == quad) {
//					Debug.Log ("clicked");
//					//SceneController.instant.ShowVRFullScreen ();
//				}
//			}
//
//			//Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);
//
//			//Debug.Log ((ray.direction.normalized * 0.5f).x.ToString() + " " + (ray.direction.normalized * 0.5f).y.ToString() + " " + (ray.direction.normalized * 0.5f).z.ToString() ) ;
//
//		}
		}

		#endregion // PRIVATE_METHODS
	}
}

