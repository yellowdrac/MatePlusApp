using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSand : MonoBehaviour
{
    public float radius = 2.0f;    // Radio del círculo
    public float speed = 1.0f;     // Velocidad del movimiento circular

    private Vector3 centerPosition; // Centro del círculo

    // Start is called before the first frame update
    void Start()
    {
        // Guardamos la posición inicial del objeto como el centro del círculo
        centerPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Calculamos el nuevo ángulo de rotación usando el tiempo transcurrido y la velocidad
        float angle = Time.time * speed;

        // Calculamos la nueva posición usando funciones trigonométricas
        float newX = centerPosition.x + Mathf.Cos(angle) * radius;
        float newY = centerPosition.y + Mathf.Sin(angle) * radius;

        // Actualizamos la posición del objeto
        transform.position = new Vector3(newX, newY, transform.position.z);
    }
}
