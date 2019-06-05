using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class TrainBlocker : MonoBehaviour
{
    SingleNodeBlocker blocker;

    bool open;

    void Awake()
    {
        blocker = GetComponent<SingleNodeBlocker>();
    }

    void Start()
    {
        // Make sure the door starts out blocked
        blocker.BlockAtCurrentPosition();
    }

    public void Close()
    {
        StartCoroutine(WaitAndClose());
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
        }

        while (blocker.manager.NodeContainsAnyExcept(node, selector))
        {
            yield return null;
        }

        open = false;
        blocker.BlockAtCurrentPosition();
    }

    public void Open()
    {
        // Stop WaitAndClose if it is running
        StopAllCoroutines();

        // Play the open door animation
        open = true;

        // Unblock the door node so that units can traverse it again
        blocker.Unblock();
    }
}
