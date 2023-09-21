using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            float interactRange = 2f;
            Collider2D[] collider2dArray = Physics2D.OverlapCircleAll(transform.position, interactRange);
        }
    }
}
