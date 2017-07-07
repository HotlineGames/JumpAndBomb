using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakPoints : MonoBehaviour {


	public List<Vector3> WeakPointList;
	private static Vector3 endPoint = new Vector3(1, 1, 0);
	private static Vector3 startPoint = new Vector3(-1, 0.8f, 0);
	// Use this for initialization
	void Start () {
		WeakPointList.Add(startPoint);
		WeakPointList.Add(endPoint);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
