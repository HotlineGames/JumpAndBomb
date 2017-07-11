using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Timers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
	public float Speed;
	public int Life;
	public float MaxSpeedX;
	public float MaxSpeedY;
	public bool IsInvul;
	public float JetPackFuel;
	public const float DefaultJetPackFuel = 2;

	//Vector3 _playerMovementDirection;

	private Rigidbody _rigidBody;

	// Use this for initialization
	void Start()
	{
		_rigidBody = GetComponent<Rigidbody>();
		ChangeColor(Color.black);
	}

	// Update is called once per frame
	void Update()
	{
		Vector2 input = GetInput();

		if (Mathf.Abs(input.x) > float.Epsilon || Mathf.Abs(input.y) > float.Epsilon)
		{
			// always move along the camera forward as it is the direction that it being aimed at
			/*Vector3 desiredMove = cam.transform.forward * input.y + cam.transform.right * input.x;
			desiredMove = Vector3.ProjectOnPlane(desiredMove, Vector3.up).normalized;*/

			Vector3 desiredMove = new Vector3(input.x, input.y);

			desiredMove = desiredMove * Speed;

			if (Math.Abs(_rigidBody.velocity.x) < MaxSpeedX || (desiredMove.x > 0 && _rigidBody.velocity.x < 0) || (desiredMove.x < 0 && _rigidBody.velocity.x > 0))
				_rigidBody.AddForce(new Vector3(desiredMove.x, 0), ForceMode.VelocityChange);

			if (desiredMove.y > 0)
				if (JetPackFuel > 0.0f)
				{
					_rigidBody.AddForce(new Vector3(0, desiredMove.y), ForceMode.VelocityChange);
					JetPackFuel = JetPackFuel - 0.1f;
				}
		}
	}

	public void ResetFuel(float fuel = DefaultJetPackFuel)
	{
		JetPackFuel = fuel;
	}

	private static Vector2 GetInput()
	{
		Vector2 input = new Vector2
		{
			x = Mathf.Clamp(Input.GetAxis("Horizontal"), -1 , 1),
			y = Input.GetAxis("Vertical")
		};
		return input;
	}

	private float GetAbsolute(float value)
	{
		if (value > Mathf.Epsilon)
		{
			return 1;
		}
		if(value < -Mathf.Epsilon)
		{
			return -1;
		}
		return 0;
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

	IEnumerator Blink(float interval)
	{
		for (int i = 0; i < 20; i++)
		{
			ChangeColor(Color.black);
			yield return new WaitForSeconds(interval / 2);
			ChangeColor(Color.white);
			yield return new WaitForSeconds(interval / 2);
		}
	}


	void OnCollisionEnter(Collision collision)
	{
		var isEnemy = collision.gameObject.CompareTag("Monster");


		if (isEnemy)
		{
			StartCoroutine(Invul());
			StartCoroutine(Blink(2f / 15));
		}
	}
}