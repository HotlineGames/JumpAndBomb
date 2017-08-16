using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.JumpAndBomb
{
	public class NotGoombaController : MonoBehaviour, IReversable
	{
		private Animator _anim;
		public float Speed;
		public float JumpSpeed;
		private Rigidbody2D rigidbody;
		private bool _inAnimation;
		public float JumpCd;
		private ParticleSystem par;

		public void Reverse()
		{
			Speed *= -1;
			rigidbody.velocity = new Vector3(Speed, rigidbody.velocity.y);
			transform.Rotate(new Vector3(0, 1, 0), 180);
		}
		
		// Use this for initialization
		void Start ()
		{
			par = GetComponent<ParticleSystem>();
			_anim = GetComponent<Animator>();
			rigidbody = GetComponent<Rigidbody2D>();
			_inAnimation = false;
		}
		// Update is called once per frame
		void Update ()
		{
			Jump();
		}

		void Jump()
		{
			if (_anim.GetCurrentAnimatorStateInfo(0).IsName("Move") && !_anim.IsInTransition(0) && _anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.2f && !_inAnimation)
			{

				rigidbody.velocity = new Vector3(Speed, JumpSpeed);
				//print(transform.GetChild(1).transform.GetChild(0).transform.position.y);
				//float y = transform.GetChild(1).transform.GetChild(0).transform.position.y;
				//Vector3 newPosition = transform.position;
				//newPosition.x += -Speed * Time.deltaTime;
				//newPosition.y += Speed * Time.deltaTime;
				//transform.position = newPosition;
				//newPosition.y = y * Time.deltaTime;
				//GetComponentInChildren<Rigidbody>().MovePosition(newPosition + new Vector3(0, y));
				StartCoroutine(JumpRoutine()); 
				
			}
			
		}

		IEnumerator JumpRoutine()
		{
			_inAnimation = true;
			_anim.SetBool("JumpAvailable", false);
			yield return new WaitForSeconds(JumpCd);
			_inAnimation = false;
			_anim.SetBool("JumpAvailable", true);

		}
		void OnTriggerEnter2D(Collider2D other)
		{
			if (other.gameObject.CompareTag("Kill"))
			{
				var rig = other.GetComponentInParent<Rigidbody2D>();
				rig.velocity	= new Vector3(rig.velocity.x, rig.velocity.y *-1);
				_anim.SetBool("IsDead", true);
				par.Play();
				gameObject.layer = LayerMask.NameToLayer("Dead");
				gameObject.tag = "Dead";
				transform.localScale = new Vector3(50,10,90);
			}
		}

		void OnCollisionEnter2D(Collision2D collision)
		{
			var isTerrain = collision.gameObject.layer == LayerMask.NameToLayer("Terrain");

			if (isTerrain)
			{
				var contact = collision.contacts[0];
				if (contact.normal.y == -1)
				{
					_anim.SetBool("Move", false);
					
				}
			}
		}
	}

	public interface IReversable
	{
		void Reverse();
	}
}