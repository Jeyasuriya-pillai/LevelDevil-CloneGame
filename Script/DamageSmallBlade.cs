using UnityEngine;

public class DamageSmallBlade : MonoBehaviour
{
    [Header("Mode Selection")]
    [Tooltip("Check [✓] for One-Time Trap mode. Uncheck [ ] for continuous Normal patrol mode.")]
    [SerializeField] private bool isTrapMode = true;         

    [Header("References")]
    [SerializeField] private Transform playerTransform;      

    [Header("Patrol Points")]
    [SerializeField] private Transform point1;               
    [SerializeField] private Transform point2;               

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
        if (point1 != null)
        {
            transform.position = point1.position;
        }
        
        currentTarget = point2;
        headingToPoint2 = true;
        playerExitedZone = true;

        if (!isTrapMode)
        {
            isMoving = true;
        }
    }

    void Update()
    {
        if (!isTrapMode)
        {
            MoveAndRotateBlade();
            return; 
        }

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

        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, currentTarget.position) < 0.1f)
        {
            if (headingToPoint2)
            {
                currentTarget = point1;
                headingToPoint2 = false; 
            }
            else
            {
                if (isTrapMode)
                {
                    isMoving = false; 
                }
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

            Vector3 safeGizmoPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.1f);
            
            Gizmos.DrawWireSphere(safeGizmoPosition, activationDistance);
        }
    }
}