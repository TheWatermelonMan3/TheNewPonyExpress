using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 10f;

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FireBullet();
        }
        Vector3 rotation = new Vector3(10,0,0);
        if(Input.GetKeyDown(KeyCode.S))
        {
            transform.Rotate(rotation);
        }
        else if(Input.GetKeyDown(KeyCode.A))
        {
            transform.Rotate(-rotation);
        }
    }

    private void FireBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
        if (bulletRigidbody != null)
        {
            bulletRigidbody.linearVelocity = bulletSpawnPoint.forward * bulletSpeed;
            Debug.Log(bulletRigidbody.linearVelocity);
        }
        else{
            Debug.Log("is null");
        }
    }
}