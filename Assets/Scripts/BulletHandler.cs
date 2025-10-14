using UnityEngine;

public class BulletHandler : MonoBehaviour
{
    public float lifeDuration = 1f;

    private void Start()
    {
        DestroyBulletAfterLifetime();
    }

    private void DestroyBulletAfterLifetime()
    {
        Destroy(gameObject, lifeDuration);
    }
}