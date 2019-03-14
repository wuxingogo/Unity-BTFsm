using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using wuxingogo.Editor;

public class BTStyle {

	public static GUISkin skin
	{
		get
		{
			if( _skin == null )
			{
				var skinPath = AssetsUtilites.FindAssetsByTags<GUISkin>("BTGUIStyle");
				_skin = skinPath[0];
			}
			return _skin;
		}
	}

	private static GUISkin _skin = null;
	[MenuItem( "Tools/BTFsm/Set GUISkin" )]
	public static void SetBTFsmSkin()
	{
		AssetDatabase.SetLabels(Selection.activeObject, new string[]{"BTGUIStyle"});
	}
	//[MenuItem( "Tools/Copy GUISkin" )]
	public static void CopySkin()
	{
		var selectSkin = SelectionUtils.GetObject<GUISkin>();
		skin.customStyles = selectSkin.customStyles;
		skin.window = selectSkin.window;
		skin.button = selectSkin.button;
		skin.label = selectSkin.label;
		EditorUtility.SetDirty( skin );
	}
	
}
