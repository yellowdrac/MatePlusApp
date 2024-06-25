using Game;
using PowerTools;
using UnityEngine;

public class ActorRendering : MonoBehaviour
{
    [SerializeField] protected ActorModel model;
	
    [Header("INFO")] 
    [SerializeField] protected eAnimation currentAnimation;

    [Header("COMPONENTS")]
    [SerializeField] protected SpriteRenderer compRnd;
    [SerializeField] protected SpriteAnim compAnim;
    
    [Header("ANIMATIONS")]
    [SerializeField] protected AnimationData animData;

    public void ChangeAnimationData(AnimationData newAnimData)
    {
        animData = newAnimData;
    }

    public void Show()
    {
        compRnd.ShowSprite();
    }

    public void Hide()
    {
        compRnd.HideSprite();
    }
    
    protected bool ShouldSkipAnimationSkel(eAnimation nextAnimation, bool conditions)
    {
        return (conditions || (currentAnimation == nextAnimation && model.LastDirection == model.Direction));
    }

    protected bool ShouldSkipAnimation(eAnimation nextAnimation, bool conditions, bool isAttacking, bool isDeath, bool isJumping, bool isHit)
    {
        // Prioridad de animaciones
        if (isDeath)
            return nextAnimation != eAnimation.Death;

        if (isHit)
            return nextAnimation != eAnimation.Hit && nextAnimation != eAnimation.Death;

        if (isJumping)
            return nextAnimation != eAnimation.Jump && nextAnimation != eAnimation.Death && nextAnimation != eAnimation.Hit;

        if (isAttacking)
            return nextAnimation != eAnimation.Attack && nextAnimation != eAnimation.Death && nextAnimation != eAnimation.Hit && nextAnimation != eAnimation.Jump;
  
        // Verifica si debe saltar la animación basada en las condiciones originales
        return conditions || (currentAnimation == nextAnimation && model.LastDirection == model.Direction);
    }
    public eAnimation GetCurrentAnimation()
    {
        return currentAnimation;
    }

    protected void SetOrientation(eDirection dir)
    {
        
        bool left = (dir == eDirection.Left);
        
        compRnd.flipX = left;
    }
}