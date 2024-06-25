using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerTools;


public class PlayerRendering : ActorRendering
{
    private PlayerModel playerModel;
    private void Awake()
    {
	    playerModel = (PlayerModel) model;
    }
    public void PlayAnimation(eAnimation nextAnimation)
    {
	    

	    bool isAttacking = playerModel.IsAttacking;
	    bool isDeath = playerModel.IsDeath;
	    bool isJumping = playerModel.IsJumping;
	    bool isHit = playerModel.IsHit;
    
	    bool conditionsToSkip = isAttacking || isDeath || isJumping || isHit;
	    

	    if (ShouldSkipAnimation(nextAnimation, conditionsToSkip, isAttacking, isDeath, isJumping, isHit)) return;

	    currentAnimation = nextAnimation;
	    switch (nextAnimation)
	    {
		    case eAnimation.Idle:
			    SetOrientation(model.LastDirection);
			    compAnim.Play(animData.animations[nextAnimation].animations[model.LastDirection]);
			    break;
		    case eAnimation.Walk:
			    SetOrientation(model.Direction);
			    compAnim.Play(animData.animations[nextAnimation].animations[model.Direction]);
			    break;
		    case eAnimation.Jump:
			    SetOrientation(model.LastDirection);
			    compAnim.Play(animData.animations[nextAnimation].animations[model.LastDirection]);
			    break;
		    case eAnimation.Death:
			    SetOrientation(model.LastDirection);
			    compAnim.Play(animData.animations[nextAnimation].animations[model.LastDirection]);
			    break;
		    case eAnimation.Attack:
			    SetOrientation(model.LastDirection);
			    compAnim.Play(animData.animations[nextAnimation].animations[model.LastDirection]);
			    break;
		    case eAnimation.Hit:
			    Debug.Log("Current Direction: " + model.LastDirection);
			    SetOrientation(model.LastDirection);
			    compAnim.Play(animData.animations[nextAnimation].animations[model.LastDirection]);
			    break;
	    }
    }

}
