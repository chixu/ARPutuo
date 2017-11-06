using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Xml.Linq;
using System.IO;
using UnityEngine.UI;

public class PutuoSelectionScene : MonoBehaviour
{
	public GameObject selectionItemBig;
	public GameObject selectionItemSmall;
	private GameObject itemsPanel;
	public ProgressPanel progressPanel;
	public OKCancelPanel okCancelPanel;
	public OKCancelPanel confirmPanel;
	private XElement layout;
	private List<GameObject> selectionItems;
//	public Text phone;
//	public Text email;
//	public Text contactUs;
	public GameObject canvas;
	private GameObject loadingPanel;
	[HideInInspector]
	public List<GameObject> tabs;
	[HideInInspector]
	public List<GameObject> tabButtons;
	[HideInInspector]
	public int activeTabIndex = -1;
//	public GameObject header;

	//public GameObject contentBg;
	private Camera ARCamera;
	private Camera normalCamera;
	// Use this for initialization
	private bool enabled = true;
	private ConfigLoader configLoader;


	void Start ()
	{
		ARCamera = GameObject.Find ("ARCamera").GetComponentInChildren<Camera>(true); 
		normalCamera = GameObject.Find ("Camera").GetComponent<Camera>();
		loadingPanel = canvas.GetChildByName ("LoadingPanel");
		tabs = new List<GameObject> ();
		tabButtons = new List<GameObject> ();
		for (int i = 1; i <= 3; i++) {
			tabs.Add (canvas.GetChildByNameInChildren("Tab"+i));
			tabButtons.Add (canvas.GetChildByNameInChildren("TabButton"+i));
		}
		itemsPanel = canvas.GetChildByNameInChildren ("ItemsContainer");
//		Debug.Log ("Start");
//		contactUs.text = I18n.Translate("select_cooperate");
//		email.text = I18n.Translate("select_email");
//		phone.text = I18n.Translate("select_phone");
		selectionItems = new List<GameObject> ();
		progressPanel.onCancelHandler = () => {
			configLoader.Cancel();
			progressPanel.Hide();
		};
		for (int i = 0; i < tabButtons.Count; i++) {
			GameObject button = tabButtons [i];
			Button btn = button.GetComponentInChildren<Button> () as Button;
			btn.onClick.AddListener (delegate {
				OnTabClicked(button);
			});
			//Text txt = button.GetComponentInChildren<Text> () as Text;
			//txt.text = I18n.Translate ("select_tab" + i);
		}
		OnTabClicked (tabButtons[0]);
		StartCoroutine (initScene ());
	}

	IEnumerator initScene ()
	{
		yield return Request.ReadPersistent ("ui/ui.xml", LayoutLoaded);
		if (layout != null) {
			XElement itemsEle = layout.Element ("items");
			var items = itemsEle.Elements ();
			int index = 0;
			foreach (XElement item in items) {
				string desc = Xml.Attribute (item, "desc");
				string title = Xml.Attribute (item, "title");
				string help = Xml.Attribute (item, "help");
				string icon = Xml.Attribute (item, "icon");
				string state = Xml.Attribute (item, "state", SelectionItemState.Active);
				string size = Xml.Attribute(item, "size", "full");
				GameObject obj = GameObject.Instantiate (size == "full"? selectionItemBig : selectionItemSmall);
				obj.GetChildByName("Title").GetComponent<Text>().text = I18n.Translate (title);
				if (size == "full") {
					obj.GetChildByName("Desc").GetComponent<Text>().text = I18n.Translate (desc);
				} else {

				}
				//obj.transform.r
				//obj.transform.parent = itemsPanel.gameObject.transform;
				//obj.GetComponent<RectTransform> ().localPosition = Vector3.zero;
				obj.transform.SetParent(itemsPanel.transform, false);


				RectTransform rectT = obj.GetComponent<RectTransform> ();
				if (index == 0)
					rectT.localPosition = new Vector3 (20, -150);
				else {
					GameObject prevobj = selectionItems[selectionItems.Count - 1];
					if (prevobj.GetComponent<SelectionItem> ().size == "full")
						rectT.localPosition = new Vector3 (20, prevobj.GetComponent<RectTransform> ().localPosition.y - 460);
					else {
						//prev obj is left half
						if(prevobj.GetComponent<RectTransform>().localPosition.x == 20)
							rectT.localPosition = new Vector3 (320, prevobj.GetComponent<RectTransform> ().localPosition.y, 0);
						else
							rectT.localPosition = new Vector3 (20, prevobj.GetComponent<RectTransform> ().localPosition.y - 220);
					}
				}

				selectionItems.Add (obj);

				SelectionItem itemComp = obj.AddComponent<SelectionItem> ();
				itemComp.nextSceneName = Xml.Attribute (item, "next", "Scan");
				itemComp.title = title;
				itemComp.id = Xml.Attribute (item, "id");;
				itemComp.size = size;
				itemComp.state = state;
				itemComp.type = Xml.Attribute (item, "type");
				//itemComp.title.text = I18n.Translate (title);
				//itemComp.description.text = I18n.Translate (desc);
				//itemComp.btnInfo.SetActive (false);// (!string.IsNullOrEmpty (help));
				itemComp.helpLink = Request.RemoteUrl + help;
				//itemComp.SetOnClick (OnItemClick);

				obj.RegisterUIClickEvent (()=> this.OnItemClick(itemComp));
//				WWW www = new WWW(Path.Combine(Application.persistentDataPath, "ui/"+icon));
//				itemComp.image.sprite = Sprite.Create(www.texture, new Rect(0,0,www.texture.width, www.texture.height), new Vector2(0,0));
				StartCoroutine(LoadIcon ("ui/"+icon, obj.GetChildByName("Image").GetComponent<Image>()));
				index++;
			}
		}
	}

