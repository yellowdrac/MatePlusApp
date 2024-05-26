using Game;
using PowerTools;
using UnityEngine;

public class SmokeRendering : MonoBehaviour
{
    [SerializeField] protected SmokeModel model;
	
    [Header("INFO")] 
    [SerializeField] protected eAnimation currentAnimation;

    [Header("COMPONENTS")]
    [SerializeField] protected SpriteAnim compAnim;
    
    
    [Header("ANIMATIONS")]
    [SerializeField] protected AnimationData animData;

    
    protected bool ShouldSkipAnimation(eAnimation nextAnimation, bool conditions)
    {
        return (conditions || (currentAnimation == nextAnimation && model.LastDirection == model.Direction));
    }

    public eAnimation GetCurrentAnimation()
    {
        return currentAnimation;
    }
    public void PlayAnimation(eAnimation nextAnimation)
    {
        currentAnimation = nextAnimation;
        switch (nextAnimation)
        {
            case eAnimation.Dissapear:
            case eAnimation.IceMelt:
            case eAnimation.IceMeltHigh:
            case eAnimation.Explote:
                compAnim.Play(animData.animations[nextAnimation].animations[model.LastDirection]);
                break;
        }
    }

}