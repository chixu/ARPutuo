using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine;

public class Logger
{
	public static Text text;

	public static void Log (string str, string color = "yellow"){
		if (text) {
			text.text += str+"\n";
		} else
			Debug.Log ("<color=" + color+ ">" + str + "</color>");
			//Debug.LogError ("***<color=yellow>" + str + "</color>");
	}
}
