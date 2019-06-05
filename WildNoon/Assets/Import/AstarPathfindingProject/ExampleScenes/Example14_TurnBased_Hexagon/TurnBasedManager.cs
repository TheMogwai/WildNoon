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
        Astar3DButton nodeUnderMouse;

        int MoveRange;
        float unitMobility;

        bool isMoving;

        public Color[] color;
        MeshRenderer render;
        Vector3 _heading;

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

        public EventSystem EventSystem
        {
            get
            {
                return eventSystem;
            }

            set
            {
                eventSystem = value;
            }
        }

        public TurnBasedAI Selected
        {
            get
            {
                return selected;
            }

            set
            {
                selected = value;
            }
        }

        public Astar3DButton NodeUnderMouse
        {
            get
            {
                return nodeUnderMouse;
            }

            set
            {
                nodeUnderMouse = value;
            }
        }

        #endregion
        public float nodes;

        private void Awake()
        {
            m_sM.AddStates(new List<IState> {
            new SelectUnit(this),                   // Num�ro 0
            new SelectTarget(this),                 // Num�ro 1
            new UnitRangeAttack(this),              // Num�ro 2
            new UnitsMoving(this),                  // Num�ro 3
            new Jacky_SlowRange(this),              // Num�ro 4
            new Jacky_TpRange(this),                // Num�ro 5
            new Jacky_TauntRange(this),             // Num�ro 6
            new Jacky_TauntBeingUsed(this),         // Num�ro 7
            new Jim_PoingAmericain(this),           // Num�ro 8
            new Jim_Lasso(this),                    // Num�ro 9
            new Jim_GrandLassoRange(this),          // Num�ro 10
            new Jim_GrandLassoBeingUsed(this),      // Num�ro 11
		});


            EventSystem = FindObjectOfType<EventSystem>();
            Player = FindObjectOfType<PlayerManager>();
            if(player != null)
            {
                if (player._onActiveUnit != null)
                {
                    unitMobility = player._onActiveUnit.Mobility;
                    nbrDeZoneDeMouvement = 2;
                }
            }
        }

        void Update () {

            m_sM.Update();
            Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            UnitUnderMouse = GetByRay<TurnBasedAI>(Ray);
            NodeUnderMouse = GetByRay<Astar3DButton>(Ray);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                HardStop = true;
            }

            // Ignore any input while the mouse is over a UI element
            if (EventSystem.IsPointerOverGameObject()) {
				return;
			}


            if (UnitUnderMouse != null)
            {
                UnitCara unit = UnitUnderMouse.GetComponent<UnitCara>();
                Player._unitUnderMouseStats.SetActive(true);
                Player._statsSlider[0].fillAmount = Mathf.InverseLerp(0, unit.unitStats.m_heatlh, unit.LifePoint);
                Player._statsSlider[1].fillAmount = Mathf.InverseLerp(0, unit.unitStats.m_armor, unit.ArmorPoint);
                Player.HealthValueDisplay.text = string.Format("{0}/{1}", unit.LifePoint, unit.unitStats.m_heatlh);
                Player.ArmorValueDisplay.text = string.Format("{0}/{1}", unit.ArmorPoint, unit.unitStats.m_armor);
                /*UnitUnderMouse.GetComponent<UnitCara>().LifebarOn = true;
                UnitUnderMouse.GetComponent<UnitCara>().TimeToShowLifeBar = UnitUnderMouse.GetComponent<UnitCara>().m_timeShowingLifeBar;*/
            }
            else
            {
                Player._unitUnderMouseStats.SetActive(false);
            }
        }

        public void OnSetMouvement()
        {
            nbrDeZoneDeMouvement = 2;
            unitMobility = player._onActiveUnit.Mobility;
            MoveRange = (player._onActiveUnit.Mobility)*6;
            mobi = unitMobility * nodes;
            mobiLeft = mobi;
            nbrNodesParcourus = 0;
        }

        public void OnUnitSelected()
        {
            DestroyPossibleMoves();
            GeneratePossibleMoves(Selected);
        }

        public void OnUnitAttack()
        {
            DestroyPossibleMoves();
            GeneratePossibleRange(Selected , player._onActiveUnit.Range);
        }

        public void OnShowRange()
        {
            DestroyPossibleMoves();
            GeneratePossibleRange(Selected, player._onActiveUnit.Spells1[player.OnUsedSpell].m_spellRange);
        }

        /*public void ShowSpellRange(Astar3DButton nodeTargeted)
        {
            GeneratePossibleSpellRange(nodeTargeted, player._onActiveUnit.Spells1[player.OnUsedSpell].m_rangeBonus);
        }*/

        #region Player Clique Sur Un Node
        public void HandleButtonUnderRay (Ray ray) {
			var button = GetByRay<Astar3DButton>(ray);

            if (EventSystem.IsPointerOverGameObject())
            {
                return;

            }
            else if (button != null && Input.GetKeyDown(KeyCode.Mouse0)) {
                if(player._onActiveUnit.ActionPoints - button.OnClick() >= 0)
                {
				    
                    //Debug.Log("Il ne reste que : " + player.OnActiveUnit1.ActionPoints + " points d'action");
                    ChangeState(3);
                    StartCoroutine(MoveToNode(Selected, button.node));
                    GraphNode node = button.node;
                    if (Player._onActiveUnit.Unit_Animator != null)
                    {
                        Player._onActiveUnit.Unit_Animator.SetTrigger("Move");
                        Player._onActiveUnit.Unit_mesh.transform.LookAt((Vector3)node.position);
                    }
                    //moveCost = button.OnClick();
                    player._onActiveUnit.ActionPoints -= button.OnClick();
                    player.ActionPointsDisplay(player._onActiveUnit.ActionPoints);                           //Call PlayerManager Action Display Method
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

        


        public T GetByRay<T>(Ray ray) where T : class {
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, float.PositiveInfinity, layerMask)) {
                
                    return hit.transform.GetComponentInParent<T>();
			}

            return null;
		}

		public void Select () {
            if(player != null)
            {
                Selected = player._onActiveUnit.GetComponent<TurnBasedAI>();
            }
            else
            {
                Selected = unitUnderMouse;
            }
		}

        public void AutoAttack(UnitCara target)
        {
            if (!target.m_isInAnimation && Player._onActiveUnit.ActionPoints > 0 && !Player.IsDisabled && !Player._onActiveUnit.m_isInAnimation)
            {
                if (Player._onActiveUnit.Unit_Animator != null)
                {
                    //Player._onActiveUnit.Unit_Animator.applyRootMotion = true;
                    Vector3 gitano = new Vector3(5, 0, 0);
                    /*Player._onActiveUnit.Unit_mesh.transform.LookAt(target.transform.position + gitano);
                    Player._onActiveUnit.Unit_Animator.SetTrigger("Shoot");*/
                }
                    Player._onActiveUnit.AutoAttack(target);
            }

        }



        #region Move Unit
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
				GeneratePossibleMoves(Selected);
				yield break;
			}

			// Set the target node so other scripts know which
			// node is the end point in the path
			unit.targetNode = path.path[path.path.Count - 1];


            yield return StartCoroutine(MoveAlongPath(unit, path, movementSpeed));

			unit.blocker.BlockAtCurrentPosition();

            // Select a new unit to move
            IsMoving = false;
            if(Player._onActiveUnit.Unit_Animator != null)
            {
                Player._onActiveUnit.Unit_Animator.SetTrigger("MoveOff");
            }

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

            //Debug.Log("NodesParcouru " + nbrNodesParcourus);
            mobiLeft = (unitMobility - nbrNodesParcourus) * 2.5f;


            /*if (player.OnActiveUnit1.ActionPoints >= 2)
            {
                nbrDeZoneDeMouvement = 2;
            }
            else
            {
                nbrDeZoneDeMouvement = player.OnActiveUnit1.ActionPoints;
            }*/
            ChangeState(0);             //Respawn Nodes

        }
        #endregion

        #region TauntMethods

        public IEnumerator MoveTowardTarget(TurnBasedAI unit, TurnBasedAI target)
        {
            if (!unit.GetComponent<UnitCara>().JimPassifEffect)
            {
                //NNInfo nn = AstarPath.active.GetNearest(transform.position, NNConstraint.Default);
                //if (nn.node != null)
                //{
                //    Debug.Log("Found closest node at " + (Vector3)(nn.node.position) + " the closest point on the node was " + nn.clampedPosition);
                //}
                //else
                //{
                //    Debug.Log("No close node found, maybe adjust A* Inspector -> Settings -> Max Nearest Node Distance");
                //}

                //
                target.blocker.Unblock();

                NNInfo nnInfo = AstarPath.active.GetNearest(target.transform.position, NNConstraint.None);
                //Debug.LogFormat("{0} - {1}", target.targetNode.position, nnInfo.position);

                IsMoving = true;
                var path = ABPath.Construct(unit.transform.position, nnInfo.position);

                path.traversalProvider = unit.traversalProvider;

                // Schedule the path for calculation
                AstarPath.StartPath(path);

                // Wait for the path calculation to complete
                yield return StartCoroutine(path.WaitForPath());  // le path error vient d'ici
                //Debug.Log(path.error);
                if (path.error)
                {
                    // Not obvious what to do here, but show the possible moves again
                    // and let the player choose another target node
                    // Likely a node was blocked between the possible moves being
                    // generated and the player choosing which node to move to
                    Debug.Log("Path failed:\n" + path.errorLog);
                    //state = State.SelectTarget;
                    GeneratePossibleMoves(Selected);
                    yield break;
                }

                // Set the target node so other scripts know which
                // node is the end point in the path
                unit.targetNode = path.path[path.path.Count - 1];


                yield return StartCoroutine(MoveAlongTauntPath(unit, path, movementSpeed, target));

                //target.blocker.Unblock();

                unit.blocker.BlockAtCurrentPosition();

                // Select a new unit to move
                IsMoving = false;
                if (Player._onActiveUnit.Unit_Animator != null)
                {
                    Player._onActiveUnit.Unit_Animator.SetTrigger("MoveOff");
                }
                //state = State.SelectUnit;
            }
            else
            {
                unit.GetComponent<UnitCara>()._isTaunt = false;
                Player.OnEffectCheck();
            }
        }
        int nbrNodes = 0;

        /** Interpolates the unit along the path */
        IEnumerator MoveAlongTauntPath(TurnBasedAI unit, ABPath path, float speed, TurnBasedAI target)
        {
            if (path.error || path.vectorPath.Count == 0)
                throw new System.ArgumentException("Cannot follow an empty path");

            // Very simple movement, just interpolate using a catmull rom spline
            float distanceAlongSegment = 0;
            for (int i = 0; i < path.vectorPath.Count - 1; i++)
            {
                UnitCara unitcara = unit.GetComponent<UnitCara>();
                if (i < (path.vectorPath.Count-1)-(unitcara.Range-1) && (unitcara.ActionPoints-1)>=0)
                {
                    nbrNodes++;
                    var p0 = path.vectorPath[Mathf.Max(i - 1, 0)];
                    // Start of current segment
                    var p1 = path.vectorPath[i];
                    // End of current segment
                    var p2 = path.vectorPath[i + 1];
                    var p3 = path.vectorPath[Mathf.Min(i + 2, path.vectorPath.Count - 1)];

                    var segmentLength = Vector3.Distance(p1, p2);

                    while (distanceAlongSegment < segmentLength)
                    {
                        var interpolatedPoint = AstarSplines.CatmullRom(p0, p1, p2, p3, distanceAlongSegment / segmentLength);
                        unit.transform.position = interpolatedPoint;
                        yield return null;
                        distanceAlongSegment += Time.deltaTime * speed;
                    }

                    distanceAlongSegment -= segmentLength;
                    if(nbrNodes == unitcara.Mobility)
                    {
                        nbrNodes = 0;
                        Player._onActiveUnit.ActionPoints--;
                        player.ActionPointsDisplay(Player._onActiveUnit.ActionPoints);                           //Call PlayerManager Action Display Method

                    }
                }
            }
            
            
            if(Player._onActiveUnit.ActionPoints > 0)
            {
                StartCoroutine(AutoAttackTaunt(Player._onActiveUnit, target.GetComponent<UnitCara>()));
            }
            else
            {
                Player.OnTurnPassed();
            }

            nbrNodesParcourus += path.vectorPath.Count - 1;         //Nbr de Nodes Parcouru

            mobiLeft = (unitMobility - nbrNodesParcourus) * 2.5f;

        }

        public IEnumerator AutoAttackTaunt(UnitCara unit, UnitCara target)
        {
            int actionLeft = unit.ActionPoints;

            if (!target.m_isInAnimation && unit.ActionPoints > 0)
            {
                for (int i = 0; i < actionLeft; i++)
                {
                    unit.m_isInAnimation = true;
                    unit.ActionPoints = unit.ActionPoints - unit.AutoAttackCost;
                    Player.ActionPointsDisplay(unit.ActionPoints);
                    if (target != null)
                    {
                        if(unit.Unit_Animator != null && target.Unit_Animator != null)
                        {
                            //unit.Unit_Animator.SetTrigger("Shoot");
                            //unit.Unit_mesh.transform.LookAt(target.transform.position);
                        }
                        target.OnTakingDamage(unit.Damage);
                    }
                    yield return new WaitForSeconds(1f);                                //Temps de l'anim de l'attaque
                    unit.m_isInAnimation = false;
                }
                Player.OnTurnPassed();
            }
            else
            {
                Player.OnTurnPassed();
            }
        }

        #endregion

        #region JackySpellsMethods

        public IEnumerator TpToNode(TurnBasedAI unit, GraphNode node)
        {
            Player._onActiveUnit.m_isInAnimation = true;
            Player._onActiveUnit.Unit_Animator.SetTrigger("Jacky_Tp");
            yield return new WaitForSeconds(0.5f);                                //Temps de l'anim de tp
            Quaternion gitan = new Quaternion(0.7071071f, 0f, 0f, 0.7071065f);
            Level.AddFX(unit.GetComponent<UnitCara>().SmokeScreen, unit.transform.position, gitan);
            Player._onActiveUnit.m_isInAnimation = false;
            unit.transform.position = (Vector3)node.position;
            unit.blocker.BlockAtCurrentPosition();

        }

        public IEnumerator SlowAttack(TurnBasedAI unit, UnitCara target)
        {
            Player._onActiveUnit.m_isInAnimation = true;

            Player._onActiveUnit.Unit_mesh.transform.LookAt(target.transform.position);
            Player._onActiveUnit.Unit_Animator.SetTrigger("Jacky_SlowAttack");
            yield return new WaitForSeconds(1f);                                //Temps de l'anim de l'attaque
            Player._onActiveUnit.m_isInAnimation = false;
            if (target.OnCheckIfCCWorks())
            {
                target.Mobility -= unit.GetComponent<UnitCara>().OnUsedSpell1.m_mobilityMalus;
                target.IsDebuffed(unit.GetComponent<UnitCara>().OnUsedSpell1.m_turnDebuffLasting, 0);
            }
            else
            {
                target.StartCoroutine(target.ResistanceFadeOut());
            }
            target.OnTakingDamage(unit.GetComponent<UnitCara>().OnUsedSpell1.m_spellDamage);

        }
        List<UnitCara> m_target;
        public IEnumerator TauntAttack(TurnBasedAI unit, List<UnitCara> target)
        {
            m_target = target;
            Player._onActiveUnit.m_isInAnimation = true;

            yield return new WaitForSeconds(0.5f);                                //Temps de l'anim de l'attaque
            Player._onActiveUnit.m_isInAnimation = false;


            for (int i = 0, l = target.Count; i < l; ++i)
            {
                if(target[i] != null)
                {
                    if (target[i].OnCheckIfCCWorks())
                    {
                        target[i].IsTaunt(unit.GetComponent<UnitCara>().OnUsedSpell1.m_turnDebuffLasting, 1, unit);
                    }
                    else
                    {
                        target[i].StartCoroutine(target[i].ResistanceFadeOut());
                    }
                    target[i].Unit_mesh.transform.LookAt(unit.transform.position);
                    target[i].OnTakingDamage(unit.GetComponent<UnitCara>().OnUsedSpell1.m_spellDamage);
                }
            }
        }

        #endregion

        #region Lasso Methods
        public IEnumerator MoveTowardLasso(TurnBasedAI unit, TurnBasedAI target)
        {

            
            target.blocker.Unblock();

            NNInfo nnInfo = AstarPath.active.GetNearest(target.transform.position, NNConstraint.None);

            var path = ABPath.Construct(unit.transform.position, nnInfo.position);

            path.traversalProvider = unit.traversalProvider;

            AstarPath.StartPath(path);

            yield return StartCoroutine(path.WaitForPath());  // le path error vient d'ici
            if (path.error)
            {

                Debug.Log("Path failed:\n" + path.errorLog);
                GeneratePossibleMoves(Selected);
                yield break;
            }


            unit.targetNode = path.path[path.path.Count - 1];


            yield return StartCoroutine(MoveAlongLassoPath(unit, path, movementSpeed, target));


            unit.blocker.BlockAtCurrentPosition();

        }

        IEnumerator MoveAlongLassoPath(TurnBasedAI unit, ABPath path, float speed, TurnBasedAI target)
        {
            if (path.error || path.vectorPath.Count == 0)
                throw new System.ArgumentException("Cannot follow an empty path");

            float distanceAlongSegment = 0;
            for (int i = 0; i < path.vectorPath.Count - 1; i++)
            {
                Debug.Log(path.vectorPath.Count - 1);
                if( i < 2 && path.vectorPath.Count-1 != i+1)
                {
                    var p0 = path.vectorPath[Mathf.Max(i - 1, 0)];
                    var p1 = path.vectorPath[i];
                    var p2 = path.vectorPath[i + 1];
                    var p3 = path.vectorPath[Mathf.Min(i + 2, path.vectorPath.Count - 1)];

                    var segmentLength = Vector3.Distance(p1, p2);

                    while (distanceAlongSegment < segmentLength)
                    {
                        var interpolatedPoint = AstarSplines.CatmullRom(p0, p1, p2, p3, distanceAlongSegment / segmentLength);
                        unit.transform.position = interpolatedPoint;
                        yield return null;
                        distanceAlongSegment += Time.deltaTime * speed;
                    }
                    distanceAlongSegment -= segmentLength;
                }
            }
        }
        #endregion

        #region JimSpellsMethods
        public IEnumerator Lasso(TurnBasedAI unit, UnitCara target)
        {
            if(Player._onActiveUnit.ActionPoints - Player._onActiveUnit.OnUsedSpell1.cost >= 0)
            {
                Player._onActiveUnit.m_isInAnimation = true;

                yield return new WaitForSeconds(0.5f);                                //Temps de l'anim de l'attaque
                Player._onActiveUnit.m_isInAnimation = false;
                Player._onActiveUnit.Unit_mesh.transform.LookAt(target.transform.position);
                if (!target.IsStunByLasso)
                {
                    if (target.OnCheckIfCCWorks())
                    {
                        target.IsStunByLasso = true;
                        target.GetComponent<TurnBasedAI>().StartCoroutine(MoveTowardLasso(target.GetComponent<TurnBasedAI>(), unit));
                    }
                    else
                    {
                        target.StartCoroutine(target.ResistanceFadeOut());

                    }
                    if (target.JimPassifEffect)
                    {
                        target.OnTakingDamage(unit.GetComponent<UnitCara>().OnUsedSpell1.m_spellDamage + unit.GetComponent<UnitCara>().unitStats.firstSpell.m_damageBonus);
                    }
                    else
                    {
                        target.OnTakingDamage(unit.GetComponent<UnitCara>().OnUsedSpell1.m_spellDamage);
                    }
                }
                else
                {
                    target.GetComponent<TurnBasedAI>().StartCoroutine(MoveTowardLasso(target.GetComponent<TurnBasedAI>(), unit));
                    if (target.JimPassifEffect)
                    {
                        target.OnTakingDamage(unit.GetComponent<UnitCara>().OnUsedSpell1.m_spellDamage + unit.GetComponent<UnitCara>().unitStats.firstSpell.m_damageBonus);
                    }
                    else
                    {
                        target.OnTakingDamage(unit.GetComponent<UnitCara>().OnUsedSpell1.m_spellDamage);
                    }
                }
                Player._onActiveUnit.ActionPoints -= Player._onActiveUnit.OnUsedSpell1.cost;
                Player.ActionPointsDisplay(Player._onActiveUnit.ActionPoints);
            }
            Player._onActiveUnit.m_isInAnimation = false;
        }

        public IEnumerator PoingAmericain(TurnBasedAI unit, UnitCara target)
        {
            Player._onActiveUnit.m_isInAnimation = true;

            yield return new WaitForSeconds(0.5f);                                //Temps de l'anim de l'attaque
            Player._onActiveUnit.m_isInAnimation = false;
            Player._onActiveUnit.Unit_mesh.transform.LookAt(target.transform.position);


            float spellDamage = target.unitStats.m_heatlh * (unit.GetComponent<UnitCara>().OnUsedSpell1.m_spellDamage / 100f);
            if (target.JimPassifEffect)
            {
                target.OnTakingDamage((int)spellDamage + unit.GetComponent<UnitCara>().unitStats.firstSpell.m_damageBonus);
            }
            else
            {
                target.OnTakingDamage((int)spellDamage);
            }

        }

        List<UnitCara> m_Lassotarget;
        public IEnumerator GrandLassoAttack(TurnBasedAI unit, List<UnitCara> target)
        {
            m_Lassotarget = target;
            Player._onActiveUnit.m_isInAnimation = true;

            yield return new WaitForSeconds(0.5f);                                //Temps de l'anim de l'attaque
            Player._onActiveUnit.m_isInAnimation = false;

            for (int i = 0, l = target.Count; i < l; ++i)
            {
                if (target[i] != null)
                {
                    target[i].Unit_mesh.transform.LookAt(unit.transform.position);
                    if (target[i].OnCheckIfCCWorks())
                    {
                        target[i].IsStun(unit.GetComponent<UnitCara>().OnUsedSpell1.m_turnDebuffLasting);
                    }
                    else
                    {
                        target[i].StartCoroutine(target[i].ResistanceFadeOut());
                    }
                    if (target[i].JimPassifEffect)
                    {
                        target[i].OnTakingDamage(unit.GetComponent<UnitCara>().OnUsedSpell1.m_spellDamage + unit.GetComponent<UnitCara>().unitStats.firstSpell.m_damageBonus);
                    }
                    else
                    {
                        target[i].OnTakingDamage(unit.GetComponent<UnitCara>().OnUsedSpell1.m_spellDamage);
                    }
                }
            }
        }
        #endregion

        public void DestroyPossibleMoves () {
            foreach (var go in possibleMoves) {
                
				GameObject.Destroy(go);
            }
            possibleMoves.Clear();
		}

        public void GeneratePossibleMoves(TurnBasedAI unit)
        {
            var path = ConstantPath.Construct(unit.transform.position, (MoveRange * 3) * 1000 + 1);

            path.traversalProvider = unit.traversalProvider;

            AstarPath.StartPath(path);

            path.BlockUntilCalculated();
            //Debug.Log("MobiLeft " + mobiLeft);


            foreach (var node in path.allNodes)
            {
                if (node != path.startNode)
                {
                    var go = GameObject.Instantiate(m_movementNode, (Vector3)node.position, Quaternion.identity) as GameObject;
                    var heading = (Vector3)node.position - player._onActiveUnit.gameObject.transform.position;
                    _heading = heading;
                    distanceToPlayer = heading.magnitude;
                    render = go.GetComponentInChildren<MeshRenderer>();



                    #region Coloring Nodes
                    for (int i = 0; i < player._onActiveUnit.ActionPoints; ++i)
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
                //Debug.LogError("Appel Paul, il saura pourquoi ca bug");
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

        /*public void GeneratePossibleSpellRange(Astar3DButton targetNode, int range)
        {
            Debug.Log(targetNode);

            var path = ConstantPath.Construct(targetNode.transform.position, (range * 3) * 1000 + 1); ;
            if (range < 5)
            {
                path = ConstantPath.Construct(targetNode.transform.position, (range * 3) * 1000 + 1);
            }
            else if (range < 7)
            {
                path = ConstantPath.Construct(targetNode.transform.position, (range * 3 - 1) * 1000 + 1);
            }
            else if (range < 9)
            {
                path = ConstantPath.Construct(targetNode.transform.position, (range * 3 - 2) * 1000 + 1);
            }
            else if (range < 11)
            {
                path = ConstantPath.Construct(targetNode.transform.position, (range * 3 - 3) * 1000 + 1);
            }
            foreach (var node in path.allNodes)
            {
                if (node != path.startNode)
                {
                    // Create a new node prefab to indicate a node that can be reached
                    // NOTE: If you are going to use this in a real game, you might want to
                    // use an object pool to avoid instantiating new GameObjects all the time
                    var go = GameObject.Instantiate(m_movementNode, (Vector3)node.position, Quaternion.identity) as GameObject;
                    possibleMoves.Add(go);

                    go.GetComponent<Astar3DButton>().node = node;
                }
            }
        }*/
    }
}
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

/*public State state = State.SelectUnit;

public enum State {
    SelectUnit,
    SelectTarget,
    Attack,
    Move
}*/
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
