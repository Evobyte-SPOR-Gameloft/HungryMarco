using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFollow : MonoBehaviour
{
    private Transform player;

    private Vector3 tempPos;

    private Camera zoomCamera;

    [SerializeField] private float zoomScrollSpeed = 1f;
    [SerializeField] private float maxZoom = 8f;
    [SerializeField] private float minZoom = 1f;

    void Start()
    {
        if(GameObject.FindWithTag("Player") != null) player = GameObject.FindWithTag("Player").transform;

        zoomCamera = Camera.main;
    }

    private void Update()
    {
        //Zoom in and out with scroll wheel
        zoomCamera.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * zoomScrollSpeed;
        zoomCamera.orthographicSize = Mathf.Clamp(zoomCamera.orthographicSize, minZoom, maxZoom);
    }


    void LateUpdate()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
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
