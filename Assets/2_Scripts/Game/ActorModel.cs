using UnityEngine;
using System.Collections;
using Game;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class ActorModel : MonoBehaviour
{
    
    [SerializeField] protected Rigidbody2D rb;
    
	protected eDirection lastDirection = eDirection.Right;
	protected eDirection direction = eDirection.Right;
    

    public void SetDirection(eDirection dir)
    {
        lastDirection = direction;
        direction = dir;
    }
    		
    protected eDirection GetDirectionFromVector(Vector2 vector)
    {
    	float angle = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;

    	if (angle > -90 && angle < 90)
    	{
    		return eDirection.Right;
    	}
    	else
    	{
    		return eDirection.Left;
    	}
    }
    
	public eDirection LastDirection => lastDirection;
	public eDirection Direction => direction;
}
