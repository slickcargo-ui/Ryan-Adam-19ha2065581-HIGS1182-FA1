using UnityEngine;

public class Collectible : MonoBehaviour
{
    [Header("Visuals")]
    [SerializeField] private float rotationSpeed = 60f;

    private void Awake()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Debug.Log("Collectible picked up by player.");

        GameManager.Instance?.AddScore(1);

        Destroy(gameObject);
    }
}
