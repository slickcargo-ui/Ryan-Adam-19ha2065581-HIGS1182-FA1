using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    private enum EnemyState { Idle, Chasing }
    private EnemyState currentState;

    [Header("Player Reference")]
    [SerializeField] private Transform playerTransform;
    [Header("Detection range")]
    [SerializeField] private SphereCollider detectionRange;
    [Header("Enemy movement")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float driftSpeed = 2f;
    [SerializeField] private float speedIncreasePerLevel = 2f;

    private Rigidbody rb;
    private Vector3 driftDirection;
    private float effectiveSpeed;

    private void Awake()
    {
        GameObject player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogWarning("Player not found in the scene. Please ensure the player has the 'Player' tag.");
        }
    }

    private void Start()
    {
        driftDirection = Random.insideUnitSphere;
        driftDirection.y *= 0.2f;
    }

    private void Update()
    {
        if (currentState == EnemyState.Idle)
        {
            Patrol();
        }

        else if (currentState == EnemyState.Chasing)
        {
            ChasePlayer();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered detection range.");
            currentState = EnemyState.Chasing;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited detection range.");
            currentState = EnemyState.Idle;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log("Enemy collided with player.");

            PlayerController player = collision.collider.GetComponent<PlayerController>();
            if (player != null)
            {
                player.Die();
            }

            Destroy(gameObject);
        }
    }

    private void SpeedIncrease()
    {
        if (GameManager.Instance != null)
            effectiveSpeed = moveSpeed + (GameManager.Instance.DifficultyLevel - 1) * speedIncreasePerLevel;
    }

    private void Patrol()
    {
        transform.position += driftDirection * driftSpeed * Time.deltaTime;
    }

    private void ChasePlayer()

    {
        SpeedIncrease();
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        transform.position += direction * effectiveSpeed * Time.deltaTime;
    }
    public void DestroyEnemy()
    {
        GameManager.Instance?.DestroyEffect(transform.position);
        Debug.Log("Enemy destroyed by player shot.");
        Destroy(gameObject);
    }

}
