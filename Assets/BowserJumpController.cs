using UnityEngine;

[RequireComponent(typeof(Transform))]
public class BowserJumpController : MonoBehaviour
{
	private Vector3 _startPosition;

	const int Magnitude = 7;
	private Vector3 _direction = new Vector3(-1, 1, 0);

	// Use this for initialization
	void Start()
	{
		var transformComponent = GetComponent<Transform>();
		_startPosition = transformComponent.localPosition;
	}

	// Update is called once per frame
	void Update()
	{

	}

	void ResetForces()
	{
		var rigidbodyComponent = GetComponent<Rigidbody>();

		rigidbodyComponent.velocity = new Vector3(0f, 0f, 0f);
		rigidbodyComponent.angularVelocity = new Vector3(0f, 0f, 0f);
	}

	void OnCollisionEnter(Collision collision)
	{
		var isGroundCollision = collision.gameObject.layer == LayerMask.NameToLayer("Terrain");

		//Debug.Log("Collision! " + (isGroundCollision ? "Ground" : ""));

		if (isGroundCollision)
		{
			Jump();
		}
	}

	private void Jump()
	{
		var rigidbodyComponent = GetComponent<Rigidbody>();

		ResetForces();

		ChangeDirectionX();
		//ChangeDirectionY();

		rigidbodyComponent.AddForce(_direction * Magnitude, ForceMode.Impulse);
	}

	private void ChangeDirectionX()
	{
		_direction.x = -_direction.x;
	}

	private void ChangeDirectionY()
	{
		_direction.y = -_direction.y;
	}
}