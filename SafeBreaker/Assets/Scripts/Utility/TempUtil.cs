using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Xml;

public class TempUtil {
	
	public static bool GetTouchDown(int count)
	{
#if UNITY_EDITOR
		if(Input.GetMouseButtonDown(count))
#else
		if(Input.touchCount > count && Input.GetTouch(count).phase == TouchPhase.Began)
#endif
		{
			return true;
		}
		
		return false;
	}
	
	public static bool GetTouch(int count)
	{
#if UNITY_EDITOR
		if(Input.GetMouseButton(count))
#else
		if(Input.touchCount > count && (Input.GetTouch(count).phase == TouchPhase.Stationary || Input.GetTouch(count).phase == TouchPhase.Moved))
#endif
		{
			return true;
		}
		
		return false;
	}
	
	public static bool GetTouchUp(int count)
	{
#if UNITY_EDITOR
		if(Input.GetMouseButtonUp(count))
#else
		if(Input.touchCount > count && Input.GetTouch(count).phase == TouchPhase.Ended)
#endif
		{
			return true;
		}
		
		return false;
	}
	
	public static void SetLayerWithChildren(GameObject obj, int layerMask)
	{
		obj.layer = layerMask;
		Transform[] childrenList = obj.GetComponentsInChildren<Transform>();
		for (int i = 0; i < childrenList.Length; i++) {
		{
			//Debug.Log("layermask : " + layerMask);
			childrenList[i].gameObject.layer = layerMask;	
		}
			
		}
	}

    /// <summary>
    /// 자식 객체를 찾아준다.
    /// </summary>
    public static Transform FindChild(GameObject go, string childName)
    {
        Transform[] objs = go.GetComponentsInChildren<Transform>(true);
        foreach (Transform trans in objs)
        {
            if (trans.name == childName)
                return trans;
        }
        return null;
    }
	
	public static void FocusLog(object obj)
	{
		Debug.Log("///////////////////////////////////////////" + obj.ToString());
	}
	
	public static GameObject GetParent(GameObject _obj)
	{
		return GetParent(_obj, 30);
	}
	
	public static GameObject GetParent(GameObject _obj, int _depth)
	{
		GameObject tempObj = _obj;
		int depth = _depth;
		while(true)
		{
			if(tempObj.transform.parent != null)
			{
				tempObj = tempObj.transform.parent.gameObject;
			}
			else
				return tempObj;
			
			depth--;
			if(depth < 0)
			{
				Debug.Log("Depth over");
				return null;
			}
		}
		
        // Fix me!
		//return tempObj;
	}

    public static void StartCoroutine(IEnumerator routine)
    {
        UtilManager.instance.StartCoroutine(routine);
    }
	
	public static void StopCoroutine(string routinename)
    {
        UtilManager.instance.StopCoroutine(routinename);
    }
	
	public static void StopAllCoroutine()
    {
        UtilManager.instance.StopAllCoroutines();
    }
	
	public static void StartCoroutine(System.Action action, float time)
	{
        UtilManager.instance.StartCoroutine(DelayCall(action, time));
	}
	
	public static IEnumerator DelayCall(System.Action action, float time)
	{
        if (time!=0)
		    yield return new WaitForSeconds(time);
		
		if(action != null)
			action();
	}
	
	public static string GetTypeString<T>()
	{
		return typeof(T).ToString();
	}
	
	public static void LoadLevel<T>()
	{
		Application.LoadLevel(GetTypeString<T>());
	}
	
	public static AsyncOperation LoadLevelAsync<T>()
	{
		return Application.LoadLevelAsync(GetTypeString<T>());
	}
	
	public static Type GetType(string name)
	{
		return Types.GetType(name, "Assembly-CSharp");
	}

}

public class Triple<T1, T2, T3>
{
	public T1 first;
	public T2 second;
	public T3 third;
	
	public Triple(T1 _first, T2 _second, T3 _third)
	{
		first = _first;
		second = _second;
		third = _third;
	}
}
