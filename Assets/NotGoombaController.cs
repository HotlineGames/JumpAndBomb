using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class NotGoombaController : MonoBehaviour, IReversable
{

	public void Reverse()
	{
		Speed  *= -1;
	}

	public float Speed;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		var transform = GetComponent<Rigidbody>();
		transform.MovePosition(transform.position + new Vector3(-Speed, 0));
	}

	
	

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Kill"))
		{
			Destroy(gameObject);
		}
	}
}

public interface IReversable
{
	void Reverse();



}