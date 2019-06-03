using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationFunctionCalling : MonoBehaviour
{
    PlayerManager Player;

    private void Awake()
    {
        Player = FindObjectOfType<PlayerManager>();
    }

    public void OnTrainAnimEnd()
    {
        Player.OnPlayerIsDisabled(false);
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
