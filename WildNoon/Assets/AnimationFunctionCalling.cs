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


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(other.GetComponentInParent<UnitCara>() == Player._onActiveUnit)
            {
                Player.OnTurnPassed();
            }
            other.GetComponentInParent<UnitCara>().OnTakingDamage(10000);
        }
    }

}
