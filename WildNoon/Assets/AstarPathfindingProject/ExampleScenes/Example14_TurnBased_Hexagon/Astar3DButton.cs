using UnityEngine;
using System.Collections;

namespace Pathfinding.Examples {
	/** Helper script in the example scene 'Turn Based' */
	[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_examples_1_1_astar3_d_button.php")]
	public class Astar3DButton : MonoBehaviour {
		public GraphNode node;
        public int cost;

        /*float distance;

        PlayerManager player;

        private void Awake()
        {
            player = FindObjectOfType<PlayerManager>();
            distance = (transform.position - player.OnActiveUnit1.gameObject.transform.position).magnitude;
        }*/


        public void OnHover (TurnBasedAI unit)
        {
            // TODO: Play animation
        }

		public int OnClick () {

            return cost;
			// TODO: Play animation
		}

        private void Update()
        {
            
        }

    }
}
