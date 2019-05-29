using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Pathfinding.Examples
{
    /** Helper script in the example scene 'Turn Based' */
    //[RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(SingleNodeBlocker))]
    [HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_examples_1_1_turn_based_door.php")]
    public class TurnBasedDoor : MonoBehaviour
    {
        //Animator animator;
        SingleNodeBlocker blocker;
        PlayerManager player;
        TurnBasedManager m_TurnBaseManager;
        HexagonTrigger button;

        public Transform linkedMine;
        public int tpCost = 2;
        bool open;

        void Awake()
        {
            //animator = GetComponent<Animator>();
            blocker = GetComponent<SingleNodeBlocker>();
            Player = FindObjectOfType<PlayerManager>();
        
        }

        void Start()
        {
            // Make sure the door starts out blocked
            //blocker.BlockAtCurrentPosition();
            Debug.Log(Player.OnActiveUnit1);
            //animator.CrossFade("close", 0.2f);
        }

        public void Close()
        {
            StartCoroutine(WaitAndClose());
        }

        public PlayerManager Player
        {
            get
            {
                return player;
            }

            set
            {
                player = value;
            }
        }

        public IEnumerator TpToMine(/*, GraphNode node*/)
        {
            Player.OnActiveUnit1.m_isInAnimation = true;
            yield return new WaitForSeconds(1f);                                //Temps de l'anim de tp
            Player.OnActiveUnit1.m_isInAnimation = false;
            Player.OnActiveUnit1.transform.position = linkedMine.position;
            Player.OnActiveUnit1.GetComponent<TurnBasedAI>().blocker.BlockAtCurrentPosition();
            Player.TurnBasedManager.ChangeState(0);


        }

        IEnumerator WaitAndClose()
        {
            var selector = new List<SingleNodeBlocker>() { blocker };
            var node = AstarPath.active.GetNearest(transform.position).node;

            // Wait while there is another SingleNodeBlocker occupying the same node as the door
            // this is likely another unit which is standing on the door node, and then we cannot
            // close the door
            if (blocker.manager.NodeContainsAnyExcept(node, selector))
            {
                // Door is blocked
                //animator.CrossFade("blocked", 0.2f);
            }

            while (blocker.manager.NodeContainsAnyExcept(node, selector))
            {
                yield return null;
            }

            open = false;
            //animator.CrossFade("close", 0.2f);
            blocker.BlockAtCurrentPosition();
        }

        public void Open()
        {
            if (Player.OnActiveUnit1.ActionPoints >= tpCost)
            {
                // Stop WaitAndClose if it is running
                StopCoroutine(WaitAndClose());

                // Play the open door animation
                //animator.CrossFade("open", 0.2f);
                open = true;
                StartCoroutine(TpToMine());
                Player.TurnBasedManager.ChangeState(1);
                Player.OnActiveUnit1.ActionPoints -= tpCost;

                // Unblock the door node so that units can traverse it again
                blocker.Unblock();
            }
        }

        public void Toggle()
        {
            if (open)
            {
                Close();
            }
            else
            {
                Open();
            }
        }
    }
}
