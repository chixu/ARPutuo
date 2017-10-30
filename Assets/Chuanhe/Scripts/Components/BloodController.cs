using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;

public class BloodController : MonoBehaviour, ITrackableController {
	public float pathRandomRange = 0.9f;
	public GameObject cell;
	public float cellScale;
	public List<GameObject> paths;
	public List<GameObject> path1s;
	public List<GameObject> path2s;
	public GameObject cellsContainer;
	public GameObject rift;
	public int spawnInWave;
	public float spwanInterval;
	//public float idleTimer = 4f;
	//private List<GameObject> cells = new List<GameObject>();
	public float cellSpeed  = 0.2f;
	//private List<List<Vector3>> destinations = new List<List<Vector3>> ();
	//public static SceneController instant;
	private List<BloodAnimation> cells =  new List<BloodAnimation>();
	private bool spawning = false;


	// Use this for initialization
	void Start () {
		//SpawnOne ();
		//StartCoroutine(Spawn());
	}


	IEnumerator Spawn(){
		yield return new WaitForSeconds (spwanInterval);
		if (spawning) {
			for (int i = 0; i < spawnInWave; i++) {
				SpawnOne ();
			}
			yield return Spawn ();
		} else
			yield return null;
	}

	void SpawnOne(){
		cells.Add (new BloodAnimation (this));
	}

	public Vector3 GetRandomPoint(GameObject obj){
		Vector3 v = obj.transform.localPosition;
		float scale = obj.transform.localScale.x * pathRandomRange / 2;
		return new Vector3(v.x + Random.Range(-scale, scale), v.y+ Random.Range(-scale, scale), v.z+ Random.Range(-scale, scale) );
	}

	// Update is called once per frame
	void Update () {
		for (int i = cells.Count - 1; i >= 0; i--) {
			if (cells [i].Update ()) {
				cells.RemoveAt (i);
				i++;
			}
		}
	}

	virtual public void OnTrackingFound ()
	{
		Animator animator = GetComponentInChildren<Animator> ();
		Logger.Log ("animator " + (animator == null), "yellow");
		if (animator != null)
			animator.Play ("idle");
		spawning = true;
		StartCoroutine(Spawn());
	}


	virtual public void OnTrackingLost ()
	{
		spawning = false;
		for (int i = cells.Count - 1; i >= 0; i--) {
			cells [i].Destroy ();
		}
		cells = new List<BloodAnimation>();
	}
}


public class BloodAnimation{

	public List<Vector3> path;
	public GameObject cell;
	public float speed;
	public bool splash = false;
	public bool splashing = false;
	public int life = 100;
	public int tick = 0;
	private Vector3 force;
	private float gravity = -0.1f;
	private float speedY = 0f;
	private BloodController controller;

	public BloodAnimation(BloodController control){
		controller = control;
//		if(controller.rifting)
//			splash = Random.value > 0.5;
		speed = controller.cellSpeed * Random.Range (0.8f, 1f);
		GameObject c = GameObject.Instantiate (controller.cell);
		c.transform.localPosition = controller.GetRandomPoint (controller.paths [0]);
		c.transform.localScale = Vector3.one * controller.cellScale;
		//cells.Add (c);
		path = new List<Vector3> ();
		for (int i = 1; i < controller.paths.Count; i++) {
			path.Add(controller.GetRandomPoint(controller.paths[i]));
		}
		cell = c;
		cell.transform.SetParent (controller.cellsContainer.transform, false);

		if (controller.path1s != null && controller.path1s.Count != null) {
			float rand = Random.value;
			List<GameObject> nextPath = rand > 0.5 ? controller.path1s : controller.path2s;
			for (int i = 0; i < nextPath.Count; i++) {
				path.Add (controller.GetRandomPoint (nextPath [i]));
			}
		}

//		if (splash) {
//			force =  (controller.rift.transform.forward +  new Vector3(Random.value, Random.value * 3, Random.value )* .8f) * speed;
//			path.RemoveRange (2, path.Count - 2);
//			path.Add (controller.GetRandomPoint(controller.rift));
//		}
		//destinations.Add (path);
	}

	public bool SplashUpdate(){
		tick++;
		force *= 0.99f;
		speedY = (speedY+gravity) * .9f;
		cell.transform.localPosition += (force + new Vector3(0, speedY, 0))* Time.deltaTime;

		if (tick > life) {
			GameObject.Destroy (cell);
			return true;
		}else
			return false;
	}

	public void Destroy(){
		GameObject.Destroy (cell);
	}


	public bool Update(){
		if (splashing)
			return SplashUpdate ();

		Vector3 dest = path [0];
		cell.transform.localPosition += (dest - cell.transform.localPosition).normalized * speed * Time.deltaTime;
		cell.transform.Rotate (new Vector3 (Random.value, Random.value, Random.value) * 3);
		if ((cell.transform.localPosition - dest).magnitude < speed * 0.01f) {
			path.RemoveAt (0);
			if (path.Count == 0) {
				if (splash) {
					splashing = true;
				} else {
					GameObject.Destroy (cell);
					return true;
				}
			}
		}
		return false;
	}


}