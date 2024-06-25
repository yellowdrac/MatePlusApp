using System;
using System.Collections;
using DG.Tweening;
using Game;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;

public class EnemyModel : ActorModel
    {
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float hitForce;
    [SerializeField] private EnemyRendering compRendering;
    [SerializeField] protected GameObject meleeComponent;
    [SerializeField] private int maxHealth = 5; // Añadir salud máxima
    private int currentHealth;
    private eEnemyType enemyType;
    private Vector2 movementInput;
    
    
    [SerializeField] private StatusBar statusBar;
    [SerializeField] private bool isJumping;
    [SerializeField] private bool isAttacking;
    [SerializeField] private bool isChallenging;
    [SerializeField] private bool isHit;
    [SerializeField] private bool isWalking;
    [SerializeField] private bool isDeath;
    [SerializeField] private PlayerModel player;
    private float distance;
    private void Start()
    {
        
        currentHealth = maxHealth;
    }
    void FixedUpdate()
    {
        if (isDeath || isHit)
        {
            if (isDeath)
            {
                rb.velocity = Vector2.zero;    
            }
             
            return;
        }
        
        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = new Vector2(player.transform.position.x - transform.position.x,  0);
        
        direction.Normalize();
        
        if (distance < 5)
        {
            if (distance < 2)
            {
                if (isAttacking != true)
                {
                    compRendering.PlayAnimation(eAnimation.Attack);
                    isAttacking = true;
                    StartCoroutine(AttackEnd());
                    rb.velocity = Vector2.zero;    
                }
                 
            }
            else
            {
                if (!player.IsHit && isAttacking != true)
                {
                    rb.velocity = speed*direction;    
                }
            }
        }
        else
        { 
            rb.velocity = new Vector2(0, rb.velocity.y); // Solo detener el eje X
        }
    
        
        if (rb.velocity.x != 0)
        {
            if(!player.IsHit && isAttacking != true && isHit!=true)
            {
                if (!isWalking)
                {
                    isWalking = true;
                }
                
                SetDirection(GetDirectionFromVector(rb.velocity));
                meleeComponent.transform.SetScaleX(GetDirectionFromVector(rb.velocity)==eDirection.Right?1:-1);
                compRendering.PlayAnimation(eAnimation.Walk);    
            }
        }
        else
        {
            isWalking = false;
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
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "PlayerAttack" && isHit!=true)
        {
            isAttacking = false;
            TakeDamage(1,other); // Restar 1 de vida cuando el jugador ataque}
            Debug.Log("Recibio 1 de daño");
            Debug.Log("Vida Actual es ");
            Debug.Log(currentHealth);
            
        }
        
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
    private IEnumerator HitEnd()
    {
        yield return new WaitForSeconds(0.5f);
        isHit = false;
    }
    
    private void TakeDamage(int damage,Collider2D other)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            isDeath = true;
            Die();
        }
        else
        {
            isHit = true;
            //Debug.Log("Hit Player");
            Vector2 direction = new Vector2(transform.position.x-other.transform.position.x ,  0).normalized;
            // Aquí activas la animación de "hit" en el player
            //SetDirection(GetDirectionFromVector(direction * -1));
            //Debug.Log("Hit Direction: " + GetDirectionFromVector(direction * -1));
            compRendering.PlayAnimation(eAnimation.Hit);
            
            
            //Debug.Log(direction);
            rb.AddForce(hitForce*direction);
            StartCoroutine(HitEnd());
        }
    }
    private void Die()
    {
        compRendering.PlayAnimation(eAnimation.Death);
        this.GetComponent<CapsuleCollider2D>().enabled = false;
        this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        this.gameObject.transform.GetChild(1).gameObject.SetActive(false);
        Invoke("DieDie",1.3f);
        // Implementar la lógica de muerte, como desactivar el objeto
        //gameObject.SetActive(false);
    }
    private void DieDie()
    {
        //this.gameObject.SetActive(false);
        Destroy(this.gameObject);
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
    public bool IsHit => isHit;
    public bool IsDeath => isDeath;
    public EnemyRendering EnemyRendering => compRendering;
    
}
