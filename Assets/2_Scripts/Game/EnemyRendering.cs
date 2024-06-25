using UnityEngine;

public class EnemyRendering : ActorRendering
{
    private EnemyModel enemyModel;
    
    private void Awake()
    {
        enemyModel = (EnemyModel) model;
    }
    
    public void PlayAnimation(eAnimation nextAnimation)
    {
        bool isAttacking = enemyModel.IsAttacking;
        bool isDeath = enemyModel.IsDeath;
        bool isJumping = enemyModel.IsJumping;
        bool isHit = enemyModel.IsHit;
    
        
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
                //SetOrientation(model.LastDirection);
                compAnim.Play(animData.animations[nextAnimation].animations[model.LastDirection]);
                break;
		
        }
    }
}
