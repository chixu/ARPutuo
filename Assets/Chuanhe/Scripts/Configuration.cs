using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;
using System;
using System.Xml.Linq;

public enum Environment{
	Production, Development
}

public class Configuration : MonoBehaviour {
	public static Configuration instant;
	public string serverVersion;
	public string version;
	[Header("Production")]
	public string prodRemoteUrl = "http://www.iyoovr.com/hsyx";
	[Header("Development")]
	public string devRemoteUrl = "http://www.iyoovr.com/hsyx";

	public string language = Language.Chinese;
	public bool enablePopupVideo = true;
	public Text message;
	public Environment environment;
	public Color mainColor;
	public Color uiGrey;

	public Image bg;
	private bool loaded = false;
	private bool timeToLoad = true;
	private string configStr;
	private bool versionLoaded = false;
	private bool loading = true;

	// Use this for initialization
	//	void Start () {
	//		StartCoroutine (NextSceneAfterSeconds (5));
	//	}

	void Awake(){
		Director.version = new Version (version);
		Director.environment = environment;
		Configuration.instant = this;
		if (environment == Environment.Development) {
			Request.RemoteUrl = devRemoteUrl;
		} else {
			Request.RemoteUrl = prodRemoteUrl;
		}
		if (!string.IsNullOrEmpty(serverVersion))
			Request.RemoteUrl += "/" + serverVersion;
	}

	// Use this for initialization
	void Start () {
		StartCoroutine (readConfig ());
		message.text = "";
	}

	IEnumerator readConfig ()
	{ 
		yield return Request.ReadRemote ("version.xml", (str)=>{
			Debug.Log(str);
			XElement verXml = XDocument.Parse(str).Root;
			Version v = new Version(Xml.Attribute(verXml, "version"));
			if(v.GreaterThan(Director.version)){
				InitVersionPanel(Xml.Attribute(verXml, "forceupdate")=="true",
				Application.platform == RuntimePlatform.IPhonePlayer?Xml.Attribute(verXml, "ios"):Xml.Attribute(verXml, "android"));
			}else{
				versionLoaded = true;
			}
		}, ()=>	versionLoaded = true);
		while (!versionLoaded)
			yield return null;
		ConfigLoader loader = new ConfigLoader ();
		//StartCoroutine (PrepareForStart (2));
		yield return loader.LoadConfig ("ui/config.xml");
		yield return Request.ReadPersistent ("ui/config.xml", str=>configStr = str);
		if (!String.IsNullOrEmpty (configStr)) {
			yield return I18n.Initialise (language);
			Director.Initialize (XDocument.Parse(configStr).Root);
			yield return Director.Load ();
			Director.style.mainColor = mainColor;
			Director.style.uiGrey = uiGrey;
			OnLoaded();
		} else {
			loading = false;
			if (I18n.language == Language.Chinese) {
				message.text = "初始化失败，请检查网络连接";
			}else
				message.text = "Failed to initialise, please check your Internet Connection";
		}
	}

	void Update(){
		if (loading) {
			int numDot = Mathf.FloorToInt (Time.frameCount / 5) % 3 + 1;
			message.text = "正在加载资源";
			for (int i = 0; i < numDot; i++)
				message.text += ".";
		}
	}

	public void NextScene(){
		if (loaded && timeToLoad)
			SceneManager.LoadScene ("Selection");
	}

	public void OnClick(){
		if (loaded)
			SceneManager.LoadScene ("Selection");
	}

	public IEnumerator PrepareForStart(int second){
		yield return new WaitForSeconds (second);
		timeToLoad = true;
		NextScene ();
	}

	public void OnLoaded(){
		loading = false;
		loaded = true;
		OnClick ();
		//WWW www = new WWW (Request.Read ());
		//StartCoroutine(LoadBg());
		//NextScene ();
	}

	public void InitVersionPanel(bool forceUpdate, string url){
		OKCancelPanel panel = Director.LoadPrefab ("OKCancelPanel", GameObject.Find("Canvas")).GetComponent<OKCancelPanel>();
		panel.autoTranslate = false;
		//panel.gameObject.SetActive (false);
		if (forceUpdate) {
			panel.okText.text = "更新";
			panel.cancelText.text = "关闭";
			panel.onCancelHandler = () => {
				Application.Quit ();
			};
		} else {
			panel.okText.text = "更新";
			panel.cancelText.text = "取消";
			panel.onCancelHandler = () => {
				panel.gameObject.SetActive (false);
				versionLoaded = true;
			};
		}
		panel.description.text = "发现新版本,是否进行更新";
		panel.onOKHandler = () => {
			Application.OpenURL (url);
		};
	}

	IEnumerator LoadBg(){
		WWW www = new WWW (Request.GetPersistentPath("/ui/bg.jpg"));
		yield return www;
		if (string.IsNullOrEmpty (www.error)) {
			Texture2D tex = www.texture;
			bg.sprite = Sprite.Create (tex, new Rect (0.0f, 0.0f, tex.width, tex.height), new Vector2 (0.5f, 0.5f));
		}
	}
}
