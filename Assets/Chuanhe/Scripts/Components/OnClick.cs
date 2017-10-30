using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class OnClick : MonoBehaviour
{

	public Action<GameObject> OnClickHandler;

	void Update ()
	{
		if (Input.GetMouseButtonDown (0)) { // if left button pressed...
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit, 100f)) {

				Logger.Log ("GetMouseButtonDown ", "greeen");
				if (OnClickHandler != null)
					OnClickHandler (this.gameObject);
			}
		}
	}
}
