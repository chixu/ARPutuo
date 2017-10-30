using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Xml.Linq;
using System.IO;
using UnityEngine.UI;

public class SelectionSceneController : MonoBehaviour
{
	public GameObject selectionItem;
	public GameObject itemsPanel;
	public ProgressPanel progressPanel;
	public OKCancelPanel okCancelPanel;
	private XElement layout;
	private List<GameObject> selectionItems;
	public Text phone;
	public Text email;
	public Text contactUs;
	public List<GameObject> tabs;
	public List<GameObject> tabButtons;
	public int activeTabIndex = -1;
	public GameObject header;

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
//		Debug.Log ("Start");
		contactUs.text = I18n.Translate("select_cooperate");
		email.text = I18n.Translate("select_email");
		phone.text = I18n.Translate("select_phone");
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
			Text txt = button.GetComponentInChildren<Text> () as Text;
			txt.text = I18n.Translate ("select_tab" + i);
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
				GameObject obj = GameObject.Instantiate (selectionItem);
				//obj.transform.r
				//obj.transform.parent = itemsPanel.gameObject.transform;
				//obj.GetComponent<RectTransform> ().localPosition = Vector3.zero;
				obj.transform.SetParent(itemsPanel.transform, false);
				RectTransform rectT = obj.GetComponent<RectTransform> ();
				rectT.localPosition = new Vector3 (rectT.localPosition.x, rectT.localPosition.y-168 * index);
				selectionItems.Add (obj);

				SelectionItem itemComp = obj.GetComponent<SelectionItem> ();
				itemComp.nextSceneName = Xml.Attribute (item, "next", "Scan");
				itemComp.name = title;
				itemComp.type = Xml.Attribute (item, "type");
				itemComp.title.text = I18n.Translate (title);
				itemComp.description.text = I18n.Translate (desc);
				itemComp.btnInfo.SetActive (false);// (!string.IsNullOrEmpty (help));
				itemComp.helpLink = Request.RemoteUrl + help;
				itemComp.SetOnClick (OnItemClick);
//				WWW www = new WWW(Path.Combine(Application.persistentDataPath, "ui/"+icon));
//				itemComp.image.sprite = Sprite.Create(www.texture, new Rect(0,0,www.texture.width, www.texture.height), new Vector2(0,0));
				StartCoroutine(LoadIcon ("ui/"+icon, itemComp.image));
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
		StartCoroutine (OnItemClickHandler (item));
	}

	IEnumerator OnItemClickHandler(SelectionItem item){
		string name = item.name;
		Logger.Log (name + " clicked");
		okCancelPanel.Reset ();
		Enabled = false;
		configLoader = new ConfigLoader ();
		//configLoader.loadedHandler = FileLoaded;
		configLoader.progressHandler = FileProgressing;
		configLoader.okCancelPanel = okCancelPanel;
		yield return configLoader.LoadConfig (name + "/config.xml");
		progressPanel.Hide ();
		Enabled = true;
		if (!configLoader.forceBreak && !okCancelPanel.isCancel) {
			Hashtable arg = new Hashtable ();
			arg.Add ("type", item.type);
			arg.Add ("name", name);
			arg.Add ("data", Xml.GetChildByAttribute(layout.Element ("items"), "title", name));
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
		Debug.Log (button.name);
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
		header.GetComponent<Image> ().enabled = idx != 1;
		header.GetComponentInChildren<Text> ().text = I18n.Translate ("select_title" + idx);
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

	public void OnEmailClick(){
		Logger.Log ("OnEmailClick");
		string[] emails = phone.text.Split (':');
		if (emails.Length > 1) {
			string email = emails [1].Trim ();
			Application.OpenURL ("mailto://" + email);
		}
	}

	public void OnPhoneClick(){
		Logger.Log ("OnPhoneClick");
		string[] phs = phone.text.Split (':');
		if (phs.Length > 1) {
			string ph = phs [1].Trim ();
			Application.OpenURL ("tel://" + ph);
		}
	}
}
