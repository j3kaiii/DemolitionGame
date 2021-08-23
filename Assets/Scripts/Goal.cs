using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    static public bool goalMet = false;

    /*private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Projectile")
        {
            Debug.Log("Target destroyed");
            goalMet = true;
            Material mat = GetComponent<SpriteRenderer>().material;
            if (mat != null) Debug.Log("material found");
            Color c = mat.color;
            Debug.Log(c);
            c.a = 0;
            mat.color = c;
        }
    }*/

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Projectile")
        {
            goalMet = true;
            Destroy(this.gameObject);
        }
    }
}
