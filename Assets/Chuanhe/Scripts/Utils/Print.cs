using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using UnityEngine;
using System;

public class Print
{
	public static string Dictionary(IDictionary dict){
		string str = "{";
		foreach (var key in dict.Keys) {
			str += key.ToString () + ":" + dict [key].ToString () + " , ";
		}
		return str+"}";
	}
}
