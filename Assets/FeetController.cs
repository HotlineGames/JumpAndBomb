using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FeetController : MonoBehaviour
{
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Terrain"))
		{
			var player = GetComponentInParent<PlayerController>();
			player.ResetFuel();
		}
	}
}