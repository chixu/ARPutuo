using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Vuforia;

public class PopMenu : MonoBehaviour {

	public List<PopMenuItem> menuItems;
	private bool showing;
	//private TrackableBehaviour mTrackableBehaviour;
	//private CustomTrackableEventHandler customTrackable;
	//public Material playerMateral;


	IEnumerator PopAllItems ()
	{
		for (int i = 0; i < menuItems.Count; i++) {
			menuItems [i].gameObject.SetActive (false);
		}
		List<int> idxes = Utils.RandomIntArray (4);
		for (int j = 0; j < menuItems.Count; j++) {
			int i = idxes [j];
			if (showing) {
				menuItems [i].Zoom ();
				yield return new WaitForSeconds (0.3f);
			} else {
				yield break;
			}
		}
	}


	public void Show()
	{
		showing = true;
		StartCoroutine (PopAllItems ());
		HideAll3DModels ();
	}

	public void Hide ()
	{
		//playerPlane.SetActive (false);
//		for (int i = 0; i < menuItems.Count; i++) {
//			menuItems [i].Reset ();
//		}
		showing = false;
		HideAllItems ();
		HideAll3DModels ();
	}

	//		public void Show3DObj(int index){
	//			threeDObj.transform.GetChild (index).gameObject.SetActive (true);
	//		}

	public void HideAll3DModels ()
	{
		//int count = threeDObj.transform.childCount;
		for (int i = 0; i < menuItems.Count; i++) {
			//threeDObj.transform.GetChild (i).gameObject.SetActive (false);
			if (menuItems [i].threeDObject != null)
				menuItems [i].threeDObject.SetActive (false);
		}
	}

	public void HideAllItems (int exception = -1)
	{
		for (int i = 0; i < menuItems.Count; i++) {
			menuItems [i].gameObject.SetActive (exception == i);
			menuItems [i].StopIdle ();
			menuItems [i].transform.DOKill ();
		}
	}
	//		#endregion // PRIVATE_METHODS

	public void ShowMenu ()
	{
		//customTrackable.StopVideo ();
		//SceneController.instant.HideAll ();
		HideAll3DModels ();
		for (int i = 0; i < menuItems.Count; i++) {
			menuItems [i].Reset ();
			menuItems [i].Idle ();
		}
	}
}
