using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchRotate : MonoBehaviour {

	//float x;
	//float y;

	float xSpeed = 20.0f;
	float ySpeed = 10.0f;

	//float pinchSpeed;
	//float distance = 10f;

	//float minimumDistance = 5f;
	//float maximumDistance = 100f;

	//Touch touch;

	float lastDist = 0f;

	float curDist = 0f;

	Camera gameCamera;

	public float maximumXAngle = 0;
	public float maximumYAngle = 0;
	//public bool isLocal = true;

	public float maximumScale = 3.0f;
	public float minimumScale = 0.5f;

	private float accumulateScale = 1.0f;

	//private float accumulateXAngle = 0;
	//private float accumulateYAngle = 0;

	private Quaternion originalRotation;
	private Vector3 originalScale;
	private Vector3 originalPosition;

	public bool scalable = true;

	public string upDireciton = "x";
	public string rightDirection = "-z";
	public bool upEnabled = true;
	public bool rightEnabled = true;

	void Awake(){
	
		originalRotation = this.transform.localRotation;
		originalScale = this.transform.localScale;
		originalPosition = this.transform.localPosition;

	}

	void OnEnable(){

		this.transform.localRotation = originalRotation;
		this.transform.localPosition = originalPosition;
		this.transform.localScale = originalScale;

		ResetParameters ();
	}

	void ResetParameters(){
	
		accumulateScale = 1.0f;
		//accumulateXAngle = 0f;
		//accumulateYAngle = 0f;
		lastDist = 0;
	}


	void ScaleUpdate(){
		if (Input.touchCount <= 1) {
			lastDist = 0;
		}

		if (scalable && Input.touchCount > 1 && (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved)) 
		{
			var touch1 = Input.GetTouch(0);

			var touch2 = Input.GetTouch(1);

			curDist = Vector2.Distance(touch1.position, touch2.position);
			if (lastDist != 0) {

				float distDiff = curDist - lastDist;
				float distDiffRatio = curDist / lastDist;

				if(curDist > lastDist)
				{
					//distance += Vector2.Distance(touch1.deltaPosition, touch2.deltaPosition)*pinchSpeed/10;

				}else{

					//distance -= Vector2.Distance(touch1.deltaPosition, touch2.deltaPosition)*pinchSpeed/10;
				}

				accumulateScale *= distDiffRatio;

				if (accumulateScale >= maximumScale) {
					accumulateScale = maximumScale;
				} else if (accumulateScale <= minimumScale) {
					accumulateScale = minimumScale;
				}

				this.transform.localScale = this.originalScale * accumulateScale;

			}
			lastDist = curDist;
		}
	}


	Vector3 ApplyTransform(Vector3 v, string dir, float value, bool clamp = false){
		string axis = dir.Substring (dir.Length - 1);
		value = dir.Length == 1 ? value : -value;
		if (axis == "x") {
			float x = Angle.ToStardard (v.x) + value;
			return v.SetX (clamp? Mathf.Clamp(x, -80, 80): x);
		} else if (axis == "y") {
			float y = Angle.ToStardard (v.y) + value;
			return v.SetY (clamp? Mathf.Clamp(y, -80, 80): y);
		} else {
			float z = Angle.ToStardard (v.z) + value;
			return v.SetZ (clamp? Mathf.Clamp(z, -80, 80): z);
		}
	}

	void Update () 
	{
		
		if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved) 
		{

			//One finger touch does orbit

			Touch touch = Input.GetTouch(0);

			float x = touch.deltaPosition.x * xSpeed * Time.deltaTime;
			float y = touch.deltaPosition.y * ySpeed * Time.deltaTime;
			Vector3 touchedRotation = transform.localRotation.eulerAngles;
			Vector3 transformedRotation = touchedRotation;
			if(upEnabled)
				transformedRotation = ApplyTransform(touchedRotation, upDireciton, y, true);
			if(rightEnabled)
				transformedRotation = ApplyTransform(transformedRotation, rightDirection, x);
			transform.localRotation = Quaternion.Euler (transformedRotation);
//			if (maximumXAngle > 0) {
//				accumulateXAngle += x;
//				if (accumulateXAngle <= -maximumXAngle) {
//					accumulateXAngle = -maximumXAngle;
//					x = 0;
//				} else if (accumulateXAngle >= maximumXAngle) {
//					accumulateXAngle = maximumXAngle;
//					x = 0;
//				}
//			}
//
//			if (maximumYAngle > 0) {
//				accumulateYAngle += y;
//				if (accumulateYAngle <= -maximumYAngle) {
//					accumulateYAngle = -maximumYAngle;
//					y = 0;
//				} else if (accumulateYAngle >= maximumYAngle) {
//					accumulateYAngle = maximumYAngle;
//					y = 0;
//				}
//			}
//
//			if (isLocal) {
//				Vector3 rotateAngle = new Vector3(-y,x,0);
//
//				transform.Rotate (new Vector3(0,x,0), Space.Self);
//				transform.Rotate (new Vector3(y,0,0), Space.World);
//
//
//			} else {
//				Vector3 rotateAngle = new Vector3(y,x,0);
//				transform.Rotate (rotateAngle, Space.World);
//			}


		}
		ScaleUpdate ();

	}
}
