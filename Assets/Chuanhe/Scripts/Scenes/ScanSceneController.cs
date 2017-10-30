using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System;
using UnityEngine.Networking;
using Vuforia;
using UnityEngine.SceneManagement;
using System.Xml.Linq;
using System.Reflection;

public class ScanSceneController : MonoBehaviour
{
	//public string remoteUrl = "";
	//public string fileName = "myassets.dlc";
	//public string configName = "config.json";
	//public Text text;
	//public MediaPlayer mediaPlayer;
	//public Material videoMaterial;
	//public GameObject buttonPanel;
	public VideoPanelPlayer videoPanel;
	public Text title;
	public Text description;
	public GameObject infoPanel;
	public GameObject planePrefab;
	public static ScanSceneController instant;
	public static GameObject currentTrackableObject;
	public ScanSceneState state;
	[HideInInspector]
	public Subtitle subtitle;

	//	private Config localConfig;
	//	private Config remoteConfig;
	private Mappings mappings;
	//private string dataSetName = "trackings.xml";
	protected Dictionary<string, UnityEngine.Object> loadedAssets;
	private Dictionary<string, string> ConfigDict = new Dictionary<string, string> ();
	[HideInInspector]
	public string sceneName;
	//from prev scene
	public XElement data;
	[HideInInspector]
	public bool exited = false;
	protected XElement itemInfos;

	//info panel
	public Text infoTitle;
	public Text infoTip1;
	public Text infoTip2;
	public Text infoTip3;
	[HideInInspector]
	public GameObject scanner;
	private string type;

	[System.Serializable]
	public class Config
	{
		public string version = "";
		public string[] files;
	}


	[System.Serializable]
	public struct Mappings
	{
		[System.Serializable]
		public struct Mapping
		{
			public string key;
			public string name;
		}

		public Mapping[] mappings;
	}


	protected string GetAssetsPath (string str, bool isFile = false)
	{
		if(string.IsNullOrEmpty(str)) return "";
		return Request.ResolvePath (Application.persistentDataPath + "/" + sceneName + "/" + str, isFile);
	}

	IEnumerator StartGame ()
	{
		loadedAssets = new Dictionary<string, UnityEngine.Object> ();
		SetState ("idle");
		yield return Request.ReadPersistent (sceneName + "/iteminfos.xml", str => itemInfos = XDocument.Parse (str).Root);
		var abNodes = data.Element ("assetbundles").Nodes ();
		foreach (XElement node in abNodes) {
			AssetBundle bundle = null;
			string abName = Xml.Attribute (node, "src");
			string keyName = sceneName + "_" + abName;
			Logger.Log ("assetbundles " + abName + " " + keyName, "green");
			if (!AssetBundleManager.bundles.ContainsKey (keyName)) {
				Logger.Log (GetAssetsPath (abName, true), "purple");
				WWW www = new WWW (GetAssetsPath (abName, true));
				yield return www;
				if (string.IsNullOrEmpty(www.error)) {
					bundle = www.assetBundle;
					AssetBundleManager.bundles.Add (keyName, bundle);
				} else {
					Logger.Log (www.error, "blue");
				}
			} else {
				bundle = AssetBundleManager.bundles [keyName];
			}
			Logger.Log (bundle ? bundle.ToString () : "bundle is null", "blue");
			if (bundle != null) {
				string[] assetNames;
				try {
					assetNames = bundle.GetAllAssetNames ();
					foreach (string name in assetNames) {
						string simpleName = Path.GetFileNameWithoutExtension (name);
						//Instantiate(bundle.LoadAsset());
						Logger.Log (simpleName, "purple");
						if (loadedAssets.ContainsKey (simpleName))
							loadedAssets [simpleName] = bundle.LoadAsset (simpleName);
						else
							loadedAssets.Add (simpleName, bundle.LoadAsset (simpleName));
					}
				} catch (Exception e) {
					//Log (e.StackTrace);
				}

			}
		}
		PrintLoadedAssets ();
		StartCoroutine (LoadDataSet ());
		//infoPanel.SetActive (Director.userBehavior.GetValue ("scantip") == "0");
	}

	public void PrintLoadedAssets ()
	{
		string str = "PrintLoadedAssets";
		foreach (string s in loadedAssets.Keys) {
			str += " " + s;
		}
		Logger.Log (str, "blue");
	}

	public virtual void SetState (string name, Hashtable args = null)
	{
		if (state != null && state.name == name) {
			return;
		}
		if (state != null) {
			state.OnExit ();
			Logger.Log ("leaving scene: " + state.ToString (), "green");
		}
		Logger.Log ("scene: " + name, "green");
		state = ScanSceneState.GetState (name);
		state.scene = this;
		state.OnEnter (args);
	}

