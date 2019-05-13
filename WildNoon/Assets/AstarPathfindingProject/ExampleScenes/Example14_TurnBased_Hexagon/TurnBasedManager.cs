using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine.EventSystems;

namespace Pathfinding.Examples {
	/** Helper script in the example scene 'Turn Based' */
	[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_examples_1_1_turn_based_manager.php")]
	public class TurnBasedManager : MonoBehaviour {

        [SerializeField] bool m_isInstatiate = true;

        #region State Machine

        public StateMachine m_sM = new StateMachine();


        public virtual void OnEnable()
        {

            if (!m_isInstatiate)
            {
                ChangeState(0);
            }
        }

        public void ChangeState(int i)
        {
            m_sM.ChangeState(i);
        }

        public int GetLastStateIndex()
        {
            return m_sM.LastStateIndex;
        }

        #endregion


        TurnBasedAI selected;

		public float movementSpeed;
		public GameObject nodePrefabMovement;
		public GameObject nodePrefabRange;
        public LayerMask layerMask;

		List<GameObject> possibleMoves = new List<GameObject>();
		EventSystem eventSystem;

        PlayerManager player;
        static int mouvementLeft;


        #region Get Set
        public Ray Ray
        {
            get
            {
                return ray;
            }

            set
            {
                ray = value;
            }
        }

        public TurnBasedAI UnitUnderMouse
        {
            get
            {
                return unitUnderMouse;
            }

            set
            {
                unitUnderMouse = value;
            }
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

        public bool IsMoving
        {
            get
            {
                return isMoving;
            }

            set
            {
                isMoving = value;
            }
        }
        #endregion

        private void Awake()
        {
            m_sM.AddStates(new List<IState> {
            new SelectUnit(this),                   // Numéro 0
            new SelectTarget(this),                 // Numéro 1
            new UnitRangeAttack(this),              // Numéro 2
            new UnitsMoving(this),                  // Numéro 3
		});


            eventSystem = FindObjectOfType<EventSystem>();
            Player = FindObjectOfType<PlayerManager>();
            if (player.OnActiveUnit1 != null)
            {
                mouvementLeft = player.OnActiveUnit1.unitStats.m_mobility;
            }
        }


		public State state = State.SelectUnit;

		public enum State {
			SelectUnit,
			SelectTarget,
            Attack,
			Move
		}

        Ray ray;
        TurnBasedAI unitUnderMouse;



        void Update () {

            m_sM.Update();
            Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            UnitUnderMouse = GetByRay<TurnBasedAI>(Ray);
            if(UnitUnderMouse != null)
            {
                Debug.Log(UnitUnderMouse.gameObject.name);
            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {

                if (UnitUnderMouse == Player.OnActiveUnit1.GetComponent<TurnBasedAI>())
                {
                    ChangeState(1);
                }
            }

            // Ignore any input while the mouse is over a UI element
            if (eventSystem.IsPointerOverGameObject()) {
				return;
			}

            

            /*if(state == State.Attack)
            {
                DestroyPossibleMoves();
                GeneratePossibleRange(selected, player.OnActiveUnit1.unitStats.m_range);
            }
            else if(state == State.SelectTarget)
            {
                    DestroyPossibleMoves();
                    GeneratePossibleMoves(selected);
            }*/

		}
        public void OnSetMouvement()
        {
                mouvementLeft = player.OnActiveUnit1.unitStats.m_mobility+1;

        }

        public void OnUnitSelected()
        {
            DestroyPossibleMoves();
            GeneratePossibleMoves(selected);
        }

        public void OnUnitAttack()
        {
            DestroyPossibleMoves();
            GeneratePossibleRange(selected , player.OnActiveUnit1.unitStats.m_range);
        }

		// TODO: Move to separate class
		public void HandleButtonUnderRay (Ray ray) {
			var button = GetByRay<Astar3DButton>(ray);

			if (button != null && Input.GetKeyDown(KeyCode.Mouse0)) {
				button.OnClick();

                ChangeState(3);
				StartCoroutine(MoveToNode(selected, button.node));
			}
		}

		T GetByRay<T>(Ray ray) where T : class {
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit, float.PositiveInfinity, layerMask)) {
                
                    return hit.transform.GetComponentInParent<T>();
			}

            return null;
		}
    /*if ((hit.rigidbody == Player.OnActiveUnit1.gameObject.GetComponent<Rigidbody>()) || ((hit.rigidbody != Player.OnActiveUnit1.gameObject.GetComponent<Rigidbody>()) &&(hit.transform.gameObject.layer == 0)))
                {*/

		public void Select () {
            selected = player.OnActiveUnit1.GetComponent<TurnBasedAI>();
		}

        bool isMoving;
		public IEnumerator MoveToNode (TurnBasedAI unit, GraphNode node) {
            IsMoving = true;
			var path = ABPath.Construct(unit.transform.position, (Vector3)node.position);

			path.traversalProvider = unit.traversalProvider;

			// Schedule the path for calculation
			AstarPath.StartPath(path);

			// Wait for the path calculation to complete
			yield return StartCoroutine(path.WaitForPath());

			if (path.error) {
				// Not obvious what to do here, but show the possible moves again
				// and let the player choose another target node
				// Likely a node was blocked between the possible moves being
				// generated and the player choosing which node to move to
				Debug.LogError("Path failed:\n" + path.errorLog);
				state = State.SelectTarget;
				GeneratePossibleMoves(selected);
				yield break;
			}

			// Set the target node so other scripts know which
			// node is the end point in the path
			unit.targetNode = path.path[path.path.Count - 1];

			yield return StartCoroutine(MoveAlongPath(unit, path, movementSpeed));

			unit.blocker.BlockAtCurrentPosition();

            // Select a new unit to move
            IsMoving = false;
            state = State.SelectUnit;
		}

		/** Interpolates the unit along the path */
		IEnumerator MoveAlongPath (TurnBasedAI unit, ABPath path, float speed) {
			if (path.error || path.vectorPath.Count == 0)
				throw new System.ArgumentException("Cannot follow an empty path");

			// Very simple movement, just interpolate using a catmull rom spline
			float distanceAlongSegment = 0;
			for (int i = 0; i < path.vectorPath.Count - 1; i++) {
				var p0 = path.vectorPath[Mathf.Max(i-1, 0)];
				// Start of current segment
				var p1 = path.vectorPath[i];
				// End of current segment
				var p2 = path.vectorPath[i+1];
				var p3 = path.vectorPath[Mathf.Min(i+2, path.vectorPath.Count-1)];

				var segmentLength = Vector3.Distance(p1, p2);

				while (distanceAlongSegment < segmentLength) {
					var interpolatedPoint = AstarSplines.CatmullRom(p0, p1, p2, p3, distanceAlongSegment / segmentLength);
					unit.transform.position = interpolatedPoint;
					yield return null;
					distanceAlongSegment += Time.deltaTime * speed;
				}

				distanceAlongSegment -= segmentLength;
			}

			unit.transform.position = path.vectorPath[path.vectorPath.Count - 1];
            Debug.Log(path.vectorPath.Count);                                                                   //Nbr de Nodes Parcouru
            mouvementLeft -= path.vectorPath.Count-1;

        }

		public void DestroyPossibleMoves () {
			foreach (var go in possibleMoves) {
				GameObject.Destroy(go);
			}
			possibleMoves.Clear();
		}

		public void GeneratePossibleMoves (TurnBasedAI unit) {
            var path = ConstantPath.Construct(unit.transform.position, (mouvementLeft * 3) * 1000 + 1);
            if (unit.MovementPoints < 5)
            {
                path = ConstantPath.Construct(unit.transform.position, (mouvementLeft * 3) * 1000 + 1);
            }
            else if (unit.MovementPoints < 7)
            {
                path = ConstantPath.Construct(unit.transform.position, (mouvementLeft * 3 -1) * 1000 + 1);
            }
            else if(unit.MovementPoints < 9)
            {
                path = ConstantPath.Construct(unit.transform.position, (mouvementLeft * 3 - 2) * 1000 + 1);
            }
            else if (unit.MovementPoints < 11)
            {
                path = ConstantPath.Construct(unit.transform.position, (mouvementLeft * 3 - 3) * 1000 + 1);
            }
            else
            {
               // Debug.LogError("Appel Paul, il saura pourquoi ca bug");
            }

            path.traversalProvider = unit.traversalProvider;

			// Schedule the path for calculation
			AstarPath.StartPath(path);

			// Force the path request to complete immediately
			// This assumes the graph is small enough that
			// this will not cause any lag
			path.BlockUntilCalculated();

			foreach (var node in path.allNodes) {
				if (node != path.startNode) {
                    // Create a new node prefab to indicate a node that can be reached
                    // NOTE: If you are going to use this in a real game, you might want to
                    // use an object pool to avoid instantiating new GameObjects all the time
                    var go = GameObject.Instantiate(nodePrefabMovement, (Vector3)node.position, Quaternion.identity) as GameObject;
					possibleMoves.Add(go);

					go.GetComponent<Astar3DButton>().node = node;
				}
			}
		}

        public void GeneratePossibleRange(TurnBasedAI unit, int range)
        {
            var path = ConstantPath.Construct(unit.transform.position, (range * 3) * 1000 + 1); ;
            if (range < 5)
            {
                path = ConstantPath.Construct(unit.transform.position, (range * 3) * 1000 + 1);
            }
            else if (range < 7)
            {
                path = ConstantPath.Construct(unit.transform.position, (range * 3 - 1) * 1000 + 1);
            }
            else if (range < 9)
            {
                path = ConstantPath.Construct(unit.transform.position, (range * 3 - 2) * 1000 + 1);
            }
            else if (range < 11)
            {
                path = ConstantPath.Construct(unit.transform.position, (range * 3 - 3) * 1000 + 1);
            }
            else
            {
                Debug.LogError("Appel Paul, il saura pourquoi ca bug");
            }

            path.traversalProvider = unit.traversalProvider;

            // Schedule the path for calculation
            AstarPath.StartPath(path);

            // Force the path request to complete immediately
            // This assumes the graph is small enough that
            // this will not cause any lag
            path.BlockUntilCalculated();

            foreach (var node in path.allNodes)
            {
                if (node != path.startNode)
                {
                    // Create a new node prefab to indicate a node that can be reached
                    // NOTE: If you are going to use this in a real game, you might want to
                    // use an object pool to avoid instantiating new GameObjects all the time
                    var go = GameObject.Instantiate(nodePrefabRange, (Vector3)node.position, Quaternion.identity) as GameObject;
                    possibleMoves.Add(go);

                    go.GetComponent<Astar3DButton>().node = node;
                }
            }
        }

    }
}
