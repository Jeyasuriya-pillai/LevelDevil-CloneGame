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
    
    private bool isDead = false;       
    private bool isSpawning = true;    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); 
        animator = GetComponent<Animator>(); 
        
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.reg);
        }

        StartCoroutine(SpawnSequence());
    }

    void Update()
    {
        if (isDead || isSpawning) return;

        moveInput = Input.GetAxisRaw("Horizontal"); 

        if (moveInput > 0) spriteRenderer.flipX = false;
        else if (moveInput < 0) spriteRenderer.flipX = true;

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            
            if (AudioManager.instance != null)
            {
                AudioManager.instance.PlaySFX(AudioManager.instance.jump);
            }
        }

        HandleRunningAudio();
        UpdateAnimations();
    }

    void FixedUpdate()
    {   
        if (isDead || isSpawning) 
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return; 
        }

        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        if (groundCheck != null) 
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        }
    }

    private void HandleRunningAudio()
    {
        if (AudioManager.instance == null) return;

        if (isGrounded && Mathf.Abs(moveInput) > 0.1f)
        {
            AudioManager.instance.PlayLoopingSFX(AudioManager.instance.run);
        }
        else
        {
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

        yield return new WaitForSeconds(0.6f);

        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
        }

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