	protected virtual void Awake ()
	{
		instant = this;
		sceneName = SceneManagerExtension.GetSceneArguments () ["name"].ToString ();
		type = SceneManagerExtension.GetSceneArguments () ["type"].ToString ();
		data = SceneManagerExtension.GetSceneArguments () ["data"] as XElement;
	}

	protected virtual void Start ()
	{
		Logger.Log ("ScanSceneController start", "purple");
		scanner = GameObject.Find ("Scanner");
		subtitle = GetComponent<Subtitle> ();
		Director.trackerManager.TrackEvent(TrackerEventName.SceneEnter, new Dictionary<string, object>(){{"Name", sceneName}});
		infoTitle.text = I18n.Translate ("tiptitle");
		infoTip1.text = string.Format(I18n.Translate ("tip1"), I18n.Translate (sceneName + "_target"));
		infoTip2.text = I18n.Translate ("tip2");
		infoTip3.text = I18n.Translate ("tip3");
		StartCoroutine (StartGame ());
	}


	protected void ClearAndLoadDataSet(){
		ObjectTracker objectTracker = Vuforia.TrackerManager.Instance.GetTracker<ObjectTracker> ();
		objectTracker.DestroyAllDataSets (false);
		objectTracker.Stop ();  // stop tracker so that we can add new dataset

		var tNodes = data.Element ("trackings").Nodes ();
		foreach (XElement node in tNodes) {
			string dataSetName = Xml.Attribute (node, "src");
			DataSet dataSet = objectTracker.CreateDataSet ();
			if (dataSet.Load (GetAssetsPath (dataSetName), VuforiaUnity.StorageType.STORAGE_ABSOLUTE)) {
				if (!objectTracker.ActivateDataSet (dataSet)) {
					// Note: ImageTracker cannot have more than 100 total targets activated
					Debug.Log ("<color=yellow>Failed to Activate DataSet: " + dataSetName + "</color>");
				}
			}
		}

	}

