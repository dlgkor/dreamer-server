using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject Player;
    public GameObject LookDirection;

    public void StartFollow(GameObject target)
    {
        Player = target;
        LookDirection = target.transform.GetChild(0).gameObject;
    }

    public void StopFollow()
    {
        Player = null;
        LookDirection = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (Player == null || LookDirection == null)
        {
            return;
        }

        transform.position = Player.transform.position;
        transform.rotation = LookDirection.transform.rotation;
    }
}
