using UnityEngine;
using System.Collections;

public class RotateFireBall : MonoBehaviour
{
    public float rotationSpeed = -290f; // Velocidad de rotación en grados por segundo
    [SerializeField] public bool isRotating;

    void Start()
    {
        isRotating = false;
        StartCoroutine(RotateContinuously());
    }

    IEnumerator RotateContinuously()
    {
        isRotating = true;
        while (true)
        {
            transform.Rotate(Vector3.back, rotationSpeed * Time.deltaTime);
            NormalizeRotation();
            yield return null;
        }
    }

    void NormalizeRotation()
    {
        // Normalizar la rotación en el rango [0, 360)
        float zRotation = transform.eulerAngles.z;
        zRotation = Mathf.Repeat(zRotation, 360f);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, zRotation);
    }
}