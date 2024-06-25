using System;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText; // Asigna este en el inspector
    private float timeRemaining; // Tiempo en segundos
    private bool timerIsRunning = false;
    private bool isPaused = false;

    private void Start()
    {
        // Inicializar el temporizador
        
    }

    public float getTimer()
    {
        return timeRemaining;
    }
    private void Update()
    {
        
        // Actualizar el temporizador en cada frame
        if (timerIsRunning && !isPaused)
        {
            if (timeRemaining >= 0)
            {
                timeRemaining += Time.deltaTime; // Sumar el tiempo en lugar de restarlo
                UpdateTimerText();
            }
        }
    }

    public void StartTimer(float duration)
    {
        Debug.Log(duration);
        timeRemaining = 0; // Iniciar desde cero
        timerIsRunning = true;
        isPaused = false;
        UpdateTimerText(); // Actualizar el texto del temporizador al iniciar
    }
    public void PauseTimer()
    {
        isPaused = true;
    }

    public void ResumeTimer()
    {
        isPaused = false;
    }

    private void UpdateTimerText()
    {
        timerText.text = FormatTime(timeRemaining);
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}