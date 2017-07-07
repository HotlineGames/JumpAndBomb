using UnityEngine;

namespace Assets.JumpAndBomb
{
	public class WalkBackFromWall : MonoBehaviour {

		// Use this for initialization
		void Start () {
		
		}
	
		// Update is called once per frame
		void Update () {
		
		}

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
