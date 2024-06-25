using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class Portal : MonoBehaviour
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
        if (other.gameObject.tag=="Player")
        {
            Feedback.Stop(eFeedbackType.WalkGravel);
            Feedback.Stop(eFeedbackType.WalkSnow);
            Feedback.Stop(eFeedbackType.WalkGrass);
            Feedback.Stop(eFeedbackType.WalkSand);
            Debug.Log("Compare Player");
            Zone currentZone = transform.parent.GetComponent<Zone>();
            if (currentZone != null)
            {
                Feedback.Do(eFeedbackType.PortalPassed);
                Debug.Log("Tratando de ocultar zona");
                currentZone.ShowEnvEffects();
                currentZone.ShowZone();
                if (currentZone.ZoneID == 4)
                {
                    Feedback.Stop(eFeedbackType.SnowZone5Ambient);
                    other.gameObject.GetComponent<PlayerModel>().IsSoundAmbient = false;
                }
            }

            other.gameObject.GetComponent<PlayerModel>().StoredX = other.transform.position.x+5;

            // Añadir lógica adicional si es necesario
        }
    }
}
