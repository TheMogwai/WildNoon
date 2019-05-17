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
		public GameObject m_movementNode;
        public GameObject m_rangeNode;
        public LayerMask layerMask;

		List<GameObject> possibleMoves = new List<GameObject>();
		EventSystem eventSystem;

        PlayerManager player;

        Ray ray;
        TurnBasedAI unitUnderMouse;

        int MoveRange;
        float unitMobility;

        bool isMoving;

        public Color[] color;
        MeshRenderer render;
        Vector3 _heading;

        int UnitActionPoints;
        int nbrDeZoneDeMouvement;

        float mobi;
        float distanceToPlayer;
        float mobiLeft;
        float nbrNodesParcourus;

        bool HardStop;

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
        public float nodes;

        private void Awake()
        {
            m_sM.AddStates(new List<IState> {
            new SelectUnit(this),                   // Numéro 0
            new SelectTarget(this),                 // Numéro 1
            new UnitRangeAttack(this),              // Numéro 2
            new UnitsMoving(this),                  // Numéro 3
            new PlayerLookingForTravel(this),       // Numéro 4
            new SpellRange(this),                   // Numéro 5
		});


            eventSystem = FindObjectOfType<EventSystem>();
            Player = FindObjectOfType<PlayerManager>();
            if(player != null)
            {
                if (player.OnActiveUnit1 != null)
                {
                    unitMobility = player.OnActiveUnit1.Mobility;
                    UnitActionPoints = player.OnActiveUnit1.ActionPoints;
                    nbrDeZoneDeMouvement = 2;
                    OneTimeThing = true;
                }
            }
        }

        
		/*public State state = State.SelectUnit;

		public enum State {
			SelectUnit,
			SelectTarget,
            Attack,
			Move
		}*/

        

        void Update () {

            m_sM.Update();
            Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            UnitUnderMouse = GetByRay<TurnBasedAI>(Ray);

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if(Player != null)
                {
                    if (UnitUnderMouse == Player.OnActiveUnit1.GetComponent<TurnBasedAI>())
                    {
                        ChangeState(1);
                    }
                }
                else
                {
                    ChangeState(1);
                }
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                HardStop = true;
            }

            // Ignore any input while the mouse is over a UI element
            if (eventSystem.IsPointerOverGameObject()) {
				return;
			}
        }

        public void OnSetMouvement()
        {
            UnitActionPoints = player.OnActiveUnit1.ActionPoints;
            nbrDeZoneDeMouvement = 2;
            unitMobility = player.OnActiveUnit1.Mobility;
            MoveRange = (player.OnActiveUnit1.Mobility)*6;
            mobi = unitMobility * nodes;
            mobiLeft = mobi;
            nbrNodesParcourus = 0;
            OneTimeThing = true;
        }

        public void OnUnitSelected()
        {
            DestroyPossibleMoves();
            GeneratePossibleMoves(selected);
        }

        public void OnUnitAttack()
        {
            DestroyPossibleMoves();
            GeneratePossibleRange(selected , player.OnActiveUnit1.Range);
        }

        public void OnShowRange()
        {
            DestroyPossibleMoves();
            Debug.Log("Jacky Dispose d'une range de : " + player.OnActiveUnit1.Spells1[player.OnUsedSpell].m_spellRange);
            GeneratePossibleRange(selected, player.OnActiveUnit1.Spells1[player.OnUsedSpell].m_spellRange);
        }

        //int moveCost;
        #region Player Clique Sur Un Node
        public void HandleButtonUnderRay (Ray ray) {
			var button = GetByRay<Astar3DButton>(ray);

            if (eventSystem.IsPointerOverGameObject())
            {
                return;

            }
            else if (button != null && Input.GetKeyDown(KeyCode.Mouse0)) {
                if(UnitActionPoints - button.OnClick() >= 0)
                {
				    
                    //Debug.Log("Il ne reste que : " + player.OnActiveUnit1.ActionPoints + " points d'action");
                    ChangeState(3);
                    StartCoroutine(MoveToNode(selected, button.node));
                    player.ActionPointsDisplay(UnitActionPoints- button.OnClick());                           //Call PlayerManager Action Display Method
                    //moveCost = button.OnClick();
                    UnitActionPoints -= button.OnClick();
                    //nbrDeZoneDeMouvement = UnitActionPoints;
                }
            }
            else if(button != null)
            {
                player.MovementCost(button.OnClick(), true);
            }
            else
            {
                player.MovementCost(0, false);
            }
        }
        #endregion

        public void HandleButtonUnderRaySpellRange(Ray ray)
        {
            var button = GetByRay<Astar3DButton>(ray);

            if (eventSystem.IsPointerOverGameObject())
            {
                return;

            }
            else if (button != null && Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (UnitActionPoints > 0)
                {
                    player.OnCoolDownspell();
                    ChangeState(0);
                    StartCoroutine(TpToNode(selected, button.node));
                }
            }
        }

        T GetByRay<T>(Ray ray) where T : class {
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, float.PositiveInfinity, layerMask)) {
                
                    return hit.transform.GetComponentInParent<T>();
			}

            return null;
		}

		public void Select () {
            if(player != null)
            {
                selected = player.OnActiveUnit1.GetComponent<TurnBasedAI>();
            }
            else
            {
                selected = unitUnderMouse;
            }
		}
        

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
				//state = State.SelectTarget;
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
            //state = State.SelectUnit;
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

            nbrNodesParcourus += path.vectorPath.Count - 1;         //Nbr de Nodes Parcouru

            Debug.Log("NodesParcouru " + nbrNodesParcourus);
            mobiLeft = (unitMobility - nbrNodesParcourus) * 2.5f;


            if (UnitActionPoints >= 2)
            {
                nbrDeZoneDeMouvement = 2;
            }
            else
            {
                nbrDeZoneDeMouvement = UnitActionPoints;
            }
            ChangeState(1);             //Respawn Nodes

        }

        IEnumerator TpToNode(TurnBasedAI unit, GraphNode node)
        {

            yield return new WaitForSeconds(1f);                                //Temps de l'anim de tp
            unit.transform.position = (Vector3)node.position;
            unit.blocker.BlockAtCurrentPosition();

        }

        public void DestroyPossibleMoves () {
            foreach (var go in possibleMoves) {
                
				GameObject.Destroy(go);
            }
            possibleMoves.Clear();
		}
        bool OneTimeThing;
        public float indiceGitanerie = 0.95f;
        public void GeneratePossibleMoves(TurnBasedAI unit)
        {
            var path = ConstantPath.Construct(unit.transform.position, (MoveRange * 3) * 1000 + 1);

            path.traversalProvider = unit.traversalProvider;

            AstarPath.StartPath(path);

            path.BlockUntilCalculated();
            Debug.Log("MobiLeft " + mobiLeft);

            #region mobiLeft and ActionPoints Maths
            /*if (mobiLeft <= 0)
            {
                for (int i = 0; i < 6; i++)
                {
                    if (mobiLeft >= i * (-mobi) && moveCost !=0)
                    {
                        if(UnitActionPoints- (i) > 0)
                        {
                            UnitActionPoints -= i +1;
                        }
                        else
                        {
                            UnitActionPoints = 0;
                        }
                        break;
                    }
                }

                
                if (UnitActionPoints == 0 && mobiLeft < 0)
                {
                    nbrDeZoneDeMouvement = 1;
                }

                while (mobiLeft <= 0 && !HardStop)
                {

                    mobiLeft = mobi + mobiLeft;

                }

                /*if(UnitActionPoints == 0 && mobiLeft == mobi && OneTimeThing)
                {
                    UnitActionPoints++;
                    OneTimeThing = false;
                }*//*

                if (mobiLeft == mobi)
                {
                    nbrDeZoneDeMouvement = UnitActionPoints;
                }
                else
                {
                    nbrDeZoneDeMouvement = UnitActionPoints;
                    nbrDeZoneDeMouvement++;
                    //nbrDeZoneDeMouvement = UnitActionPoints;
                }
            }

            if(mobiLeft != mobi)
            {
                nbrNodesParcourus = ((mobi - mobiLeft) / 2.5f);
            }
            else
            {
                nbrNodesParcourus = 0;
            }
                Debug.Log("Action Point Left " + UnitActionPoints);

            Debug.Log("nbrDeZoneDeMouvement " + nbrDeZoneDeMouvement);*/

    
            #endregion

            foreach (var node in path.allNodes)
            {
                if (node != path.startNode)
                {
                    var go = GameObject.Instantiate(m_movementNode, (Vector3)node.position, Quaternion.identity) as GameObject;
                    var heading = (Vector3)node.position - player.OnActiveUnit1.gameObject.transform.position;
                    _heading = heading;
                    distanceToPlayer = heading.magnitude;
                    render = go.GetComponentInChildren<MeshRenderer>();



                    #region Coloring Nodes
                    for (int i = 0; i < UnitActionPoints; ++i)
                    {
                        if(i == 0)
                        {
                            
                            if (distanceToPlayer < ((mobi)))
                            {
                                render.material.color = color[0];

                                go.GetComponent<Astar3DButton>().cost = (1);

                                //if (mobiLeft == mobi)
                                //{
                                //}
                                //else
                                //{
                                //    go.GetComponent<Astar3DButton>().cost = (0);
                                //}
                                break;
                            }
                        }
                        else
                        {
                            if (distanceToPlayer < (mobi) * (i + 1))
                            {
                                render.material.color = color[i];
                                go.GetComponent<Astar3DButton>().cost = (i+1);
                                /*if(mobiLeft == mobi)
                                {
                                    go.GetComponent<Astar3DButton>().cost = (i+1);
                                }
                                else
                                {
                                }*/
                                break;
                            }
                        }
                    }
                    #endregion

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
                    var go = GameObject.Instantiate(m_rangeNode, (Vector3)node.position, Quaternion.identity) as GameObject;
                    possibleMoves.Add(go);

                    go.GetComponent<Astar3DButton>().node = node;
                }
            }
        }

    }
}
//if (unit.MovementPoints < 5)
//{
//    path = ConstantPath.Construct(unit.transform.position, (mouvementLeft * 3) * 1000 + 1);
//}
//else if (unit.MovementPoints < 7)
//{
//    path = ConstantPath.Construct(unit.transform.position, (mouvementLeft * 3 - 1) * 1000 + 1);
//}
//else if (unit.MovementPoints < 9)
//{
//    path = ConstantPath.Construct(unit.transform.position, (mouvementLeft * 3 - 2) * 1000 + 1);
//}
//else if (unit.MovementPoints < 11)
//{
//    path = ConstantPath.Construct(unit.transform.position, (mouvementLeft * 3 - 3) * 1000 + 1);
//}
//else
//{
//    Debug.LogError("Appel Paul, il saura pourquoi ca bug");
//}
/*for (int i = 0, l = mouvementLeft; i < l; ++i)
{
    nbrTotalNodes += 6 * i;
}*/
//Debug.Log(path.allNodes.Capacity);

