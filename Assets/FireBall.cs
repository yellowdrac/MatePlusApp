using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public float amplitude = 2.0f;  // Amplitud del movimiento (cuánto se mueve hacia arriba y hacia abajo)
    public float frequency = 1.0f;  // Frecuencia del movimiento (qué tan rápido se mueve)

    private Vector3 startPosition;  // La posición inicial de la plataforma
    private bool hasRotated = false; // Bandera para saber si ya hemos rotado al máximo de amplitud

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

        // Detectar los puntos máximos y mínimos de amplitud
        if (Mathf.Abs(newY - (startPosition.y + amplitude)) < 0.01f && !hasRotated)
        {
            // Rotar 180 grados en el eje X
            transform.Rotate(-180, 0, 0);
            hasRotated = true; // Marcamos que ya hemos rotado en el punto máximo
        }
        else if (Mathf.Abs(newY - (startPosition.y - amplitude)) < 0.01f && hasRotated)
        {
            // Rotar -180 grados en el eje X
            transform.Rotate(180, 0, 0);
            hasRotated = false; // Marcamos que ya hemos rotado en el punto mínimo
        }
    }
}
