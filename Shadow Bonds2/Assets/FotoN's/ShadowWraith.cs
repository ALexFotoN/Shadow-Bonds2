using UnityEngine;

public class ShadowWraith : MonoBehaviour
{
    public float moveSpeed = 4f;
    public float slowMoveSpeed = 1.5f;
    public float attackRadius = 12f;
    public Transform pointA;
    public Transform pointB;

    private Transform currentTarget;
    private Transform player;
    private Rigidbody2D rb;
    private enum WraithState { Patrolling, MovingToPlayer };
    private WraithState currentState;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentTarget = pointA;
        currentState = WraithState.Patrolling;
    }

    void Update()
    {
        HandleState();
    }

    void FixedUpdate()
    {
        if (currentState == WraithState.Patrolling)
            PatrolBehavior();
        else if (currentState == WraithState.MovingToPlayer)
            MoveToPlayerBehavior();
    }

    private void HandleState()
    {
        if (currentState == WraithState.Patrolling)
            LookForPlayer();
    }

    private void PatrolBehavior()
    {
        MoveTowards(currentTarget.position, moveSpeed);
        if (Vector2.Distance(transform.position, currentTarget.position) < 0.1f)
        {
            currentTarget = currentTarget == pointA ? pointB : pointA;
        }
    }

    private void LookForPlayer()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRadius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                player = hit.transform;
                currentState = WraithState.MovingToPlayer;
                break;
            }
        }
    }

    private void MoveToPlayerBehavior()
    {
        if (player != null)
        {
            MoveTowards(player.position, slowMoveSpeed);
            if (Vector2.Distance(transform.position, player.position) > attackRadius * 1.5f)
            {
                currentState = WraithState.Patrolling;
            }
        }
        else
        {
            currentState = WraithState.Patrolling;
        }
    }

    private void MoveTowards(Vector2 targetPosition, float speed)
    {
        Vector2 direction = ((Vector2)targetPosition - rb.position).normalized;
        rb.velocity = direction * speed;
    }
}
