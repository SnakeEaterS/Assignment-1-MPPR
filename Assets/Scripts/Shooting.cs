using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    private Camera mainCam;
    private Vector3 mousePos;
    public Transform bullet;
    public Transform bulletTransform;
    public Transform bulletParent;
    public bool canFire;
    private float timer;
    public float timeBetweenFiring;

    public enum FireType { Straight, Snake } // Firing modes
    public FireType fireType = FireType.Straight; // Default firing type

    public float snakeFrequency = 5f; // Frequency of snake oscillation
    public float snakeAmplitude = 0.5f; // Amplitude of snake oscillation

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main; // Get the main camera
    }

    // Update is called once per frame
    void Update()
    {
        // Get the mouse position in world space
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);

        // Ensure mousePos has the same z as the Player
        mousePos.z = transform.position.z;

        // Calculate the direction vector from Player to the mouse
        Vector3 direction = mousePos - transform.position;

        // Calculate the angle in degrees
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Apply the rotation to the Player
        transform.rotation = Quaternion.Euler(0, 0, rotZ);

        // Handle firing cooldown
        if (!canFire)
        {
            timer += Time.deltaTime;
            if (timer > timeBetweenFiring)
            {
                canFire = true;
                timer = 0;
            }
        }

        // Switch firing types based on key presses
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            fireType = FireType.Straight;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            fireType = FireType.Snake;
        }

        // Handle firing when the mouse button is held
        if (Input.GetMouseButton(0) && canFire)
        {
            canFire = false;

            // Spawn the bullet
            Transform bulletPrefab = Instantiate(bullet, bulletTransform.position, Quaternion.identity);
            if (bulletParent != null)
            {
                bulletPrefab.SetParent(bulletParent);
            }

            // Configure the bullet based on the firing type
            BulletScript bulletScript = bulletPrefab.GetComponent<BulletScript>();
            if (bulletScript != null)
            {
                bulletScript.fireType = fireType; // Set the firing type
                bulletScript.snakeFrequency = snakeFrequency;
                bulletScript.snakeAmplitude = snakeAmplitude;
            }
        }
    }
}
