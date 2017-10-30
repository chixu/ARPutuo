using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneContoller : MonoBehaviour {
	public Animator animator;
	public List<string> states = new List<string>();
	public int NextChangedTime;
	// Use this for initialization
	void Start () {
		RuntimeAnimatorController ac  = animator.runtimeAnimatorController;
		AnimationClip[] clips = ac.animationClips;
		for (int i = 0; i < clips.Length; i++) {
			Logger.Log (clips[i].name, "blue");
			if (clips [i].name != "Idle") {
				states.Add (clips [i].name);
			}
		}
		NextChangedTime = Time.frameCount + 300;
	}
	
	// Update is called once per frame
	void Update () {
		if (NextChangedTime <= Time.frameCount) {
			animator.Play (states[Random.Range(0, states.Count - 1)]);
			NextChangedTime = Time.frameCount + Random.Range(200, 400);
		}
	}
}
