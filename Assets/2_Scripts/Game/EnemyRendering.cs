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
        bool conditionsToSkip = enemyModel.IsAttacking || enemyModel.IsJumping;

        if (ShouldSkipAnimation(nextAnimation, conditionsToSkip)) return;

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
            case eAnimation.Attack:
                SetOrientation(model.LastDirection);
			
                compAnim.Play(animData.animations[nextAnimation].animations[model.LastDirection]);
                break;
		
        }
    }
}
