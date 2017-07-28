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
	public bool CanJump;

	private bool IsMoving;
	private bool IsJumping;
	private bool IsStanding;
	private bool IsFalling;
	private bool HasFuel;

	public float velocity;

	private Rigidbody _rig;
	Vector3 _playerPosition;

	// Use this for initialization
	void Start () {
		ChangeColor(Color.black);
		_rig = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{

		velocity = _rig.velocity.y;
		Move();
	}

	void Move()
	{
		//_playerPosition = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
		//transform.position += _playerPosition * Speed * Time.deltaTime;
		float moveVertically = Input.GetAxis("Horizontal");
		float moveHorizontally = Input.GetAxis("Vertical");

		if(moveVertically == 0.0f)
		{
			IsStanding = true;
		}
		if ((moveHorizontally > 0f || moveVertically > 0f) && _rig.velocity.x > 0 || _rig.velocity.y > 0)
		{
			IsMoving = true;
		}
		else
		{
			IsMoving = false;
		}
		if (_rig.velocity.y > 0)
		{
			IsJumping = true;
			CanJump = false;
			StartCoroutine(FuelCountdown());
		}
		if (_rig.velocity.y < -1)
		{
			IsFalling = true;
			IsJumping = false;
			CanJump = false;
		}
		else
		{
			IsFalling = false;
		}
		if (CanJump)
		{
			HasFuel = true;
		}
		if (_rig.velocity.y == 0)
		{
			CanJump = true;
		}

		if (moveHorizontally > 0 && HasFuel)
		{
			Vector3 movement = new Vector3(0.0f, moveHorizontally * Speed, 0.0f);
			_rig.velocity = movement;
		}
		if (moveVertically > 0 || moveVertically < 0)
		{
			Vector3 movement = new Vector3(moveVertically * Speed, _rig.velocity.y, 0.0f);
			_rig.velocity = movement;
		}


		if (IsMoving)
		{
			SpeedTransform(0.1f);
		}
		else
		{
			if(Speed > 3) SpeedTransform(-0.1f);
		}
		if (IsMoving) print("isMoving:   " + IsMoving);
		if (IsFalling) print("isFalling:   " + IsFalling);
		if (IsJumping) print("isJumping:   " + IsJumping);


		//if(!(moveHorizontally > 0f) && CanJump <= 0)
		//	{
		//		moveHorizontally = 0.0f;
		//	}

		//if (moveHorizontally != 0f)
		//{
		//	Vector3 movement = new Vector3(0.0f, moveHorizontally * Speed, 0.0f);
		//	_rig.velocity = movement;
		//	CanJump--;
		//}
		//if (moveVertically != 0f)
		//{
		//	Vector3 movement = new Vector3(moveVertically * Speed, 0.0f, 0.0f);
		//	_rig.velocity = movement;
		//	SpeedTransform(0.1f);
		//}
		//else
		//{
		//	if (Speed > 2) SpeedTransform(-0.1f);
		//}
	}

	IEnumerator FuelCountdown()
	{
		yield return new WaitForSeconds(0.3f);
		HasFuel = false;
	}
	void SpeedTransform(float velocity)
	{
		if (Speed <= MaxSpeed && Speed >= 3)
		{
			Speed += velocity;
		}
		else
		{
			Speed = 3;
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
		CanJump = true;
		var isEnemy = collision.gameObject.CompareTag("Monster");
		StopCoroutine("FuelCountdown");
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
