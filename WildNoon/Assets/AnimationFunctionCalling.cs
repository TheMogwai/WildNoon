using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class AnimationFunctionCalling : MonoBehaviour
{
    PlayerManager Player;
    public GameObject blockerParent;
    SingleNodeBlocker[] blocker;

    int i =0;
    private void Awake()
    {
        Player = FindObjectOfType<PlayerManager>();
        blocker = blockerParent.GetComponentsInChildren<SingleNodeBlocker>();
        CountTeam1 = 0;
        CountTeam2 = 0;
    }

    public void OnTrainAnimEnd()
    {
        Player.OnPlayerIsDisabled(false);
        if(i == 0)
        {
            for (int i = 0, l = blocker.Length; i < l; ++i)
            {
                blocker[i].BlockAtCurrentPosition();
            }
            i++;
        }
        else if(i == 1)
        {
            for (int i = 0, l = blocker.Length; i < l; ++i)
            {
                blocker[i].Unblock();
            }
            i = 0;
        }
        
    }
    int CountTeam1;
    int CountTeam2;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(other.GetComponentInParent<UnitCara>() == Player._onActiveUnit)
            {
                if (!other.GetComponentInParent<UnitCara>().IsTeam2)
                {
                    CountTeam1++;
                    Debug.Log(CountTeam1);
                }
                else
                {
                    CountTeam2++;
                }
                if (CountTeam1 == 1 || CountTeam2 == 1)
                {
                    StartCoroutine(checkTimer(0f));
                }
                else if (CountTeam1 == 2 || CountTeam2 == 2)
                {
                    StartCoroutine(checkTimer(0.25f));
                }
                else if (CountTeam1 == 3 || CountTeam2 == 3)
                {
                    StartCoroutine(checkTimer(0.5f));
                }
                /*else if (CountTeam1 < 4 && CountTeam2 < 4)
                {

                    StartCoroutine(checkTimer(2.5f));

                    //Player.OnTurnPassed();
                }*/
            }
            other.GetComponentInParent<UnitCara>().OnTakingDamage(10000);
        }
    }

    IEnumerator checkTimer(float f)
    {
        yield return new WaitForSeconds(f);
        Player.OnTurnPassed();

    }
}
