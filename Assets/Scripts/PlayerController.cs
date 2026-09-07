using UnityEditor.Build.Content;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 12f;
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float verticalSpeed = 8f;

    [Header("Reference")]
    [SerializeField] private Projectile projectile;

    private Rigidbody rb;
    private float yaw;
    private float pitch;

    private Vector3 moveInput;
    private bool firePressed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.linearDamping = 2f;

        if (projectile == null)
        {
            projectile = GetComponentInChildren<Projectile>();
        }
    }

    private void Update()
    {
        HandleInput();
        HandleMouseLook();

        if (firePressed && projectile != null)
        {
            projectile.Fire();
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void HandleInput()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        float y = 0f;

        if (Input.GetKey(KeyCode.Space)) y += 1f;
        if (Input.GetKey(KeyCode.LeftControl)) y -= 1f;

        moveInput = new Vector3(x, y, z);
        firePressed = Input.GetMouseButton(0);

    }

    private void Move()
    {
       Vector3 direction = (transform.right * moveInput.x) + (transform.up * moveInput.y) + (transform.forward * moveInput.z);
        rb.linearVelocity = direction.normalized * moveSpeed;
    }

    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -80f, 80f);

        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }

    public void Die()
    {
        Debug.Log("Player has died.");
        GameManager.Instance.GameOver();
    }
}

