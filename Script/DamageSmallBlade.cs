using UnityEngine;

public class DamageSmallBlade : MonoBehaviour
{
    [Header("Mode Selection")]
    [Tooltip("Check [✓] for One-Time Trap mode. Uncheck [ ] for continuous Normal patrol mode.")]
    [SerializeField] private bool isTrapMode = true;         // Default to trap mode

    [Header("References")]
    [SerializeField] private Transform playerTransform;      // Drag your Player here

    [Header("Patrol Points")]
    [SerializeField] private Transform point1;               // Drag Point 1 empty GameObject here
    [SerializeField] private Transform point2;               // Drag Point 2 empty GameObject here

    [Header("Movement Settings")]
    [Tooltip("How many grid units away the player needs to be from the BLADE to trigger it (Only used in Trap Mode).")]
    [SerializeField] private float activationDistance = 4f;  
    [SerializeField] private float moveSpeed = 7f;           
    [SerializeField] private float rotationSpeed = 200f;     

    private bool isMoving = false;
    private Transform currentTarget;
    private bool headingToPoint2 = true; 
    private bool playerExitedZone = true; 

    void Start()
    {
        // Shuruat mein blade ko Point 1 par set karo
        if (point1 != null)
        {
            transform.position = point1.position;
        }
        
        currentTarget = point2;
        headingToPoint2 = true;
        playerExitedZone = true;

        // Agar NORMAL MODE hai, toh game shuru hote hi blade chalna shuru kar dega
        if (!isTrapMode)
        {
            isMoving = true;
        }
    }

    void Update()
    {
        // --- 1. NORMAL MODE PATROL LOGIC ---
        if (!isTrapMode)
        {
            MoveAndRotateBlade();
            return; // Normal mode chal raha hai toh niche ka trap logic skip karo
        }

        // --- 2. TRAP MODE LOGIC ---
        if (playerTransform == null) return;

        float distance = Vector2.Distance(playerTransform.position, transform.position);

        if (distance > activationDistance)
        {
            playerExitedZone = true;
        }

        if (!isMoving && playerExitedZone)
        {
            if (distance <= activationDistance)
            {
                isMoving = true;
                playerExitedZone = false; 
                currentTarget = point2;
                headingToPoint2 = true;
            }
        }
        else if (isMoving)
        {
            MoveAndRotateBlade();
        }
    }

    private void MoveAndRotateBlade()
    {
        if (point1 == null || point2 == null || currentTarget == null) return;

        // --- SPIN & MOVE TOGETHER ---
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, moveSpeed * Time.deltaTime);

        // Check if the blade has arrived at its current target
        if (Vector3.Distance(transform.position, currentTarget.position) < 0.1f)
        {
            if (headingToPoint2)
            {
                currentTarget = point1;
                headingToPoint2 = false; 
            }
            else
            {
                // TRAP MODE: Wapas Point 1 par aane par blade stop ho jayega
                if (isTrapMode)
                {
                    isMoving = false; 
                }
                // NORMAL MODE: Bina ruke loop chalta rahega
                else
                {
                    currentTarget = point2;
                    headingToPoint2 = true;
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (isTrapMode)
        {
            Gizmos.color = Color.red;

            // Gizmos draw karne ke liye ek halka sa Z-offset (jaise 0.1) dene se 
            // Unity ke view frustum ka 'zero depth math bug' bypass ho jata hai.
            Vector3 safeGizmoPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.1f);
            
            Gizmos.DrawWireSphere(safeGizmoPosition, activationDistance);
        }
    }
}