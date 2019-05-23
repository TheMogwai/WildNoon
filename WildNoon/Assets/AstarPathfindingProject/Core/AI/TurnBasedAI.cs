using UnityEngine;
using System.Collections.Generic;

namespace Pathfinding.Examples {
	/** Helper script in the example scene 'Turn Based' */
	[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_examples_1_1_turn_based_a_i.php")]
	public class TurnBasedAI : VersionedMonoBehaviour {
        Characters m_unitStats;
        int movementPoints;
        private BlockManager blockManager;
        public SingleNodeBlocker blocker;
		public GraphNode targetNode;
		public BlockManager.TraversalProvider traversalProvider;

        #region Get Set
        public int MovementPoints
        {
            get
            {
                return movementPoints;
            }

            set
            {
                movementPoints = value;
            }
        }
        #endregion

        void Start () {
			blocker.BlockAtCurrentPosition();
		}

        public override void Awake () {
			base.Awake();
            m_unitStats = GetComponent<UnitCara>().unitStats;
            MovementPoints = m_unitStats.m_mobility;
            // Set the traversal provider to block all nodes that are blocked by a SingleNodeBlocker
            // except the SingleNodeBlocker owned by this AI (we don't want to be blocked by ourself)
            blocker = GetComponent<SingleNodeBlocker>();
            blockManager = FindObjectOfType<BlockManager>();
            traversalProvider = new BlockManager.TraversalProvider(blockManager, BlockManager.BlockMode.AllExceptSelector, new List<SingleNodeBlocker>() { blocker });


		}
	}
}
