using UnityEngine;

namespace Assets.JumpAndBomb
{
	public class WalkBackFromWall : MonoBehaviour {

		void OnCollisionEnter(Collision collision)
		{
			var isTerrain = collision.gameObject.layer == LayerMask.NameToLayer("Terrain");
			var isMonster = collision.gameObject.layer == LayerMask.NameToLayer("Monster");

			if (isTerrain || isMonster)
			{
				ContactPoint contact = collision.contacts[0];
				if (contact.normal.y <= 0.2f && contact.normal.y >= -0.2f)
					GetComponent<IReversable>().Reverse();
			}
			
		}

	}
}
