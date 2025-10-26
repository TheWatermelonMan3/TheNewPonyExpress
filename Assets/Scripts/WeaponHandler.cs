using UnityEngine;
using System;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 10f;

    private bool fineTune = false;
    private int rangeMin = 69;
    private int rangeMax = 112;

    private void Update()
    {
        HandleInput();
    }

    //return angle in -180 to 180
    private float angleWrapped()
    {
        return transform.eulerAngles.y%180;
    }

    private float angleWrapped(float ang)
    {
        return ang%180;
    }


    //snap to 45 deg
    private int closestBigAngle(float currentAngle)
    {
        currentAngle = currentAngle % 180;

        float a = (float)(currentAngle) / 45.0f;
        if(a <= 0.5 && a >= -0.5)
        {
            return 0;
        }
        else if(a > 0.5 && a <= 1.5)
        {
            return 45;
        }
        else if(a > 1.5 && a <= 2.5)
        {
            return 90;
        }
        else if(a > 2.5 && a <= 3.5)
        {
            return 135;
        }
        else if(a > 3.5 && a <= 4.5)
        {
            return 180;
        }
        else if(a > 4.5 && a <= 5.5)
        {
            return 225;
        }
        else if(a > 5.5 && a <= 6.5)
        {
            return 270;
        }
        else if(a < -0.5 && a >= -1.5)
        {
            return -45;
        }
        else if(a < -1.5 && a >= -2.5)
        {
            return -90;
        }
        else if(a < -2.5 && a >= -3.5)
        {
            return -135;
        }
        else if(a < -3.5 && a >= -4.5)
        {
            return -180;
        }

        Debug.Log(a);
        //shouldnt be an else but just in case
        return -1;
    }


    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FireBullet();
        }

        //toggle fine tune
        if(Input.GetKeyDown(KeyCode.F))
        {
            fineTune = fineTune ? false : true;
            if(fineTune)
            {
                rangeMin =  Math.Max((int)(angleWrapped() - 5)%180,90-45/2);
                rangeMax = Math.Min((int)(angleWrapped() + 5)%180,90+45/2);
            }
            else{
                rangeMin = 90-45/2; //69
                rangeMax = 90+45/2; //112
            }
        }

        Vector3 rotation = new Vector3(1,0,0);
        Vector3 rotationUp = new Vector3(0,0,-1);

        if(Input.GetKey(KeyCode.S))
        {
            if(angleWrapped() + 1 <= rangeMax) transform.Rotate(rotation);
        }
        else if(Input.GetKey(KeyCode.A))
        {
            if(angleWrapped() - 1 >= rangeMin) transform.Rotate(-rotation);
        }
        else if(Input.GetKey(KeyCode.E))
        {
            transform.Rotate(rotationUp);
        }
        else if(Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-rotationUp);
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