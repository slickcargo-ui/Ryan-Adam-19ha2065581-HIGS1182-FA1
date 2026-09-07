using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Camera Settings")]
    [SerializeField] private Vector3 offset = new Vector3(0f, 2f, -6f);
    [SerializeField] private float FollowSmoothTime = 0.15f;
    [SerializeField] private float RotationSmoothSpeed = 8f;

    private Vector3 velocity = Vector3.zero;

    private void LateUpdate()
    {
        if (target == null)
            return;

        FollowPosition();
        FollowRotation();
        
    }
    private void FollowPosition()
    {
        Vector3 targetPosition = target.TransformPoint(offset);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, FollowSmoothTime);
    }

    private void FollowRotation()
    {
        Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position + target.forward * 2f);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, RotationSmoothSpeed * Time.deltaTime);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
