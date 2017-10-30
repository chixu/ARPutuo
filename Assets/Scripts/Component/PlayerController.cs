using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	private Animator animator;
	public GameObject bodyBase;
	public Texture tex1;
	public Texture tex2;
	private int tick = 0;
	public bool isTalking;
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (isTalking) {
			tick++;
			bodyBase.GetComponent<SkinnedMeshRenderer> ().material.mainTexture = tick%20<10?tex2:tex1;
		}
//		if (Input.GetKeyDown ("1")) {
//			isTalking = true;
//			animator.Play ("Talking");
//		}else if(Input.GetKeyDown(KeyCode.Space)){
//			isTalking = false;
//			animator.Play ("Jump");
//
//		}
	}
}
