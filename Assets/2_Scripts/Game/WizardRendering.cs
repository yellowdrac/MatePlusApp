using Game;
using PowerTools;
using UnityEngine;

public class WizardRendering : MonoBehaviour
{
    [SerializeField] protected WizardModel model;
	
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
            case eAnimation.Revive:
                compAnim.Play(animData.animations[nextAnimation].animations[model.LastDirection]);
                break;
        }
    }

}