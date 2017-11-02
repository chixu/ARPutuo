using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class ProgressPanel : MonoBehaviour
{

	public ProgressBar bar;
	public Text desc;
	public Text btnText;
	//public int totalAssets;
	public Action onCancelHandler;
	public float fileSize;
	//public int curretAssets;

	public void Start ()
	{
		btnText.text = I18n.Translate ("cancel");
		bar.maxValue = 1f;
	}

	public void Show (int cur, int total, float progress)
	{
		//totalAssets = total;
		//bar.maxValue = total;
		//Load (0);
		this.gameObject.SetActive (true);
		bar.SetValue (progress);
		//desc.text = string.Format (I18n.Translate ("loading_desc"), "");
		desc.text = string.Format (I18n.Translate ("loading_desc"), fileSize.ToString ("0.00") + "M") + cur.ToString () + "/" + total.ToString ();
	}

	public void Hide ()
	{
		this.gameObject.SetActive (false);
	}

//	public void Load (int n)
//	{
//		bar.SetValue (n);
//		desc.text = string.Format (I18n.Translate ("loading_desc"), fileSize.ToString () + "M") + n.ToString () + "/" + totalAssets.ToString ();
//	}

	public void OnCancelClick ()
	{
		if (onCancelHandler != null)
			onCancelHandler.Invoke ();
	}
}
