using System;

public class Version {
	public int[] numbers;
	private string versionString;

	public Version(string str){
		versionString = str;
		string[] strs = str.Split ('.');
		numbers = new int[strs.Length];
		for (int i = 0; i < strs.Length; i++) {
			numbers [i] = int.Parse(strs [i]);
		}
	}

	public bool EqualTo(Version v){
		if (numbers.Length != v.numbers.Length)
			return false;
		for (int i = 0; i < numbers.Length; i++) {
			if (numbers [i] != v.numbers [i])
				return false;
		}
		return true;
	}

	public bool GreaterThan(Version v){
		int len = Math.Min (numbers.Length, v.numbers.Length);
		for (int i = 0; i < len; i++) {
			if (numbers [i] > v.numbers [i])
				return true;
			if (numbers [i] < v.numbers [i])
				return false;
		}
		return false;
	}

	public bool LessThan(Version v){
		int len = Math.Min (numbers.Length, v.numbers.Length);
		for (int i = 0; i < len; i++) {
			if (numbers [i] < v.numbers [i])
				return true;
			if (numbers [i] > v.numbers [i])
				return false;
		}
		return false;
	}


	public bool LessOrEqualThan(Version v){
		return !GreaterThan(v);
	}

	public bool GreaterOrEqualThan(Version v){
		return !LessThan (v);
	}

	public string ToString(){
		return "Version: " + versionString;
	}
}
