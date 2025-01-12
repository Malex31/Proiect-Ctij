using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Mechanics;

public class CheckPoint: MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            Debug.Log("Player entered checkpoint zone.");
            PlayerController.lastCheckPointPos = new Vector3(transform.position.x, transform.position.y, 0);
           

            Debug.Log("Checkpoint set to: " + PlayerController.lastCheckPointPos);
            GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
}
