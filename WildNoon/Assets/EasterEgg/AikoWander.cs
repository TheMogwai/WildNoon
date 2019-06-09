using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AikoWander : MonoBehaviour
{

    public GameObject[] waypoints;
    float speed = 10f;
    int currentWaypoint;
    Vector3 _heading;
    float distanceToPlayer;
    Animator anim;
    RaycastHit ray;
    Rigidbody rb;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        StartCoroutine(LookForWayPoints());
    }


    IEnumerator LookForWayPoints()
    {
        float randomTimeOnWayPoint = Random.Range(0.5f, 2f);
        int lookForNextWayPoint = Random.Range(0, waypoints.Length);
        while (lookForNextWayPoint == currentWaypoint)
        {
            lookForNextWayPoint = Random.Range(0, waypoints.Length);
        }
        currentWaypoint = lookForNextWayPoint;
        var heading = transform.position - waypoints[currentWaypoint].transform.position;
        distanceToPlayer = heading.magnitude;

        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        if (distanceToPlayer > 20f)
        {
            anim.SetTrigger("Run");
            speed = 10f;
        }
        else
        {
            anim.SetTrigger("Walk");
            speed = 5f;
        }
        while (distanceToPlayer > 2f)
        {
            float step = speed * Time.deltaTime;
            heading = transform.position - waypoints[currentWaypoint].transform.position;
            _heading = heading;
            distanceToPlayer = heading.magnitude;
            transform.LookAt(waypoints[currentWaypoint].transform.position);
            transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypoint].transform.position, step);
            yield return new WaitForSeconds(0.02f);
        }
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ ;
        anim.SetTrigger("Idle");
        yield return new WaitForSeconds(randomTimeOnWayPoint);
        StartCoroutine(LookForWayPoints());
    }
}
