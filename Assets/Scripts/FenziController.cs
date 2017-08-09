using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenziController : MonoBehaviour {

	public GameObject[] labels;
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < labels.Length; i++) {
			labels [i].transform.rotation = Quaternion.Euler(90, 0, 0);
			labels [i].transform.Rotate (0, 180, 0);
		}
	}
}
