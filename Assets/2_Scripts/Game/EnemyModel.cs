using System;
using System.Collections;
using Game;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;

public class EnemyModel : ActorModel
    {
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;

    [SerializeField] private EnemyRendering compRendering;
    [SerializeField] protected GameObject meleeComponent;

    private eEnemyType enemyType;
    private Vector2 movementInput;
    
    
    [SerializeField] private StatusBar statusBar;
    [SerializeField] private bool isJumping;
    [SerializeField] private bool isAttacking;
    [SerializeField] private bool isChallenging;
    [SerializeField] private bool isWalking;
    [SerializeField] private PlayerModel player;
    private float distance;
    private void Start()
    {
        
        
    }
    void FixedUpdate()
    {
        
        if (player!=null)
        {
            distance = Vector2.Distance(transform.position, player.transform.position);
            Vector2 direction = new Vector2(player.transform.position.x - transform.position.x,  0);
            
            direction.Normalize();
            
            if (distance < 5)
            {
                if (distance < 2)
                {
                    
                    compRendering.PlayAnimation(eAnimation.Attack);
                    isAttacking = true;
                    StartCoroutine(AttackEnd());
                    rb.velocity = Vector2.zero; 
                }
                else
                {
                    rb.velocity = speed*direction;
                }
            }
            else
            { 
                rb.velocity = Vector2.zero; 
            }
        }
        
        if (rb.velocity.x != 0)
        {
            // Si no estaba caminando antes, empezar a reproducir el sonido
            if (!isWalking)
            {
                //Feedback.Do(eFeedbackType.WalkGravel);
                isAttacking = false;
                isWalking = true;
            }
            SetDirection(GetDirectionFromVector(rb.velocity));
            meleeComponent.transform.SetScaleX(GetDirectionFromVector(rb.velocity)==eDirection.Right?1:-1);
            compRendering.PlayAnimation(eAnimation.Walk);
        }
        else
        {
            // Si estaba caminando pero dejó de hacerlo, detener el sonido
            if (isWalking)
            {
                //Feedback.Stop(eFeedbackType.WalkGravel);
                isWalking = false;
            }
            SetDirection(GetDirectionFromVector(rb.velocity));
            compRendering.PlayAnimation(eAnimation.Idle);
        }
    }

    public void SetEnemyType(eEnemyType enemyType)
    {
        this.enemyType = enemyType;
    }
    public void SetPlayer(PlayerModel player)
    {
        this.player = player;
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
       
        
    }
    private IEnumerator AttackEnd()
    {
        yield return new WaitForSeconds(0.6666f);
        meleeComponent.SetActive(true);
        yield return new WaitForSeconds(0.6666f);
        meleeComponent.SetActive(false);
        yield return new WaitForSeconds(0.3333f);
        meleeComponent.SetActive(true);
        yield return new WaitForSeconds(0.6666f);
        meleeComponent.SetActive(false);
        yield return new WaitForSeconds(0.4999f);
        isAttacking = false;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        
        
    }

    public void OnMovement(InputAction.CallbackContext value)
    {
        
        
    }
    public void OnJump(InputAction.CallbackContext value)
    {
        

    }
    public void OnAttack(InputAction.CallbackContext value)
    {
       

    }
    
    public bool IsJumping => isJumping;
    public bool IsAttacking => isAttacking;
    public EnemyRendering EnemyRendering => compRendering;
    
}
