using UnityEngine;
using System.Collections;

public class Editor : MonoBehaviour {

	public Transform wrapper;
	public Transform wrapperObjects;

	public 	void CreateJson()
	{
		JSONObject j = new JSONObject(JSONObject.Type.OBJECT);
		JSONObject arr = new JSONObject(JSONObject.Type.ARRAY);
		j.AddField("game", arr);
		
		foreach(Transform tr in wrapper)
		{
			JSONObject obj = new JSONObject(JSONObject.Type.OBJECT);
			RectTransform reTrans = tr.GetComponent<RectTransform>();
			obj.AddField("x", reTrans.anchoredPosition.x);
			obj.AddField("y", reTrans.anchoredPosition.y);
			obj.AddField("rot", reTrans.localEulerAngles.z);
			obj.AddField("type", tr.GetComponent<PuzzleCell>().typeCell.ToString());
			arr.Add(obj);
		}
		
		JSONObject arr2 = new JSONObject(JSONObject.Type.ARRAY);
		j.AddField("objects", arr2);
		foreach(Transform tr in wrapperObjects)
		{
			JSONObject obj = new JSONObject(JSONObject.Type.OBJECT);
			JSONObject arr3 = new JSONObject(JSONObject.Type.ARRAY);
			obj.AddField("obj", arr3);
			foreach(Transform trInside in tr)
			{
				JSONObject obj2 = new JSONObject(JSONObject.Type.OBJECT);
				RectTransform reTrans = trInside.GetComponent<RectTransform>();
				obj2.AddField("x", reTrans.anchoredPosition.x);
				obj2.AddField("y", reTrans.anchoredPosition.y);
				obj2.AddField("rot", reTrans.localEulerAngles.z);
				obj2.AddField("type", trInside.GetComponent<PuzzleCell>().typeCell.ToString());
				arr3.Add(obj2);
			}
			arr2.Add(obj);
		}
		System.IO.File.WriteAllText (Application.dataPath + "/Data/1.txt", j.ToString ());
		Debug.Log (j.ToString ());
	}
}