	bool Enabled{
		get{
			return enabled;
		}
		set{
			enabled = value;
//			if (enabled) {
//				for (int i = 0; i < selectionItems.Count; i++) {
//					Logger.Log ("enbaled ", "red");
//					Button btn = selectionItems [i].GetComponent<Button> ();
//					btn.interactable = enabled;
//				}
//			}
		}
	}

	void OnItemClick(SelectionItem item){
		//item.gameObject.GetComponent<Button> ().interactable = false;

		Logger.Log (item.id + " clicked");
		if (item.state == SelectionItemState.Active) {
			StartCoroutine (OnItemClickHandler (item));
		} else {
			StartCoroutine (confirmPanel.ShowOK (I18n.Translate (item.state + "_hint" )));
		}
	}

	IEnumerator OnItemClickHandler(SelectionItem item){
		string id = item.id;
		Logger.Log (id + " clicked");
		okCancelPanel.Reset ();
		Enabled = false;
		configLoader = new ConfigLoader ();
		//configLoader.loadedHandler = FileLoaded;
		configLoader.progressHandler = FileProgressing;
		configLoader.okCancelPanel = okCancelPanel;
		yield return configLoader.LoadConfig (id + "/config.xml");
		progressPanel.Hide ();
		Enabled = true;
		if (!configLoader.forceBreak && !okCancelPanel.isCancel) {
			if (loadingPanel != null)
				loadingPanel.SetActive (true);
			Hashtable arg = new Hashtable ();
			arg.Add ("type", item.type);
			arg.Add ("name", id);
			arg.Add ("data", Xml.GetChildByAttribute(layout.Element ("items"), "id", id));
			SceneManagerExtension.LoadScene (item.nextSceneName, arg);
		}
	}

	void FileProgressing(int idx, int total, float progress){
		progressPanel.fileSize = configLoader.fileSize;
		progressPanel.Show (idx, total, progress);
	}

//	void FileLoaded(int idx, int total){
//		if (idx == 0) {
//			progressPanel.Show (total);
//			return;
//		}
//		progressPanel.fileSize = configLoader.fileSize;
//		progressPanel.Load (idx);
//		if (idx == total) {
//			progressPanel.Hide ();
//		}
//	}

	IEnumerator LoadIcon(string url, Image image){
		//Debug.Log (Path.Combine ("file:////"+ Application.persistentDataPath, url));
		WWW www = new WWW(Request.ResolvePath(Application.persistentDataPath + "/" + url));
		yield return www;
		image.sprite = Sprite.Create(www.texture, new Rect(0,0,www.texture.width, www.texture.height), new Vector2(0,0));
	}


	void SelectTabButton(int idx, bool shown = true){
		GameObject btn = tabButtons [idx];
		Text text = btn.GetChildByName ("Text").GetComponent<Text> ();
		Image icon = btn.GetChildByName ("Image").GetComponent<Image> ();
		text.color = icon.color = shown ? Director.style.mainColor : Director.style.uiGrey;
	}

	void OnTabClicked(GameObject button){
		int idx = tabButtons.IndexOf (button);
		if (idx == activeTabIndex)
			return;
		activeTabIndex = idx;
		for (int i = 0; i < tabs.Count; i++) {
			tabs [i].SetActive (false);
			SelectTabButton (i, false);
		}
		tabs [idx].SetActive (true);
		SelectTabButton (idx);
		normalCamera.gameObject.SetActive (idx != 1);
		ARCamera.gameObject.SetActive (idx == 1);
//		header.GetComponent<Image> ().enabled = idx != 1;
//		header.GetComponentInChildren<Text> ().text = I18n.Translate ("select_title" + idx);
		StatusBar.Show (idx != 1);
	}

//	void OnGUI ()
//	{
////		for (int i = 0; i < selectionItems.Count; i++) {
////			selectionItems [i].GetComponent<RectTransform> ().localPosition = Vector3.zero;
////		}
//	}

	void LayoutLoaded (string str)
	{
		layout = XDocument.Parse (str).Root;
	}
}
