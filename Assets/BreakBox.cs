using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBox : MonoBehaviour
{
    public float hp; 
    public GameObject Destroyed; 

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Shoot")
        {
            hp--;
        }
    }

    private void Update()
    {
        if (hp == 0)
        {
            Instantiate(Destroyed, transform.position, transform.rotation);
            Destroy(gameObject); 
        }
    }
}
