using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Timers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
	public float Speed;
	public int Life;
	public float MaxSpeed;
	public bool IsInvul;
	Vector3 _playerMovementDirection;
	
	// Use this for initialization
	void Start () {
		ChangeColor(Color.black); 
	}
	
	// Update is called once per frame
	void Update () {
		_playerMovementDirection = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
		transform.position += _playerMovementDirection * Speed * Time.deltaTime;
		
		SpeedTransform();

	}

	void SpeedTransform()
	{
		if (Speed <= MaxSpeed)
		{
			Speed += 0.1f;
		}
	}

	void ChangeLayer(string layer)
	{
		gameObject.layer = LayerMask.NameToLayer(layer);

		foreach (Transform child in gameObject.transform)
		{
			child.gameObject.layer = LayerMask.NameToLayer(layer);
		}
	}

	void ChangeColor(Color color)
	{
		gameObject.GetComponent<MeshRenderer>().sharedMaterial.color = color;
	}

	IEnumerator Invul()
	{
		Life -= 1;
		if (Life <= 0)
		{
			ChangeColor(Color.red);
			SceneManager.LoadScene(("GameOver"));
		}
		IsInvul = true;
		ChangeLayer("Invuln");

		yield return new WaitForSeconds(2.0f);
		
		ChangeLayer("Player");
		IsInvul = false;
	}

	IEnumerator Blink(float Interval)
	{
		
		for (int i = 0; i < 20; i++)
		{
			ChangeColor(Color.black);
			yield return new WaitForSeconds(Interval/2);
			ChangeColor(Color.white);
			yield return new WaitForSeconds(Interval/2);
		}
	}


	void OnCollisionEnter(Collision collision)
	{
		
		var isEnemy = collision.gameObject.CompareTag("Monster");
		

		if (isEnemy)
		{
	//		ContactPoint contact = collision.contacts[0];
	//		List<Vector3> weakPoints = new List<Vector3>(collision.gameObject.GetComponent<WeakPoints>().WeakPointList);

	//		Vector3 weakZoneStart = weakPoints[0];
	//		Vector3 weakZoneEnd = weakPoints[1];

	//		print("x" + contact.normal.x);
	//		print("y" + contact.normal.y);

	//		//print("weakZoneEnd x" + weakZoneEnd.x);
	//		//print("weakZoneEnd y" + weakZoneEnd.y);
	//		//print("weakZoneStart x" + weakZoneStart.x);
	//		//print("weakZoneStart y" + weakZoneStart.y);


	//		if (contact.normal.x >= weakZoneStart.x && contact.normal.x <= weakZoneEnd.x && contact.normal.y >= weakZoneStart.y &&
	//		    contact.normal.y <= weakZoneEnd.y)
	//		{
	//			Destroy(collision.gameObject);
	//		}
	//		else
	//		{
				StartCoroutine(Invul());
			StartCoroutine(Blink(2f/15 ));
		}
	}
	//}

}
