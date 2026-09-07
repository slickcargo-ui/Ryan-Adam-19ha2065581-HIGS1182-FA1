using UnityEngine;

public class AsteroidBehaviour : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float driftSpeed = 2f;
    [SerializeField] private float rotationSpeed = 20f;

    private Rigidbody rb;
    private Vector3 driftDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    private void Start()
    {
        driftDirection = Random.insideUnitSphere;
        driftDirection.y *= 0.2f;
    }

    private void Update()
    {
        transform.position += driftDirection * driftSpeed * Time.deltaTime;
        transform.Rotate(Vector3.one * rotationSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log("Asteroid collided with player.");

            PlayerController player = collision.collider.GetComponent<PlayerController>();
            if (player != null)
            {
                player.Die();
            }

            Destroy(gameObject);
        }
    }
    public void DestroyAsteroid()
    {
        Debug.Log("Asteroid destroyed by player shot.");
        // Could add an explosion particle effect here before destroying.
        Destroy(gameObject);
    }
}
