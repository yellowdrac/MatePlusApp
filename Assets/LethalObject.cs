using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LethalObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.gameObject.tag == "Player")
        {
               
            Feedback.Do(eFeedbackType.DeathScream);
            
            other.gameObject.GetComponent<PlayerModel>().PlayDeath();
            
            other.gameObject.GetComponent<PlayerModel>().Death=true;
            other.gameObject.GetComponent<CapsuleCollider2D>().enabled=false;
            // other.gameObject.GetComponent<Rigidbody2D>().gravityScale=0.1f;
            
            //other.gameObject.GetComponent<PlayerModel>().PlayDeath();
        }
    }
}
