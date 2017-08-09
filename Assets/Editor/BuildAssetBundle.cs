using UnityEngine;
using UnityEditor;

public class CreateAssetBundles
{

	[MenuItem ("Assets/Build AssetBundles Windows")]
	static void BuildAllAssetBundlesWindows ()
	{
		BuildPipeline.BuildAssetBundles ("Assets/AssetBundlesWindows", BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
	}

	[MenuItem ("Assets/Build AssetBundles Android")]
	static void BuildAllAssetBundlesAndroid ()
	{
		BuildPipeline.BuildAssetBundles ("Assets/AssetBundlesAndroid", BuildAssetBundleOptions.None, BuildTarget.Android);
	}

	[MenuItem ("Assets/Build AssetBundles IOS")]
	static void BuildAllAssetBundlesIOS ()
	{
		BuildPipeline.BuildAssetBundles ("Assets/AssetBundlesIOS", BuildAssetBundleOptions.None, BuildTarget.iOS);
	}

}

