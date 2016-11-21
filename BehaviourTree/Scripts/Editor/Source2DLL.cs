using UnityEngine;
using System.Collections;
using UnityEditor;
using wuxingogo.tools;
using wuxingogo.btFsm;

internal class Source2DLL : XBaseEditor {

	[MenuItem("Wuxingogo/BTEditor/Transition Source to DLL")]
	public static void Transition()
	{
		var totalState = AssetsUtilites.FindAssetsByType<BTState>();
		for (int i = 0; i < totalState.Length; i++) {
			XLogger.Log(totalState[i].name);
		}
	}
}