	protected virtual IEnumerator LoadDataSet ()
	{
		ClearAndLoadDataSet ();
		//int counter = 0;
		IEnumerable<TrackableBehaviour> tbs = Vuforia.TrackerManager.Instance.GetStateManager ().GetTrackableBehaviours ();
		GameObject buttonCanvasPrefab = null, buttonPrefab = null;// = Resources.Load("Prefab/ScanButtonCanvas") as GameObject;
		foreach (TrackableBehaviour tb in tbs) {

			Logger.Log (tb.TrackableName, "purple");
			tb.gameObject.name = tb.TrackableName;

			XElement info = Xml.GetChildByAttribute (itemInfos, "id", tb.TrackableName);
			if (info == null)
				continue;
			string objType = Xml.Attribute (info, "type");
			tb.gameObject.AddComponent<DefaultTrackableEventHandler> ();
			tb.gameObject.AddComponent<CustomTrackableEventHandler> ();
			tb.gameObject.AddComponent<TurnOffBehaviour> ();
			CustomTrackableEventHandler cte = tb.gameObject.GetComponent<CustomTrackableEventHandler> ();
			cte.type = objType;
			cte.subtitlePath = GetAssetsPath (Xml.Attribute (info, "subtitle"), true);
			UnityEngine.Object asset = null;
			if (objType == "object") {
//				string prefabName = Xml.Attribute (info, "prefab");
//				asset = loadedAssets.ContainsKey (prefabName) ? loadedAssets [prefabName] : new GameObject ();
//				var buttonNodes = info.Elements ();
//				GameObject bPanel = null;
//				foreach (XElement n in buttonNodes) {
//					if (buttonCanvasPrefab == null)
//						buttonCanvasPrefab = Resources.Load ("Prefabs/ScanButtonCanvas") as GameObject;
//					if (buttonPrefab == null)
//						buttonPrefab = Resources.Load ("Prefabs/ScanButton") as GameObject;
//					if (bPanel == null) {
//						bPanel = Instantiate (buttonCanvasPrefab) as GameObject;
//						bPanel.transform.SetParent (tb.gameObject.transform, false);
//					}
//					GameObject button = Instantiate (buttonPrefab) as GameObject;
//					button.AddComponent<ToggleButtonShowHide> ();
//					button.transform.SetParent (bPanel.transform.GetChild(0), false);
//				}
				InstantiateModelObject(info, tb, out asset);
			} else if (objType == "video") {
				InstantiateVideoObject(info, tb, out asset);

				//cte.mediaPlayer = mediaPlayer;
			} else if (objType == "menu4") {
				//asset = planePrefab;
				//Renderer render = (planePrefab).GetComponent<Renderer> ();
				//render.material = videoMaterial;
				//CustomTrackableEventHandler cte = tb.gameObject.GetComponent<CustomTrackableEventHandler> ();
				//cte.videoPath = GetAssetsPath (tb.TrackableName + ".mp4");
				//cte.mediaPlayer = mediaPlayer;
				tb.gameObject.AddComponent<PopMenu> ();
				PopMenu popmenu = tb.gameObject.GetComponent<PopMenu> ();
				popmenu.menuItems = new List<PopMenuItem> ();
				//popmenu.playerMateral = videoMaterial;

				var menuNodes = info.Elements ();
				//XElement res = null;
				int index = 0;
				foreach (XElement n in menuNodes) {
					GameObject planeItem = GameObject.Instantiate (Resources.Load ("Prefabs/PlaneItem4")) as GameObject;
					PopMenuItem pmi = planeItem.GetComponent<PopMenuItem> ();
					popmenu.menuItems.Add (pmi);
					pmi.floatSpeed = 5f;
					pmi.floatAmplitude = 0.03f;
					pmi.index = index;
					pmi.menu = popmenu;
					pmi.trackableHandler = cte;
					Vector3 position = planeItem.transform.localPosition;
					planeItem.transform.SetParent (tb.gameObject.transform, false);
					if (index == 1) {
						planeItem.transform.localPosition = position.SetX (-position.x);
					} else if (index == 2) {
						planeItem.transform.localPosition = position.SetZ (-position.z);
					} else if (index == 3) {
						planeItem.transform.localPosition = new Vector3 (-position.x, position.y, -position.z);
					}
					pmi.Initiate ();
					string itemSrc = Xml.Attribute (n, "src");
					string videoPath = Xml.Attribute (n, "videosrc");
					planeItem.name = pmi.name = tb.TrackableName+"_"+Xml.Attribute (n, "id");
					pmi.subtitlePath = GetAssetsPath (Xml.Attribute (n, "subtitle"), true);
					if (!string.IsNullOrEmpty (videoPath)) {
						pmi.videoPath = GetAssetsPath (videoPath);
						planeItem.RegisterClickEvent ();
					}else {
						GameObject prefab = loadedAssets [Xml.Attribute (n, "prefab")] as GameObject;
						pmi.threeDObject = GameObject.Instantiate (prefab, prefab.transform.position, prefab.transform.rotation) as GameObject;
						pmi.threeDObject.transform.SetParent (tb.gameObject.transform, false);
						ApplyItemInfo (pmi.threeDObject, n);
					}
					WWW www = new WWW (GetAssetsPath (itemSrc, true));
					yield return www;
					Logger.Log (GetAssetsPath (itemSrc, true) + " " + www.texture.ToString ());
					pmi.material.mainTexture = www.texture;
					//Logger.Log (planeItem.transform.localPosition.x.ToString() + " " +planeItem.transform.localPosition.y+ " " + planeItem.transform.localPosition.z, "blue");
					index++;
				}
				popmenu.Hide ();
				InstantiateObject (tb, asset);
			}
			//GameObject obj = (GameObject)GameObject.Instantiate (asset);
//			if (asset != null) {
//				GameObject prefab = asset as GameObject;
//				GameObject obj = (GameObject)GameObject.Instantiate (prefab, prefab.transform.position, prefab.transform.rotation);
//
//				obj.transform.SetParent (tb.gameObject.transform, false);
//				ApplyItemInfo (obj, Xml.GetChildByAttribute (itemInfos, "id", tb.TrackableName));
//				obj.RegisterClickEvent ();
//			}
		}

		ObjectTracker objectTracker = Vuforia.TrackerManager.Instance.GetTracker<ObjectTracker> ();
		if (!objectTracker.Start ()) {
			Debug.Log ("<color=yellow>Tracker Failed to Start.</color>");
		}
	}

	protected GameObject InstantiateObject(TrackableBehaviour tb, UnityEngine.Object asset){
		if (asset != null) {
			GameObject prefab = asset as GameObject;
			GameObject obj = (GameObject)GameObject.Instantiate (prefab, prefab.transform.position, prefab.transform.rotation);
			obj.transform.SetParent (tb.gameObject.transform, false);
			ApplyItemInfo (obj, Xml.GetChildByAttribute (itemInfos, "id", tb.TrackableName));
			obj.RegisterClickEvent ();
			return obj;
		}
		return null;
	}

