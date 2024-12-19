using UnityEngine;

public class ShadowCrawler : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float slowMoveSpeed = 2f;
    public float attackRadius = 10f;
    public Transform pointA;
    public Transform pointB;

    private Transform currentTarget;
    private Transform player;
    private Rigidbody2D rb;
    private enum CrawlerState { Patrolling, MovingToPlayer };
    private CrawlerState currentState;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentTarget = pointA;
        currentState = CrawlerState.Patrolling;
    }

    void Update()
    {
        HandleState();
    }

    void FixedUpdate()
    {
        if (currentState == CrawlerState.Patrolling)
            PatrolBehavior();
        else if (currentState == CrawlerState.MovingToPlayer)
            MoveToPlayerBehavior();
    }

    private void HandleState()
    {
        if (currentState == CrawlerState.Patrolling)
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
                currentState = CrawlerState.MovingToPlayer;
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
                currentState = CrawlerState.Patrolling;
            }
        }
        else
        {
            currentState = CrawlerState.Patrolling;
        }
    }

    private void MoveTowards(Vector2 targetPosition, float speed)
    {
        Vector2 direction = ((Vector2)targetPosition - rb.position).normalized;
        rb.velocity = direction * speed;
    }
}
