using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerPlatform : MonoBehaviour
{
    [SerializeField] private bool invisible = true;
    private Vector3 startPosition;  // La posición inicial de la plataforma
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (invisible)
            {
        
        
                this.GetComponent<Rigidbody2D>().gravityScale = 2;    
            }
            
            
        }
        
        
    }
    
}
