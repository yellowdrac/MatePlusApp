using System;
using System.Collections;
using DG.Tweening;
using Game;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.OnScreen;


public class PlayerModel : ActorModel
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float hitForce;
    
    [SerializeField] private PlayerRendering compRendering;
    [SerializeField] protected GameObject meleeComponent;

    private int zoneId;
    private Vector2 movementInput;
    private int maxExperience = 100;
    private int currentLevel;
    private int currentExp;
    
    [SerializeField] private StatusBar statusBar;
    [SerializeField] private bool isJumping;
    
    [SerializeField] private bool isAttacking;
    [SerializeField] private bool isChallenging;
    [SerializeField] private bool isHit;
    [SerializeField] private bool isDeath;
    [SerializeField] private bool isWalking;
    private bool isSoundAmbient;
    private float storedX;
    private void Start()
    {
        currentLevel = 0;
        currentExp = 0;
        
        isWalking = false;
        statusBar.SetMaxExperience(maxExperience);
        isSoundAmbient = false;
        storedX = 5;
    }

    void FixedUpdate()
    {
        if (isAttacking)
        {
            Debug.Log("Esta atacando player");
        }
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
            if(isDeath){
                isJumping=false;
                rb.velocity = new Vector2(0,0);    
            }
        }
        
        
        if (rb.velocity.x != 0)
        {
            if (isJumping)
            {
                isWalking = false;
               
                if (zoneId == 0)
                {
                    Feedback.Stop(eFeedbackType.WalkGravel);    
                }
                if (zoneId == 4)
                {
                    Feedback.Stop(eFeedbackType.WalkSnow);    
                }
                if (zoneId == 5)
                {
                    Feedback.Stop(eFeedbackType.WalkWood);    
                }
                if(zoneId==2 || zoneId==3)
                {
                    Feedback.Stop(eFeedbackType.WalkSand);   
                }
                
                if(zoneId==1)
                {
                    Feedback.Stop(eFeedbackType.WalkGrass);   
                }
                if (zoneId == 6)
                {
                    Feedback.Stop(eFeedbackType.WalkMud);    
                }
            }
            else
            {
                if (!isWalking)
                {
                
                    isWalking = true;
                    if (zoneId == 0)
                    {
                        Feedback.Do(eFeedbackType.WalkGravel);
                    }
                    if (zoneId == 4)
                    {
                        Feedback.Do(eFeedbackType.WalkSnow);    
                    }
                    if(zoneId==2 || zoneId==3)
                    {
                        Feedback.Do(eFeedbackType.WalkSand);   
                    }
                    if(zoneId==1)
                    {
                        Feedback.Do(eFeedbackType.WalkGrass);   
                    }
                
                    if (zoneId == 5)
                    {
                        Feedback.Do(eFeedbackType.WalkWood);    
                    }
                    if (zoneId == 6)
                    {
                        Feedback.Do(eFeedbackType.WalkMud);    
                    }
                    /*
                    if (GameController.Instance.GetZoneId() == 5)
                    {
                        Feedback.Do(eFeedbackType.WalkSnow);
                    }*/
                }

                SetDirection(GetDirectionFromVector(rb.velocity));
                meleeComponent.transform.SetScaleX(GetDirectionFromVector(rb.velocity)==eDirection.Right?1:-1);
                compRendering.PlayAnimation(eAnimation.Walk);
            }
            // Si no estaba caminando antes, empezar a reproducir el sonido
            
        }
        else
        {
            // Si estaba caminando pero dejó de hacerlo, detener el sonido
            if (isWalking)
            {
                isWalking = false;
                if (zoneId == 0)
                {
                    Feedback.Stop(eFeedbackType.WalkGravel);    
                }
                if (zoneId == 4)
                {
                    Feedback.Stop(eFeedbackType.WalkSnow);    
                }
                if (zoneId == 5)
                {
                    Feedback.Stop(eFeedbackType.WalkWood);    
                }
                if(zoneId==2 || zoneId==3)
                {
                    Feedback.Stop(eFeedbackType.WalkSand);   
                }
                
                if(zoneId==1)
                {
                    Feedback.Stop(eFeedbackType.WalkGrass);   
                }
                if (zoneId == 6)
                {
                    Feedback.Stop(eFeedbackType.WalkMud);    
                }
                /*
                if (GameController.Instance.GetZoneId() == 5)
                {
                    Feedback.Stop(eFeedbackType.WalkSnow);    
                }*/
                
                
            }
            compRendering.PlayAnimation(eAnimation.Idle);
        }
        if (transform.position.y < -10)
        {
            Feedback.Stop(eFeedbackType.WalkMud);
            Feedback.Stop(eFeedbackType.WalkSand);  
            Feedback.Stop(eFeedbackType.WalkGravel);  
            Feedback.Stop(eFeedbackType.WalkSnow);  
            Feedback.Stop(eFeedbackType.WalkWood);
            Feedback.Stop(eFeedbackType.WalkGrass);
            compRendering.PlayAnimation(eAnimation.Jump);
            this.gameObject.GetComponent<CapsuleCollider2D>().enabled=true;
            isDeath=false;
            isSoundAmbient = false;
            isWalking = false;
            
            GameController.Instance.DoFade();
            
            StartCoroutine(TransformPosition());
            
        }
    }

    IEnumerator TransformPosition()
    {
        yield return new WaitForSeconds(2);
        transform.position = new Vector2(storedX, 5);
        GameObject canvas = GameObject.Find("Canvas");

        if (canvas != null)
        {
            // Encuentra el Joystick dentro del Canvas
            Transform joystick = canvas.transform.Find("Joystick");

            if (joystick != null)
            {
                // Encuentra el Knob dentro del Joystick
                Transform knob = joystick.Find("Knob");

                if (knob != null)
                {
                    // Desactiva el objeto Knob
                    
                    knob.GetComponent<OnScreenStick>().enabled = true;
                    knob.localPosition = Vector3.zero;
                }
                else
                {
                    Debug.LogWarning("Knob no encontrado");
                }
            }
            else
            {
                Debug.LogWarning("Joystick no encontrado");
            }
        }
        else
        {
            Debug.LogWarning("Canvas no encontrado");
        }
        
    }
    public void PlusExp(int expPlus)
    {
        int lastCurrentExp;
        lastCurrentExp = currentExp;
        currentExp += expPlus;
        if (currentExp > 100)
        {
            currentLevel += 1;
        }
        statusBar.SetExperience(currentExp,lastCurrentExp);
        statusBar.SetLevel(currentLevel);
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            
            isJumping = false;
            zoneId = 0;
            // SpriteRenderer spriteRenderer = GameController.Instance.Zone.BlackZone.GetComponent<SpriteRenderer>();
            // Color color = spriteRenderer.color;
            // color.a = 0f;
            // spriteRenderer.color = color;

            // Usar DOTween para hacer el fade del alfa de 0 a 1 en 1 segundo
            //spriteRenderer.DOFade(1f, 1f);

        }
        if (other.gameObject.tag == "Snow")
        {
            if (!isSoundAmbient)
            {
                Feedback.Do(eFeedbackType.SnowZone5Ambient);
                isSoundAmbient = true;
            }
            isJumping = false;
            zoneId = 4;
            // SpriteRenderer spriteRenderer = GameController.Instance.Zone.BlackZone.GetComponent<SpriteRenderer>();
            // Color color = spriteRenderer.color;
            // color.a = 0f;
            // spriteRenderer.color = color;

            // Usar DOTween para hacer el fade del alfa de 0 a 1 en 1 segundo
            //spriteRenderer.DOFade(1f, 1f);

        }
        if (other.gameObject.tag == "Sand")
        {
            
            if (!isSoundAmbient)
            {
                Feedback.Do(eFeedbackType.SandZone23Ambient);
                isSoundAmbient = true;
            }
            isJumping = false;
            zoneId = 2;
            // SpriteRenderer spriteRenderer = GameController.Instance.Zone.BlackZone.GetComponent<SpriteRenderer>();
            // Color color = spriteRenderer.color;
            // color.a = 0f;
            // spriteRenderer.color = color;

            // Usar DOTween para hacer el fade del alfa de 0 a 1 en 1 segundo
            //spriteRenderer.DOFade(1f, 1f);

        }
        if (other.gameObject.tag == "Grass")
        {
            if (!isSoundAmbient)
            {
                Feedback.Do(eFeedbackType.SandZone23Ambient);
                isSoundAmbient = true;
            }
            isJumping = false;
            zoneId = 1;
            // SpriteRenderer spriteRenderer = GameController.Instance.Zone.BlackZone.GetComponent<SpriteRenderer>();
            // Color color = spriteRenderer.color;
            // color.a = 0f;
            // spriteRenderer.color = color;

            // Usar DOTween para hacer el fade del alfa de 0 a 1 en 1 segundo
            //spriteRenderer.DOFade(1f, 1f);

        }
        if (other.gameObject.tag == "Mud")
        {
            if (!isSoundAmbient)
            {
                Feedback.Do(eFeedbackType.MudZone7Ambient);
                isSoundAmbient = true;
            }
            isJumping = false;
            zoneId = 6;
            // SpriteRenderer spriteRenderer = GameController.Instance.Zone.BlackZone.GetComponent<SpriteRenderer>();
            // Color color = spriteRenderer.color;
            // color.a = 0f;
            // spriteRenderer.color = color;

            // Usar DOTween para hacer el fade del alfa de 0 a 1 en 1 segundo
            //spriteRenderer.DOFade(1f, 1f);

        }
        if (other.gameObject.tag == "Wood")
        {
            if (!isSoundAmbient)
            {
                Feedback.Do(eFeedbackType.WoodZone6Ambient);
                isSoundAmbient = true;
            }
            isJumping = false;
            zoneId = 5;
            // SpriteRenderer spriteRenderer = GameController.Instance.Zone.BlackZone.GetComponent<SpriteRenderer>();
            // Color color = spriteRenderer.color;
            // color.a = 0f;
            // spriteRenderer.color = color;

            // Usar DOTween para hacer el fade del alfa de 0 a 1 en 1 segundo
            //spriteRenderer.DOFade(1f, 1f);

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
                
                //Debug.Log("Hit Player");
                Vector2 direction = new Vector2(transform.position.x-other.transform.position.x ,  0).normalized;
                // Aquí activas la animación de "hit" en el player
                compRendering.PlayAnimation(eAnimation.Hit);
                this.lastDirection = GetDirectionFromVector(direction*-1);
                isHit = true;
                //Debug.Log(direction);
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
        if(isDeath) return;
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
        if(isDeath) return;
        if (isHit) return;
        if (isChallenging) return;
        if (isAttacking) return;
        if (isJumping) return;
        Feedback.Do(eFeedbackType.SwordAttack);
        if (value.ReadValueAsButton())
        {
            
            compRendering.PlayAnimation(eAnimation.Attack);
            isAttacking = true;
            StartCoroutine(AttackEnd());
        }

    }

    public void PlayDeath()
    {
        GameObject canvas = GameObject.Find("Canvas");

        if (canvas != null)
        {
            // Encuentra el Joystick dentro del Canvas
            Transform joystick = canvas.transform.Find("Joystick");

            if (joystick != null)
            {
                // Encuentra el Knob dentro del Joystick
                Transform knob = joystick.Find("Knob");

                if (knob != null)
                {
                    // Desactiva el objeto Knob
                    
                    knob.GetComponent<OnScreenStick>().enabled = false;
                    knob.localPosition = Vector3.zero;
                }
                else
                {
                    Debug.LogWarning("Knob no encontrado");
                }
            }
            else
            {
                Debug.LogWarning("Joystick no encontrado");
            }
        }
        else
        {
            Debug.LogWarning("Canvas no encontrado");
        }
        rb.AddForce(Vector2.up * jumpForce);
        compRendering.PlayAnimation(eAnimation.Death);
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
    public bool IsChallenging {
        set { isChallenging = value; }
        get { return isChallenging; }
    }
    public bool IsJumping => isJumping;
    public bool IsAttacking => isAttacking;
    public bool IsSoundAmbient {
        set { isSoundAmbient = value; }
        get { return isSoundAmbient; }
    }
    public bool IsHit => isHit;
    public bool IsDeath => isDeath;
    public bool Death {
        set { isDeath = value; }
        get { return isDeath; }
    }
    public float JumpForce {
    set { jumpForce = value; }
    get { return jumpForce; }
    }
    public float StoredX
    {
        set { storedX = value; }
        get { return storedX; }
    }
}
