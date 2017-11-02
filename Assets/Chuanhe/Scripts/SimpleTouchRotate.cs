using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTouchRotate : MonoBehaviour {

	//float x;
	//float y;

	float rotationSpeed = .2f;
	Camera gameCamera;
	private Quaternion originalRotation;
	public string upDireciton = "x";
	public string rightDirection = "-z";
	public bool upEnabled = true;
	public bool rightEnabled = true;
	[HideInInspector]
	public bool rotating;
	private Vector3 touchedPosition;
	private Vector3 touchedObjRotation;
	public float horizontalRange = -1;
	public float verticalRange = -1;
	private Vector3 origRotation;

	void Start(){
		origRotation = transform.localRotation.eulerAngles;
	}

//	void Awake(){
//		originalRotation = this.transform.localRotation;
//		originalScale = this.transform.localScale;
//		originalPosition = this.transform.localPosition;
//	}
//
//	void OnEnable(){
//		this.transform.localRotation = originalRotation;
//		this.transform.localPosition = originalPosition;
//		this.transform.localScale = originalScale;
//		ResetParameters ();
//	}
//
//	void ResetParameters(){
//			
//		accumulateScale = 1.0f;
//		//accumulateXAngle = 0f;
//		//accumulateYAngle = 0f;
//		lastDist = 0;
//	}


	Vector3 ApplyTransform(Vector3 v, string dir, float value, float range){
		string axis = dir.Substring (dir.Length - 1);
		bool clamp = range > -1;
		value = dir.Length == 1 ? value : -value;
		if (axis == "x") {
			float x = Angle.ToStardard (v.x) + value;
			return v.SetX (clamp? Mathf.Clamp(x, origRotation.x - range, origRotation.x + range): x);
		} else if (axis == "y") {
			float y = Angle.ToStardard (v.y) + value;
			return v.SetY (clamp? Mathf.Clamp(y, origRotation.y - range, origRotation.y + range): y);
		} else {
			float z = Angle.ToStardard (v.z) + value;
			return v.SetZ (clamp? Mathf.Clamp(z, origRotation.z - range, origRotation.z + range): z);
		}
	}

	void TouchUpdate(){
		if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved) 
		{
			Touch touch = Input.GetTouch(0);
			if (!rotating) {
				touchedObjRotation = transform.localRotation.eulerAngles;
				touchedPosition = new Vector3(touch.position.x, touch.position.y, 0);
			}
			rotating = true;
			float x = touch.position.x - touchedPosition.x;
			float y = touch.position.y - touchedPosition.y;
			//Vector3 touchedRotation = transform.localRotation.eulerAngles;
			Vector3 transformedRotation = touchedObjRotation;
			if(upEnabled)
				transformedRotation = ApplyTransform(touchedObjRotation, upDireciton, y * .2f, verticalRange);
			if(rightEnabled)
				transformedRotation = ApplyTransform(touchedObjRotation, rightDirection, x * .2f, horizontalRange);
			transform.localRotation = Quaternion.Euler (transformedRotation);
			//One finger touch does orbit

			//			Touch touch = Input.GetTouch(0);
			//
			//			float x = touch.deltaPosition.x * xSpeed * Time.deltaTime;
			//			float y = touch.deltaPosition.y * ySpeed * Time.deltaTime;
			//			Vector3 touchedRotation = transform.localRotation.eulerAngles;
			//			Vector3 transformedRotation = touchedRotation;
			//			if(upEnabled)
			//				transformedRotation = ApplyTransform(touchedRotation, upDireciton, y, true);
			//			if(rightEnabled)
			//				transformedRotation = ApplyTransform(transformedRotation, rightDirection, x);
			//			transform.localRotation = Quaternion.Euler (transformedRotation);
		} else {
			rotating = false;
		}
	}

	void MouseUpdate(){
		if (Input.GetMouseButton (0)) {
			if (!rotating) {
				touchedObjRotation = transform.localRotation.eulerAngles;
				touchedPosition = Input.mousePosition;
			}
			rotating = true;

			//One finger touch does orbit

			//			Touch touch = Input.GetTouch(0);
			//
			float x = Input.mousePosition.x - touchedPosition.x;
			float y = Input.mousePosition.y - touchedPosition.y;
			Vector3 transformedRotation = touchedObjRotation;
			if(upEnabled)
				transformedRotation = ApplyTransform(transformedRotation, upDireciton, y *.2f, verticalRange);
			if(rightEnabled)
				transformedRotation = ApplyTransform(transformedRotation, rightDirection, x *.2f, horizontalRange);
			transform.localRotation = Quaternion.Euler (transformedRotation);
			Logger.Log (transform.localRotation.ToString());
		} else {
			rotating = false;
		}
	}

	void Update () 
	{
		#if UNITY_EDITOR
		MouseUpdate ();
		#else
		TouchUpdate ();
		#endif
	}
}