	void InstantiateVideoObject(XElement info, TrackableBehaviour tb, out UnityEngine.Object asset){
		asset = planePrefab;
		CustomTrackableEventHandler cte = tb.gameObject.GetComponent<CustomTrackableEventHandler> ();
		cte.videoPath = GetAssetsPath (Xml.Attribute (info, "videosrc"), true);
		InstantiateObject (tb, asset);
	}


	void InstantiateModelObject(XElement info, TrackableBehaviour tb, out UnityEngine.Object asset){
		GameObject buttonCanvasPrefab = null, buttonPrefab = null;
		string prefabName = Xml.Attribute (info, "prefab");
		asset = loadedAssets.ContainsKey (prefabName) ? loadedAssets [prefabName] : new GameObject ();
		var buttonNodes = info.Elements ();
		GameObject bPanel = null;
		List<ToggleButtonShowHide> buttons = new List<ToggleButtonShowHide> ();
		foreach (XElement n in buttonNodes) {
			if (buttonCanvasPrefab == null)
				buttonCanvasPrefab = Resources.Load ("Prefabs/ScanButtonCanvas") as GameObject;
			if (buttonPrefab == null)
				buttonPrefab = Resources.Load ("Prefabs/ScanButton") as GameObject;
			if (bPanel == null) {
				bPanel = Instantiate (buttonCanvasPrefab) as GameObject;
				bPanel.transform.SetParent (tb.gameObject.transform, false);
			}
			GameObject button = Instantiate (buttonPrefab) as GameObject;
			button.AddComponent<ToggleButtonShowHide> ();
			ToggleButtonShowHide tgsh = button.GetComponent<ToggleButtonShowHide> ();
			tgsh.label = Xml.Attribute (n, "label");
			tgsh.defaultShowing = Xml.Attribute (n, "show") == "false";
			//tgsh.target = asset as GameObject;
			tgsh.targetName = Xml.Attribute (n, "id");
			button.transform.SetParent (bPanel.transform.GetChild(0), false);
			buttons.Add (tgsh);
		}
		CustomTrackableEventHandler cte = tb.gameObject.GetComponent<CustomTrackableEventHandler> ();
		GameObject obj = InstantiateObject (tb, asset);
		obj.AddComponent<TouchRotate> ();
		TouchRotate touchrotate = obj.GetComponent<TouchRotate> ();
		touchrotate.scalable = Xml.Boolean (info, "scalable", true);
		touchrotate.upEnabled = Xml.Boolean (info, "upEnabled", true); 
		touchrotate.upDireciton = Xml.Attribute (info, "upDirection", "x"); 
		touchrotate.rightEnabled = Xml.Boolean (info, "rightEnabled", true); 
		touchrotate.rightDirection = Xml.Attribute (info, "rightDirection", "-z"); 
		ITrackableController[] controllers = obj.GetComponents<ITrackableController> ();
		cte.controllers.AddRange (controllers);
		for (int i = 0; i < buttons.Count; i++) {
			buttons [i].target = obj;
			cte.controllers.Add (buttons [i]);
		}
	}

	void ApplyItemInfo (GameObject obj, XElement info)
	{
//		if (itemInfos == null)
//			return;
//		XElement info = Xml.GetChildByAttribute (itemInfos, "id", name);
		if (info == null)
			return;
		Vector3 scale = obj.transform.localScale;
		Vector3 pos = obj.transform.localPosition;
		obj.transform.localScale = new Vector3 (scale.x * Xml.Float (info, "scalex", 1), scale.y * Xml.Float (info, "scaley", 1), scale.z * Xml.Float (info, "scalez", 1));
		obj.transform.localPosition = new Vector3 (pos.x + Xml.Float (info, "x"), pos.y + Xml.Float (info, "y"), pos.z + Xml.Float (info, "z"));
	}


	public void ShowDescription(bool show = true){
		description.gameObject.SetActive (show);
		VideoController.instant.videoSlider.gameObject.SetActive (!show);
	}

	public void ShowVideoSlide(){
		ShowDescription (false);
	}

//	public void OnInfoNotShowAgainClick ()
//	{
//		Director.userBehavior.SetValueAndSave ("scantip", "0");
//	}
	//	void Log (string str)
	//	{
	//		if (!String.IsNullOrEmpty (str))
	//			text.text += "\n" + str;
	//	}
	void Update(){
		if(state!=null)
		state.Update ();
	}
}
