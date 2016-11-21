using UnityEngine;
//using UnityEditor;
using System.Collections;
#if !UNITY_ANDROID
using UnityEngine.iOS;
#endif
using System.Collections.Generic;
public class GameObjectUtil {
	public static Transform FindByName(Transform root,string name)
	{
		var result = root.Find( name );
		if(result == null)
		{
			for( int i = 0; i < root.childCount; i++ ) {
				//  TODO loop in root.childCount
				result = FindByName( root.GetChild( i ), name );
				if(result != null)
				{
					return result;
				}
			}
		}
		return result;
	}

	public static void FindByNameAll(Transform root,string name,ref List<Transform> list)
	{
		for( int i = 0; i < root.childCount; ++i )
		{
			Transform t = root.GetChild(i);
			if( t.gameObject.name == name)
				list.Add(t);
		}
		for( int i = 0; i < root.childCount; ++i )
		{
			Transform t = root.GetChild(i);
			FindByNameAll(t,name,ref list);
		}
	}

//	[MenuItem ("GameObjectUtil/Add Empty GameObject")]
//	public static void AddGameObject()
//	{
//		if(Selection.activeGameObject == null )
//		{
//			GameObject obj = new GameObject("GameObject");
//			obj.transform.position = Vector3.zero;
//			obj.transform.rotation = Quaternion.identity;
//			obj.transform.localScale = Vector3.one;
//		}
//		else
//		{
//			GameObject obj = new GameObject("GameObject");
//			obj.transform.parent = Selection.activeGameObject.transform;
//			obj.transform.localPosition = Vector3.zero;
//			obj.transform.localRotation = Quaternion.identity;
//			obj.transform.localScale = Vector3.one;
//		}
//	}

	public static GameObject CreatePrefab(Transform parent,GameObject prefab)
	{
		if( prefab == null )
			return null;
		GameObject go = Object.Instantiate<GameObject>(prefab);
		go.transform.parent = parent;
		go.transform.localPosition = Vector3.zero;
		go.transform.localRotation = Quaternion.identity;
		go.transform.localScale = Vector3.one;
		if( parent !=null)
			go.layer = parent.gameObject.layer;
		return go;
	}
	public static GameObject CreatePrefab(Vector3 pos,Vector3 eulrAngles,string prefabName)
	{
		GameObject result = CreatePrefab(null,prefabName);
		if( result == null )
			return result;
		result.transform.position = pos;
		result.transform.eulerAngles = eulrAngles;
		result.transform.localScale = Vector3.one;
		return result;
	}
	public static bool IsWideScreen
	{
		get
		{
			float screenRito = (float)Screen.width / (float)Screen.height;
			if( screenRito <= 1.4 )
			{
				return false;
			}
			else
			{
				return true;
			}
		}
	}

	public static bool isPoorDevice()
	{
		#if UNITY_IPHONE
		switch(UnityEngine.iOS.Device.generation)
		{
		case UnityEngine.iOS.DeviceGeneration.iPad1Gen:
		case UnityEngine.iOS.DeviceGeneration.iPad2Gen:
		case UnityEngine.iOS.DeviceGeneration.iPad3Gen:
		case UnityEngine.iOS.DeviceGeneration.iPadMini1Gen:
		case UnityEngine.iOS.DeviceGeneration.iPadMini2Gen:
		case UnityEngine.iOS.DeviceGeneration.iPhone4:
		case UnityEngine.iOS.DeviceGeneration.iPhone4S:
		case UnityEngine.iOS.DeviceGeneration.iPodTouch1Gen: 
		case UnityEngine.iOS.DeviceGeneration.iPodTouch2Gen:
		case UnityEngine.iOS.DeviceGeneration.iPodTouch3Gen:
		case UnityEngine.iOS.DeviceGeneration.iPodTouch4Gen:
		case UnityEngine.iOS.DeviceGeneration.iPodTouch5Gen:
			return true;
		}
		#endif
		return false;

	}

	public static GameObject CreatePrefab( Transform parent,string prefabName)
	{
		Object o = Resources.Load(prefabName);
		if( o == null )
			return null;
		GameObject go = (GameObject)GameObject.Instantiate(o);
		go.transform.parent = parent;
		go.transform.localPosition = Vector3.zero;
		go.transform.localRotation = Quaternion.identity;
		go.transform.localScale = Vector3.one;
		if( parent !=null)
			go.layer = parent.gameObject.layer;
		return go;
	}
	public static void DestoryAllChildren(Transform root)
	{
		for( int i = 0; i < root.childCount; ++i )
		{
			Transform t = root.GetChild(i);
			Object.Destroy(t.gameObject);
		}
	}
	public static void DestoryChild(Transform root,string childName)
	{
		Transform childGo = GameObjectUtil.FindByName(root,childName);
		if( childGo != null)
			Object.Destroy(childGo.gameObject);
	}
	public static void ChildrenAction(Transform root,System.Action<GameObject> action,bool isRecursion = true)
	{
		for( int i = 0; i < root.childCount; ++i )
		{
			Transform t = root.GetChild(i);
			if( isRecursion ) ChildrenAction(t,action);
			action(t.gameObject);
		}
	}

	public static string getFullPathName(GameObject obj,string oldStr)
	{
		GameObject o = obj;

		while(o.transform.parent != null )
		{
			oldStr = "/" + o.name + oldStr;
			o = o.transform.parent.gameObject;
		}
		oldStr = "/" + o.name + oldStr;
		return oldStr;
	}

    public static string getRelativePath( GameObject obj, GameObject child)
    {
        Transform o = child.transform;
        string relitivePath = child.name;
        while( o.parent != null )
        {
            if( o.parent == obj.transform )
            {
                return relitivePath;
            }
            else
            {
                relitivePath =  o.parent.name + "/" + relitivePath;
                o = o.transform.parent;
                
            }
        }
        return "";
    }
}
