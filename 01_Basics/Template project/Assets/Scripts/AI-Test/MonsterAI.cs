using UnityEngine;

[RequireComponent(typeof(CharacterMotor))]
public class MonsterAI : MonoBehaviour
{
    public Transform player;

    [Header("Patrol points")]
    public Transform[] patrolPoints;
    private int currentPatrolIndex = 0;
    public float patrolSpeed = 3f;

    [Header("Distances before detection")]
    public float normalChaseDistance = 10f;
    public float maxChaseDistance = 20f;

    [Header("Chasing")]
    public float chaseDuration = 5f;
    private float chaseTimer = 0f;
    public float chaseSpeed = 10f;
    private bool isChasing = false;

    private bool playerInLight;
    private bool playerCrouching;
    private bool playerSeen;
    private bool playerRunning;

    private Vector3 currentMoveDirection;
    public float turnSmoothSpeed = 3f;

    private CharacterMotor motor;

    private void Awake()
    {
        motor = GetComponent<CharacterMotor>();
    }

    private void Update()
    {
        playerInLight = CheckIfPlayerInLight();
        playerCrouching = CheckIfPlayerIsCrouching();
        playerSeen = CheckIfPlayerSeen();
        playerRunning = CheckIfPlayerIsRunning();

        DetermineChaseDistance();

        if (playerSeen && Vector3.Distance(transform.position, player.position) <= normalChaseDistance)
        {
            StartChasingPlayer();
        }

        if (isChasing)
        {
            chaseTimer -= Time.deltaTime;

            if (chaseTimer <= 0)
            {
                isChasing = false;
            }
            else
            {
                ChasePlayer();
            }
        }
        else
        {
            Patrol();
        }
    }

    private void StartChasingPlayer()
    {
        if (!isChasing)
        {
            isChasing = true;
            chaseTimer = chaseDuration;
        }
    }

    private void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        Vector3 targetPosition = patrolPoints[currentPatrolIndex].position;
        Vector3 targetDirection = targetPosition - transform.position;
        targetDirection.y = 0f;
        targetDirection.Normalize();

        // Smoothly blend toward target direction (THIS creates the arc)
        currentMoveDirection = Vector3.Slerp(
            currentMoveDirection,
            targetDirection,
            Time.deltaTime * turnSmoothSpeed
        ).normalized;

        

        if (Vector3.Distance(transform.position, targetPosition) < 1f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            Move(currentMoveDirection, patrolSpeed, true);

        }
        else
        {
            Move(currentMoveDirection, patrolSpeed, false);
        }
        
    }

    private void ChasePlayer()
    {
        Vector3 direction = player.position - transform.position;

        direction.y = 0f; // prevent tilting
        direction.Normalize();

        Move(direction, chaseSpeed,false);
    }

    private void Move(Vector3 direction, float speed, bool jump)
    {
        // Rotate toward movement direction
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                Time.deltaTime * motor.movementConfig.rotationSpeed
            );
        }

        motor.Tick(direction, speed, jump);
        motor.SetFacingData(currentMoveDirection);
    }

    private bool CheckIfPlayerInLight()
    {
        return false;
    }

    private bool CheckIfPlayerIsCrouching()
    {
        return player.GetComponent<PlayerController>().motor.isCrouching;
    }

    private bool CheckIfPlayerIsRunning()
    {
        return false;
    }

    private void DetermineChaseDistance()
    {
        normalChaseDistance = 10f;

        if (playerCrouching)
            normalChaseDistance = 5f;

        if (playerRunning)
            normalChaseDistance = 15f;

        if (playerInLight)
            normalChaseDistance *= 1.5f;

        normalChaseDistance = Mathf.Min(normalChaseDistance, maxChaseDistance);
    }

    private bool CheckIfPlayerSeen()
    {
        RaycastHit hit;
        Vector3 directionToPlayer = player.position - transform.position;

        if (Physics.Raycast(transform.position, directionToPlayer.normalized, out hit, normalChaseDistance))
        {
            if (hit.collider.CompareTag("Player"))
            {
                return true;
            }
        }

        return false;
    }
}