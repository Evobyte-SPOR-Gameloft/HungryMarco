using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InnerBounds : MonoBehaviour
{
    [HideInInspector]
    public BoxCollider2D innerCollider;

    void Start()
    {
        innerCollider = GetComponent<BoxCollider2D>();
    }


    void Update()
    {
        
    }

}//class
