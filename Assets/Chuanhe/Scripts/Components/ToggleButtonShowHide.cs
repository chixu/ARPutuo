using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButtonShowHide : MonoBehaviour, ITrackableController
{
	public string label;
	public GameObject target;
	public string targetName;
	public Text text;
	public Button button;
	public bool showing;
	public bool defaultShowing;

	private bool inited = false;
	// Use this for initialization
	void Start ()
	{
		Initialize ();
	}

	void Initialize(){
		if (!inited) {
			text = transform.GetChild (0).gameObject.GetComponent<Text> ();
			button = GetComponent<Button> ();
			button.onClick.AddListener (ShowHide);
			inited = true;
		}
	}

	void ShowHide ()
	{
		if (showing) {
			text.text = I18n.Translate ("hide") + I18n.Translate (label);
		}else
			text.text = I18n.Translate ("show") + I18n.Translate (label);
		target.GetChildByName(targetName).SetActive (showing);
		showing = !showing;
	}

	public void OnTrackingFound(){
		Initialize ();
		showing = defaultShowing;
		text.text = I18n.Translate (showing?"show":"hide") + I18n.Translate (label);
		target.GetChildByName(targetName).SetActive (!showing);
	}

	public void OnTrackingLost(){

	}
}
