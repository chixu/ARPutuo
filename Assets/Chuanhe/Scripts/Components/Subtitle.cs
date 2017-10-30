using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml.Linq;

public class Subtitle : MonoBehaviour {

	public Text text;
	private string prevText;
	private XElement data;
	private bool playing;
	private float timing;

	public void Play(string path){
		if (string.IsNullOrEmpty (path))
			return;
		playing = true;
		prevText = text.text;
		text.text = "";
		StartCoroutine (LoadSubtitle (path));
		timing = 0;
	}

	IEnumerator LoadSubtitle(string path){
		yield return Request.Read (path, Loaded);
	}

	void Loaded(string str){
		data = XDocument.Parse (str).Root;
	}

	public void Stop(){
		if (!playing)
			return;
		text.text = prevText;
		data = null;
		playing = false;
	}

	void Update(){
		if (data == null || !playing)
			return;
		timing += Time.deltaTime;
		var nodes = data.Nodes ();
		foreach (XElement node in nodes) {
			float time = float.Parse (Xml.Attribute (node, "time"));
			if (timing > time)
				text.text = node.Value;
			//Logger.Log (time.ToString () + "  " + node.Value);
		}
	}
}
