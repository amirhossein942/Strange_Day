using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detection : MonoBehaviour
{
    
    public List<Collider2D> detectedList = new List<Collider2D> ();

   public  Collider2D col;

    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            //Debug.Log("Hello");
            return;
        }
        else if (collision.tag == ("Player")) {

            //Debug.Log("Bye");
            detectedList.Add (collision);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {

            return;
        }

        else if (collision.tag == "Player") {
        
            detectedList.Remove (collision);    
        }
    }
   

}
