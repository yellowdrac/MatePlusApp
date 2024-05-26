using UnityEngine;
using System.Collections;
using Game;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class SmokeModel : MonoBehaviour
{
    
    [SerializeField] private SmokeRendering compRendering;
    
    
    public void StartAnim(string anim)
    {
        if (anim == "dissappear")
        {
            Debug.Log("He ejecutado dissapear");
            compRendering.PlayAnimation(eAnimation.Dissapear);    
        }
        else
        {
            if (anim == "explote")
            {
                Debug.Log("He ejecutado explote");
                compRendering.PlayAnimation(eAnimation.Explote);
            }
        }
        
    }
    
    protected eDirection lastDirection = eDirection.Right;
    protected eDirection direction = eDirection.Right;
    
    public eDirection LastDirection => lastDirection;
    public eDirection Direction => direction;
}