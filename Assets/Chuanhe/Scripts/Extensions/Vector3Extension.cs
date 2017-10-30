using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector3Extension {

	public static Vector3 SetY(this Vector3 vect, float value){
		return new Vector3 (vect.x, value, vect.z);
	}

	public static Vector3 SetX(this Vector3 vect, float value){
		return new Vector3 (value, vect.y, vect.z);
	}

	public static Vector3 SetZ(this Vector3 vect, float value){
		return new Vector3 (vect.x, vect.y, value);
	}

	public static Vector3 One(this Vector3 vect){
		return new Vector3 (1, 1, 1);
	}
}
