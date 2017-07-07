using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.JumpAndBomb
{
	public class NotGoombaController : MonoBehaviour, IReversable
	{
		private Animator anim;
		public float Speed;
		public void Reverse()
		{
			Speed  *= -1;
			transform.Rotate(new Vector3(0,1,0), 180);
		}
		
		// Use this for initialization
		void Start ()
		{
			anim = GetComponent<Animator>();
			anim.SetBool("move", true);
		}
		// Update is called once per frame
		void Update ()
		{
			Vector3 newPosition = transform.position;
			GetComponentInChildren<Rigidbody>().MovePosition(newPosition + new Vector3(-Speed, 0));
			

		}

		void OnAnimatorMove()
		{
			
		}

		void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.CompareTag("Kill"))
			{
				Destroy(gameObject);
			}
		}

		void OnCollisionEnter(Collision collision)
		{
			var isTerrain = collision.gameObject.layer == LayerMask.NameToLayer("Terrain");

			if (isTerrain)
			{
				ContactPoint contact = collision.contacts[0];
				if (contact.normal.y == -1)
				{
					anim.SetBool("move", false);
					
				}
			}
		}
	}

	public interface IReversable
	{
		void Reverse();
	}
}