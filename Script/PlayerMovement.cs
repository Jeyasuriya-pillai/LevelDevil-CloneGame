using UnityEngine;
using System.Collections; 
using UnityEngine.SceneManagement; 

public class Player : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    
    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Health System")]
    public float health = 100f; 

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer; 
    private Animator animator; 
    private bool isGrounded;
    private float moveInput; 
    
    private bool isDead = false;       // डैमेज होने पर मूवमेंट रोकने के लिए
    private bool isSpawning = true;    // शुरुआत में 'PlayerRegAni' को एक बार चलाने के लिए

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); 
        animator = GetComponent<Animator>(); 
        
        // Play Registration Spawn Sound instantly
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.reg);
        }

        // गेम रीस्टार्ट होते ही स्पॉन सीक्वेंस शुरू करें
        StartCoroutine(SpawnSequence());
    }

    void Update()
    {
        if (isDead || isSpawning) return;

        // 1. Get Horizontal Input
        moveInput = Input.GetAxisRaw("Horizontal"); 

        // Flip the sprite based on movement direction
        if (moveInput > 0) spriteRenderer.flipX = false;
        else if (moveInput < 0) spriteRenderer.flipX = true;

        // 2. Single Jump & Play Jump Audio
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            
            // Play Jump Sound (Using new 'jump' name)
            if (AudioManager.instance != null)
            {
                AudioManager.instance.PlaySFX(AudioManager.instance.jump);
            }
        }

        // Handle Audio playback state for running
        HandleRunningAudio();

        // Handle Animations every frame
        UpdateAnimations();
    }

    void FixedUpdate()
    {   
        if (isDead || isSpawning) 
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return; 
        }

        // 3. Smooth physics-based horizontal movement
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // 4. Ground Check Logic
        if (groundCheck != null) 
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        }
    }

    private void HandleRunningAudio()
    {
        if (AudioManager.instance == null) return;

        // If player is firmly on the ground AND actively pressing A or D keys
        if (isGrounded && Mathf.Abs(moveInput) > 0.1f)
        {
            // Using new 'run' name
            AudioManager.instance.PlayLoopingSFX(AudioManager.instance.run);
        }
        else
        {
            // Stop playing the running clip the exact frame they stop or jump
            AudioManager.instance.StopLoopingSFX();
        }
    }

    private IEnumerator SpawnSequence()
    {
        isSpawning = true;

        if (animator != null)
        {
            animator.Play("PlayerRegAni"); 
        }

        yield return new WaitForSeconds(0.5f); 

        isSpawning = false; 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Damage") && !isDead)
        {
            StartCoroutine(KillSequence());
        }
    }

    private IEnumerator KillSequence()
    {
        isDead = true; 
        health = 0f; 

        // Stop running sound instantly upon dying
        if (AudioManager.instance != null)
        {
            AudioManager.instance.StopLoopingSFX();
            AudioManager.instance.PlaySFX(AudioManager.instance.death); 
        }

        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic; 

        if (animator != null)
        {
            animator.Play("PlayerDeathAni"); 
        }

        // 1. Hold for death animation
        yield return new WaitForSeconds(0.6f);

        // 2. Hide player sprite
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
        }

        // 3. 1.5 Second Gap before restart
        yield return new WaitForSeconds(1.5f);

        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    void UpdateAnimations()
    {
        if (animator == null || isDead || isSpawning) return;

        if (isGrounded)
        {
            if (moveInput == 0f) animator.Play("PlayerAni"); 
            else animator.Play("PlayerRunAni"); 
        }
        else
        {
            if (rb.linearVelocity.y > 0.1f) animator.Play("PlayerJumpAni"); 
            else if (rb.linearVelocity.y < -0.1f) animator.Play("PlayerFallAni"); 
        }
    }
}