using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjectHorizontal : MonoBehaviour
{
    public float amplitude = 2.0f;  // Amplitud del movimiento (cuánto se mueve hacia arriba y hacia abajo)
    public float frequency = 1.0f;  // Frecuencia del movimiento (qué tan rápido se mueve)

    private Vector3 startPosition;  // La posición inicial de la plataforma


    void Start()
    {
        startPosition = transform.position; // Guardar la posición inicial
    }

    void Update()
    {
        // Calculamos el nuevo desplazamiento vertical usando una función sinusoidal
        float newX = startPosition.x + Mathf.Sin(Time.time * frequency) * amplitude;

        // Actualizamos la posición de la plataforma con el nuevo desplazamiento vertical
        transform.position = new Vector3(newX, startPosition.y, startPosition.z);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
