using System;
using System.Collections;
using Game;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerModel : ActorModel
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float hitForce;
    
    [SerializeField] private PlayerRendering compRendering;
    [SerializeField] protected GameObject meleeComponent;
	
	
    private Vector2 movementInput;
    private int maxExperience = 100;
    private int currentExp;
    [SerializeField] private StatusBar statusBar;
    [SerializeField] private bool isJumping;
    [SerializeField] private bool isAttacking;
    [SerializeField] private bool isChallenging;
    [SerializeField] private bool isHit;
    [SerializeField] private bool isWalking;

    private void Start()
    {
        currentExp = 0;
        isWalking = false;
        statusBar.SetMaxExperience(maxExperience);
        
    }

    void FixedUpdate()
    {
        Vector2 velocity = rb.velocity;
        velocity.x = movementInput.normalized.x * speed;
        if (!isChallenging && !isHit)
        {
            rb.velocity = velocity;  
                    
        }
        else
        {
            if (isChallenging)
            {
                rb.velocity = new Vector2(0,rb.velocity.y);    
            }
            
        }
        
        
        if (rb.velocity.x != 0)
        {
            // Si no estaba caminando antes, empezar a reproducir el sonido
            if (!isWalking)
            {
                Feedback.Do(eFeedbackType.WalkGravel);
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
                Feedback.Stop(eFeedbackType.WalkGravel);
                isWalking = false;
            }
            compRendering.PlayAnimation(eAnimation.Idle);
        }
    }

    public void PlusExp(int expPlus)
    {
        int lastCurrentExp;
        lastCurrentExp = currentExp;
        currentExp += expPlus;
        statusBar.SetExperience(currentExp,lastCurrentExp);
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            isJumping = false;
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.gameObject.tag == "Challenge")
        {
            Zone zone = other.transform.parent.GetComponent<Zone>();
            isChallenging = true;
            other.gameObject.GetComponent<SpriteRenderer>().enabled=true;
            
            Feedback.Do(eFeedbackType.StartChallenge);
            other.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            
            GameController.Instance.StartChallenge(zone);
        }
        else
        {
            if (other.gameObject.tag == "MeleeEnemy")
            {
                
                Debug.Log("Hit Player");
                Vector2 direction = new Vector2(transform.position.x-other.transform.position.x ,  0).normalized;
                // Aquí activas la animación de "hit" en el player
                compRendering.PlayAnimation(eAnimation.Hit);
                this.lastDirection = GetDirectionFromVector(direction*-1);
                isHit = true;
                Debug.Log(direction);
                rb.AddForce(hitForce*direction);
                StartCoroutine(HitEnd());
            }
        }
    }

    public void OnMovement(InputAction.CallbackContext value)
    {
        movementInput = value.ReadValue<Vector2>();
        
    }
    public void OnJump(InputAction.CallbackContext value)
    {
        if (isHit) return;
        if (isChallenging) return;
        if (isJumping) return;
        if (isAttacking) return;

        if (value.ReadValueAsButton())
        {
            compRendering.PlayAnimation(eAnimation.Jump);
            isJumping = true;
            rb.AddForce(Vector2.up * jumpForce);
            
        }

    }
    public void OnAttack(InputAction.CallbackContext value)
    {
        if (isHit) return;
        if (isChallenging) return;
        if (isAttacking) return;
        if (isJumping) return;
        if (value.ReadValueAsButton())
        {
            
            compRendering.PlayAnimation(eAnimation.Attack);
            isAttacking = true;
            StartCoroutine(AttackEnd());
        }

    }
    private IEnumerator AttackEnd()
    {
        yield return new WaitForSeconds(0.1f);
        meleeComponent.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        meleeComponent.SetActive(false);
        yield return new WaitForSeconds(0.3f);
        isAttacking = false;
    }
    private IEnumerator HitEnd()
    {
        yield return new WaitForSeconds(0.5f);
        isHit = false;
    }

    public bool IsJumping => isJumping;
    public bool IsAttacking => isAttacking;
    public bool IsHit => isHit;

}
