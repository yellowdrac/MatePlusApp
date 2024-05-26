using Game;
using System.Collections.Generic;
using PowerTools;
using UnityEngine;

public class IceRendering : MonoBehaviour
{
    [SerializeField] protected IceModel modelIceHighFirst;
    [SerializeField] protected IceModel modelIceHighLast;
	
    [Header("INFO")] 
    [SerializeField] protected eAnimation currentAnimation;

    [Header("COMPONENTS")]
    [SerializeField] protected SpriteAnim compAnimIceHighMeltFirst;
    [SerializeField] protected SpriteAnim compAnimIceHighMeltLast;
    [SerializeField] protected List<SpriteAnim> compAnimIceMelt;
    
    
    [Header("ANIMATIONS")]
    [SerializeField] protected AnimationData animData;
    [SerializeField] protected AnimationData animDataIce;

    
    protected bool ShouldSkipAnimation(eAnimation nextAnimation, bool conditions)
    {
        return (conditions || (currentAnimation == nextAnimation && modelIceHighFirst.LastDirection == modelIceHighFirst.Direction));
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
            case eAnimation.IceMelt:
                foreach (var anim in compAnimIceMelt)
                {
                    anim.Play(animDataIce.animations[nextAnimation].animations[modelIceHighFirst.LastDirection]);
                }
                break;
            case eAnimation.IceMeltHigh:
                compAnimIceHighMeltLast.Play(animData.animations[nextAnimation].animations[modelIceHighLast.LastDirection]);
                compAnimIceHighMeltFirst.Play(animData.animations[nextAnimation].animations[modelIceHighFirst.LastDirection]);
                break;
        }
    }

}