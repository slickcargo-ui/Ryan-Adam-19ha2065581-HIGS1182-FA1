using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Shooting Settings")]
    [SerializeField] private float MaxDistance = 100f;
    [SerializeField] private float FireRate = 0.3f;
    [SerializeField] private LayerMask HitTableLayers;

    [Header("Reference")]
    [SerializeField] private Transform FirePoint;
    [SerializeField] private LineRenderer ShotVisual;

    private float lastFireTime;

    private void Awake()
    {
        if (FirePoint == null)
        {
            FirePoint = transform;
        }

    }

    public void Fire()
    {
        if (Time.time < lastFireTime + FireRate)
            return;
        lastFireTime = Time.time;

        ShootRay();
    }

    private void ShootRay()
    {
        Ray ray = new Ray(FirePoint.position, FirePoint.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, MaxDistance, HitTableLayers))
        {
            Debug.Log("Hit: " + hit.collider.name);

            AsteroidBehaviour asteroid = hit.collider.GetComponent<AsteroidBehaviour>();
            if (asteroid != null)
            {
                asteroid.DestroyAsteroid();
            }

            DrawShotVisual(FirePoint.position, hit.point);
        }
        else
        {
            DrawShotVisual(FirePoint.position, FirePoint.position + FirePoint.forward * MaxDistance);
        }

    }

    private void DrawShotVisual(Vector3 start, Vector3 end)
    {
        if (ShotVisual == null) return;

        ShotVisual.enabled = true;
        ShotVisual.SetPosition(0, start);
        ShotVisual.SetPosition(1, end);
        Invoke(nameof(HideShotVisual), 0.05f);
    }

    private void HideShotVisual()
    {
        if (ShotVisual != null) ShotVisual.enabled = false;
    }
}
