using UnityEngine;

namespace Assets.JumpAndBomb
{
	public class WalkBackFromWall : MonoBehaviour {

		void OnCollisionEnter2D(Collision2D collision)
		{
			var isTerrain = collision.gameObject.layer == LayerMask.NameToLayer("Terrain");
			var isMonster = collision.gameObject.layer == LayerMask.NameToLayer("Monster");

			if (isTerrain || isMonster)
			{
				var contact = collision.contacts[0];
				if (contact.normal.y <= 0.2f && contact.normal.y >= -0.2f)
					GetComponent<IReversable>().Reverse();
			}
			
		}

	}
}
