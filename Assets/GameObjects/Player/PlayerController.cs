using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
	public const float DefaultJetPackFuel = 2;

	//Vector3 _playerMovementDirection;

	private Rigidbody _rigidBody;
	public bool IsInvul;
	public float JetPackFuel;
	public int Life;
	public float MaxSpeedX;
	public float MaxSpeedY;
	public float Speed;
	public float JumpForce;

	// Use this for initialization
	private void Start()
	{
		_rigidBody = GetComponent<Rigidbody>();
		ChangeColor(Color.black);
	}

	// Update is called once per frame
	private void Update()
	{
		var input = GetInput();

		var anyInput = Mathf.Abs(input.x) > float.Epsilon || Mathf.Abs(input.y) > float.Epsilon;

		var velocity = _rigidBody.velocity;
		if (anyInput)
		{
			var desiredMove = new Vector3(input.x, input.y);

			desiredMove = desiredMove * Speed;

			var isBelowMaxHorizontalSpeed = Math.Abs(velocity.x) < MaxSpeedX;

			var isAgainstCurrentDirection = desiredMove.x * velocity.x < -Mathf.Epsilon;

			if (isBelowMaxHorizontalSpeed || isAgainstCurrentDirection)
					_rigidBody.velocity = velocity + new Vector3(desiredMove.x * Speed, 0);
				

			if (desiredMove.y > 0)
				if (JetPackFuel > 0.0f)
				{
					_rigidBody.AddForce(new Vector3(0, desiredMove.y * JumpForce), ForceMode.VelocityChange);
					JetPackFuel = JetPackFuel - 0.1f;
				}
		}
		else if(Mathf.Abs(velocity.x) > Mathf.Epsilon)
		{
			var f = 0.1f;

			var antiDirection = Mathf.Sign(velocity.x) * -1;

			var newVelocity = velocity + new Vector3(f * antiDirection, 0);

			var wouldChangeDirection = velocity.x * newVelocity.x < -Mathf.Epsilon;
			if (wouldChangeDirection)
			{
				_rigidBody.velocity = new Vector3(0, velocity.y);
			}
			else
			{
				_rigidBody.velocity = newVelocity;
			}
			
		}
	}

	public void ResetFuel(float fuel = DefaultJetPackFuel)
	{
		JetPackFuel = fuel;
	}

	private static Vector2 GetInput()
	{
		var input = new Vector2
		{
			x = Math.Sign(Input.GetAxis("Horizontal")),
			y = Math.Sign(Input.GetAxis("Vertical"))
		};
		return input;
	}


	private void ChangeLayer(string layer)
	{
		gameObject.layer = LayerMask.NameToLayer(layer);

		foreach (Transform child in gameObject.transform)
			child.gameObject.layer = LayerMask.NameToLayer(layer);
	}

	private void ChangeColor(Color color)
	{
		gameObject.GetComponent<MeshRenderer>().sharedMaterial.color = color;
	}

	private IEnumerator Invul()
	{
		Life -= 1;
		if (Life <= 0)
		{
			ChangeColor(Color.red);
			SceneManager.LoadScene("GameOver");
		}
		IsInvul = true;
		ChangeLayer("Invuln");

		yield return new WaitForSeconds(2.0f);

		ChangeLayer("Player");
		IsInvul = false;
	}

	private IEnumerator Blink(float interval)
	{
		for (var i = 0; i < 20; i++)
		{
			ChangeColor(Color.black);
			yield return new WaitForSeconds(interval / 2);
			ChangeColor(Color.white);
			yield return new WaitForSeconds(interval / 2);
		}
	}


	private void OnCollisionEnter(Collision collision)
	{
		var isEnemy = collision.gameObject.CompareTag("Monster");


		if (isEnemy)
		{
			StartCoroutine(Invul());
			StartCoroutine(Blink(2f / 15));
		}
	}
}