using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;
using System;
using System.Xml.Linq;

public class UserBehavior {
	private XElement data;
	private string dataString;
	public static string fileName = "user.xml";

	public UserBehavior(){
		
	}

	public IEnumerator Load(){
		yield return Request.ReadPersistent (fileName, str=>dataString = str);
		if (!String.IsNullOrEmpty (dataString)) {
			data = XDocument.Parse(dataString).Root;
		} else {
			data = new XElement ("user");
		}
	}

	public string GetValue(string name){
		return Xml.Attribute(data, name);
	}

	public void SetValue(string name, string value){
		string v = GetValue (name);
		if (v == value)
			return;
		XAttribute att = data.Attribute (name);
		if (att != null)
			att.Value = value;
		else {
			XAttribute newAtt = new XAttribute (name, value);
			data.Add (newAtt);
		}
	}

	public void SetValueAndSave(string name, string value){
		SetValue (name, value);
		Save ();
	}

	private void Save(){
		File.WriteAllText (Application.persistentDataPath+"/" + fileName, data.ToString());
	}
}
