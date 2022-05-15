using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform player;

    private Vector3 tempPos;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }



    void LateUpdate()
    {
        tempPos = transform.position;

        if (player != null)
        {
            tempPos.x = player.position.x;
            tempPos.y = player.position.y;
        }

        transform.position = tempPos;
    }



}//class
