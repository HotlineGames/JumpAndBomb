using UnityEngine;

namespace Assets.JumpAndBomb
{
	public class WalkBackFromWall : MonoBehaviour {

		void OnCollisionEnter(Collision collision)
		{
			var isTerrain = collision.gameObject.layer == LayerMask.NameToLayer("Terrain");

			if (isTerrain)
			{
				ContactPoint contact = collision.contacts[0];
				if (contact.normal.y == 0.0f)
					GetComponent<IReversable>().Reverse();
			}
		}

	}
}
