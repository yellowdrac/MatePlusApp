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
    
    protected bool ShouldSkipAnimation(eAnimation nextAnimation, bool conditions)
    {
        return (conditions || (currentAnimation == nextAnimation && model.LastDirection == model.Direction));
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