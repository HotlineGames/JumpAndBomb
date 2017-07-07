using UnityEngine;

public class BowserJumpController : MonoBehaviour
{
	private Vector3 _startPosition;

	const int Magnitude = 7;
	private Vector3 _direction = new Vector3(-1 * Magnitude, 1 * Magnitude, 0 * Magnitude);

	// Use this for initialization
	void Start()
	{
		var transformComponent = this.GetComponent<Transform>();
		_startPosition = transformComponent.localPosition;
	}

	// Update is called once per frame
	void Update()
	{
	}

	void Jump()
	{
		var rigidbodyComponent = GetComponent<Rigidbody>();

		rigidbodyComponent.AddForce(_direction, ForceMode.Impulse);
	}

	void OnCollisionEnter(Collision collision)
	{
		var isGroundCollision = collision.gameObject.layer == LayerMask.NameToLayer("Terrain");

		Debug.Log("Collision! " + (isGroundCollision ? "Ground" : ""));

		if (isGroundCollision)
		{
			Jump();
			ChangeDirection();
		}
	}

	private void ChangeDirection()
	{
		_direction.x = -_direction.x;
	}
}