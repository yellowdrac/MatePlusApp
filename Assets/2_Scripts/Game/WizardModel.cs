using UnityEngine;
using System.Collections;
using Game;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class WizardModel : MonoBehaviour
{
    
    [SerializeField] private WizardRendering compRendering;
    
    
    public void StartAnim(string anim)
    {
        if (anim == "revive")
        {
            Debug.Log("He ejecutado revivir");
            compRendering.PlayAnimation(eAnimation.Revive);    
        }
        
    }
    
    protected eDirection lastDirection = eDirection.Right;
    protected eDirection direction = eDirection.Right;
    
    public eDirection LastDirection => lastDirection;
    public eDirection Direction => direction;
}