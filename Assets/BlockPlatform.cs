using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPlatform : MonoBehaviour
{
    public float amplitude = 2.0f;  // Amplitud del movimiento (cuánto se mueve hacia arriba y hacia abajo)
    public float frequency = 1.0f;  // Frecuencia del movimiento (qué tan rápido se mueve)

    private Vector3 startPosition;  // La posición inicial de la plataforma

    // Start is called before the first frame update
    void Start()
    {
        // Guardamos la posición inicial de la plataforma
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Calculamos el nuevo desplazamiento vertical usando una función sinusoidal
        float newY = startPosition.y + Mathf.Sin(Time.time * frequency) * amplitude;

        // Actualizamos la posición de la plataforma con el nuevo desplazamiento vertical
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
}
