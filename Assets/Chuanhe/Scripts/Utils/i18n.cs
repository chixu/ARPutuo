using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using UnityEngine;


public class Language{
	public const string Chinese = "cn";
	public const string English = "en";
}

public class I18n
{
	private static XElement data;
	public static string language = Language.Chinese;

	public static IEnumerator Initialise (string lang = "cn"){
		language = lang;
		yield return Request.ReadPersistent ("ui/i18n.xml", I18nLoaded);
	}

	private static void I18nLoaded(string str){
		data = XDocument.Parse (str).Root;
	}

	public static string Translate(string key){
		if (data == null)
			return "";
		XElement lang = data.Element (language);
//		IEnumerable<XElement> eles = from el in lang.Elements ()
//		                             where el.Attribute ("key") == key
//		                             select el;
		IEnumerable<XElement> eles = lang.Elements();
		foreach (XElement el in eles) {
			if(el.Attribute("key").Value == key)
				return el.Attribute("value").Value;
		}
		return "";
	}
}
