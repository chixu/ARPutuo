using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class OKCancelPanel : MonoBehaviour
{
	public string txtDesc;
	public string txtOK ="ok";
	public string txtCancel = "cancel";
	public Text description;
	public Action onCancelHandler;
	public Action onOKHandler;
	public Text okText;
	public Text cancelText;
	public bool isOK;
	public bool isCancel;
	public bool autoTranslate = true;
	//public int curretAssets;

	public void Start ()
	{
		if (autoTranslate) {
			okText.text = I18n.Translate (txtOK);
			cancelText.text = I18n.Translate (txtCancel);
		}
	}

	public void Reset(){
		isOK = false;
		isCancel = false;
	}

	public IEnumerator ShowOK (string desc = "")
	{
		isOK = false;
		isCancel = false;
		if(!string.IsNullOrEmpty(desc))
			description.text = desc;
		gameObject.GetChildByNameInChildren ("Cancel").SetActive (false);
		RectTransform okTrans = gameObject.GetChildByNameInChildren ("OK").GetComponent<RectTransform> ();
		okTrans.localPosition = okTrans.localPosition.SetX (0);
		this.gameObject.SetActive (true);

		//okCancelPanel.Show (I18n.Translate("not_in_wifi"));
		while (!isOK && !isCancel) {
			yield return null;
		}
	}

	public IEnumerator Show (string desc = "")
	{
		isOK = false;
		isCancel = false;
		if(!string.IsNullOrEmpty(desc))
			description.text = desc;
		this.gameObject.SetActive (true);

		//okCancelPanel.Show (I18n.Translate("not_in_wifi"));
		while (!isOK && !isCancel) {
			yield return null;
		}
//		if (isOK) {
//			Logger.Log ("~~isOK~~", "purple");
//		}
//		if (isCancel) {
//			Logger.Log ("~~isCancel~~", "purple");
//
//		}
	}


	public void Show (string desc, string oktxt, string canceltxt)
	{
		description.text = desc;
		okText.text = oktxt;
		cancelText.text = canceltxt;
		this.gameObject.SetActive (true);
	}

	public void Hide ()
	{
		this.gameObject.SetActive (false);
	}

	public void OnOKClick ()
	{
		isOK = true;
		if (onOKHandler != null)
			onOKHandler.Invoke ();
		this.Hide ();
	}

	public void OnCancelClick ()
	{
		isCancel = true;
		if (onCancelHandler != null)
			onCancelHandler.Invoke ();
		this.Hide ();
	}
}
