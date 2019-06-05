using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class AnimationFunctionCalling : MonoBehaviour
{
    PlayerManager Player;
    SingleNodeBlocker blocker;

    int i =0;
    private void Awake()
    {
        Player = FindObjectOfType<PlayerManager>();
        blocker = GetComponent<SingleNodeBlocker>();

    }

    public void OnTrainAnimEnd()
    {
        Player.OnPlayerIsDisabled(false);
        if(i == 0)
        {
            blocker.BlockAtCurrentPosition();
            i++;
        }
        else if(i == 1)
        {
            blocker.Unblock();
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
