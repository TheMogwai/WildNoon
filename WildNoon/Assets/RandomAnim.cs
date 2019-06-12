using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAnim : MonoBehaviour
{
    public float _minTimeBeforeIdleBonus;
    public float _maxTimeBeforeIdleBonus;
    Animator anim;

    void Start()
    {
        StartCoroutine(animRandom());
        anim = GetComponent<Animator>();
    }

    IEnumerator animRandom()
    {
        float randomTime = Random.Range(_minTimeBeforeIdleBonus, _maxTimeBeforeIdleBonus);
        yield return new WaitForSeconds(randomTime);
        anim.SetTrigger("Search");
        StartCoroutine(animRandom());
    }
}
