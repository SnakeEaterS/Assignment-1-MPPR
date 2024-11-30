using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public static Shooting Instance;  // Static reference to the instance

    public Transform bullet1; // Red bullet
    public Transform bullet2; // Blue bullet
    public Transform bullet3; // Powerup bullet
    public Transform bulletTransform;
    public Transform bulletParent;

    private Camera mainCam;
    private Vector3 mousePos;
    private Transform currentBullet; // Current bullet prefab

    private float timer;

    public bool canFire;
    public bool powerUpObtained = false; // Checks if the player has collected a power-up
    public bool powerUpActive = false; // Checks if the power-up is currently active

    public float timeBetweenFiring;
    public float snakeFrequency = 5f; // Frequency of snake oscillation
    public float snakeAmplitude = 0.5f; // Amplitude of snake oscillation

    public enum FireType { Straight, Snake } // Firing modes
    public FireType fireType = FireType.Straight; // Default firing type

    void Awake()
    {
        Instance = this;  // Set this instance as the singleton
    }
    void Start()
    {
        mainCam = Camera.main; // Get the main camera
        currentBullet = bullet1; // Set the default bullet prefab
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

        // Switch bullet type based on key presses
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!powerUpActive)
            {
                currentBullet = bullet2;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            if (!powerUpActive)
            {
                currentBullet = bullet1;

            }
        }

        // Handle firing when the mouse button is held
        if (Input.GetMouseButton(0) && canFire)
        {
            canFire = false;

            // Spawn the bullet
            Transform bulletPrefab = Instantiate(currentBullet, bulletTransform.position, Quaternion.identity);
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

    private void LateUpdate() // Checks if the player has picked up a power-up, starts the Power-up coroutine if true
    {
        if (powerUpObtained == true)
        {
            StartCoroutine(Powerup());
            powerUpObtained = false;
        }
    }

    // Powerup Coroutine: When power-up is active, shoot deadly bullets 1.5x the size of a normal bullet at 2x speed for 5 seconds
    public IEnumerator Powerup()
    {

        float currentShootRate = timeBetweenFiring;
        float newShootRate = (currentShootRate / 2);

        timeBetweenFiring = newShootRate;
        currentBullet = bullet3;

        // Wait for 5 seconds
        yield return new WaitForSeconds(5f);

        // Restore the original firing interval
        timeBetweenFiring = currentShootRate;
        currentBullet = bullet1; // Falls back to deafult bullet
        powerUpActive = false;
    }
}
