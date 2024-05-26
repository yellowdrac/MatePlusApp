using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class StatusBar : MonoBehaviour
{
    private float expIncreaseInterval = 1f; // Intervalo de aumento de experiencia en segundos
    
    public Slider slider;
    [SerializeField] public TextMeshProUGUI textComponent;
    // Start is called before the first frame update
    public void SetExperience(int exp, int lastExp)
    {
        StartCoroutine(IncreaseExperienceGradually(exp,lastExp));
    }
    private IEnumerator IncreaseExperienceGradually(int expToAdd, int lastExp)
    {
        float elapsedTime = 0f;
        float timeToIncrease = expIncreaseInterval / expToAdd; // Tiempo para aumentar 1 punto de experiencia

        while (lastExp < expToAdd)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= timeToIncrease)
            {
                lastExp++;
                slider.value = lastExp;

                // Si alcanza el nivel máximo, reiniciar la experiencia y actualizar el valor de la barra
                if (lastExp >= 100)
                {
                    expToAdd -= 100;
                    lastExp = 0;
                    slider.value = 0;
                }

                elapsedTime = 0f;
            }

            yield return null;
        }
    }

    public void SetMaxExperience(int exp)
    {
        slider.maxValue = exp;
        slider.value = 0;
    }
    public void SetLevel(int level)
    {
        textComponent.text = level.ToString();
    }
}
