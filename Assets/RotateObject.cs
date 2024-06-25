using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public GameObject emptyGameObject; // Referencia al GameObject auxiliar

    void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            // Rotar el GameObject auxiliar alrededor del eje Z
            emptyGameObject.transform.Rotate(Vector3.forward * Time.deltaTime * 90); // 90 grados por segundo
        }
    }
}
