using UnityEngine;
using System.Collections;
public class BlockRotateSand : MonoBehaviour
{
    public float RotationSpeed = 290f; // Velocidad de rotación en grados por segundo
    public int idBlock;
    [SerializeField] public bool isRotating;
    private float targetRotation;
    private Quaternion startRotation;
    private Quaternion endRotation;

    void Start()
    {
        isRotating = false;
        StartCoroutine(RotateRoutine());
    }

    IEnumerator RotateRoutine()
    {
        if (idBlock == 2)
        {
            yield return new WaitForSeconds(5f);    
        }
        
        while (true)
        {
            if (idBlock == 3)
            {
                // Rotar continuamente en una dirección
                yield return RotateContinuously();
            }
            else
            {
                // Rotar 180 grados en una dirección
                targetRotation = 180f;
                yield return RotateToTarget(targetRotation);
                // Esperar 10 segundos
                if (idBlock == 2)
                {
                    yield return new WaitForSeconds(3f);    
                }
                if (idBlock == 0)
                {
                    yield return new WaitForSeconds(3f);    
                }
                
                // Rotar 180 grados en la dirección opuesta
                targetRotation = -180f;
                yield return RotateToTarget(targetRotation);
                // Esperar 10 segundos
                if (idBlock == 2)
                {
                    yield return new WaitForSeconds(28f);    
                }
                if (idBlock == 0)
                {
                    
                    yield return new WaitForSeconds(10f);    
                }
            }
        }
    }
    IEnumerator RotateContinuously()
    {
        float rotationSpeedCont = 200f;
        isRotating = true;
        while (idBlock == 3)
        {
            transform.Rotate(Vector3.back, rotationSpeedCont * Time.deltaTime);
            yield return null;
        }
        isRotating = false;
    }
    IEnumerator RotateToTarget(float targetRotation)
    {
        isRotating = true;

        startRotation = transform.rotation;
        endRotation = Quaternion.Euler(0, 0, transform.eulerAngles.z + targetRotation);

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * RotationSpeed / 180f;
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, t);
            yield return null;
        }

        transform.rotation = endRotation;
        isRotating = false;
    }
}