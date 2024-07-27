using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform Player;
    public Transform LookDirection;

    public void StartFollow(Transform target)
    {
        Player = target;
        LookDirection = target.Find("LookDirection");
    }

    public void StopFollow()
    {
        Player = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(Player == null) {
            return;
        }

        transform.position = Player.position;
        transform.rotation = LookDirection.rotation;
    }
}
