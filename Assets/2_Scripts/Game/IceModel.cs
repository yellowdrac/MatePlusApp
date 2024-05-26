using UnityEngine;
using System.Collections;
using Game;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class IceModel : MonoBehaviour
{
    
    [SerializeField] private IceRendering compRendering;
    
    
    public void StartAnim(string anim)
    {
        if (anim == "meltHigh")
        {
            Debug.Log("He ejecutado iceMeltHigh");
            compRendering.PlayAnimation(eAnimation.IceMeltHigh);    
        }
        if (anim == "melt")
        {
            Debug.Log("He ejecutado iceMelt");
            compRendering.PlayAnimation(eAnimation.IceMelt);    
        }
        
    }
    
    protected eDirection lastDirection = eDirection.Right;
    protected eDirection direction = eDirection.Right;
    
    public eDirection LastDirection => lastDirection;
    public eDirection Direction => direction;
}