//Debug.Log("checkFix : " + nbrTotalNodesFix);
/*for (int i = 0, l = path.allNodes.Capacity + 1; i < l; ++i)
{
    if(path.allNodes[i] != null)
    {
        if (path.allNodes[i] != path.startNode)
        {
            //GameObject go;
            if (i < nbrTotalNodes+1)
            {
                // Create a new node prefab to indicate a node that can be reached
                // NOTE: If you are going to use this in a real game, you might want to
                // use an object pool to avoid instantiating new GameObjects all the time
                var go = GameObject.Instantiate(nodePrefabMovement_1PA, (Vector3)path.allNodes[i].position, Quaternion.identity) as GameObject;
                possibleMoves.Add(go);

                go.GetComponent<Astar3DButton>().node = path.allNodes[i];
            }
            else
            {
                var go = GameObject.Instantiate(nodePrefabMovement_2PA, (Vector3)path.allNodes[i].position, Quaternion.identity) as GameObject;
                possibleMoves.Add(go);

                go.GetComponent<Astar3DButton>().node = path.allNodes[i];
            }

        }
    }
}*/
/*for (int l = nbrTotalNodesFix+1; I < l; I++)
{
    Debug.Log("check : "+I);
    Debug.Log("checkFix : "+ l);
    //if (path.allNodes[I] != null)
    //{

        // Create a new node prefab to indicate a node that can be reached
        // NOTE: If you are going to use this in a real game, you might want to
        // use an object pool to avoid instantiating new GameObjects all the time
        var go = GameObject.Instantiate(nodePrefabRange, (Vector3)path.allNodes[I].position, Quaternion.identity) as GameObject;
    //possibleMoves.Add(go);

    //go.GetComponent<Astar3DButton>().node = path.allNodes[I];
    possibleMoves.Add(go);

    go.GetComponent<Astar3DButton>().node = path.allNodes[I];
    //}
}*/
