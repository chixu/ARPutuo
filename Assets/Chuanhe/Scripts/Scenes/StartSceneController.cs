//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.SceneManagement;
//using UnityEngine.UI;
//
//public class StartSceneController : MonoBehaviour {
//	public Image bg;
//	public bool loaded = false;
//	private bool timeToLoad = false;
//	// Use this for initialization
////	void Start () {
////		StartCoroutine (NextSceneAfterSeconds (5));
////	}
//
//	public void NextScene(){
//		if (loaded && timeToLoad)
//			SceneManager.LoadScene ("Selection");
//	}
//
//	public void OnClick(){
//		if (loaded)
//			SceneManager.LoadScene ("Selection");
//	}
//	
//	public IEnumerator NextSceneAfterSeconds(int second){
//		yield return new WaitForSeconds (second);
//		timeToLoad = true;
//		NextScene ();
//	}
//
//	public void OnLoaded(){
//		loaded = true;
//
//		//WWW www = new WWW (Request.Read ());
//		//StartCoroutine(LoadBg());
//		NextScene ();
//	}
//
//	public void InitVersionPanel(bool forceUpdate, string url){
//		OKCancelPanel panel = Director.LoadPrefab ("OKCancelPanel", GameObject.Find("Canvas")).GetComponent<OKCancelPanel>();
//		panel.autoTranslate = false;
//		//panel.gameObject.SetActive (false);
//		if (forceUpdate) {
//			panel.okText.text = "更新";
//			panel.cancelText.text = "关闭";
//			panel.onCancelHandler = () => {
//				Application.Quit ();
//			};
//		} else {
//			panel.okText.text = "更新";
//			panel.cancelText.text = "取消";
//			panel.onCancelHandler = () => {
//				panel.gameObject.SetActive (false);
//			};
//		}
//		panel.onOKHandler = () => {
//			Application.OpenURL (url);
//		};
//	}
//
//	IEnumerator LoadBg(){
//		WWW www = new WWW (Request.GetPersistentPath("/ui/bg.jpg"));
//		yield return www;
//		if (string.IsNullOrEmpty (www.error)) {
//			Texture2D tex = www.texture;
//			bg.sprite = Sprite.Create (tex, new Rect (0.0f, 0.0f, tex.width, tex.height), new Vector2 (0.5f, 0.5f));
//		}
//	}
//
//}
