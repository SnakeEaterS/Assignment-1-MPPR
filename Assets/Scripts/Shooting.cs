using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public static Shooting Instance;  // Singleton instance for easy access to this script from other scripts.

    public Transform bullet1; // Red bullet prefab reference.
    public Transform bullet2; // Blue bullet prefab reference.
    public Transform bullet3; // Powerup bullet prefab reference.
    public Transform bulletTransform; // Position from which bullets are fired.
    public Transform bulletParent; // Parent object to organize instantiated bullets.

    private Camera mainCam; // Reference to the main camera.
    private Vector3 mousePos; // Stores the mouse position in world space.
    private Transform currentBullet; // Current bullet prefab being fired.

    private float timer; // Timer for tracking firing cooldown.

    public bool canFire; // Flag to control whether the player can fire.
    public bool powerUpObtained = false; // Tracks if a power-up was collected.
    public bool powerUpActive = false; // Tracks if a power-up is currently active.

    public float timeBetweenFiring; // Time delay between each shot.
    public float snakeFrequency = 5f; // Frequency of snake oscillation movement.
    public float snakeAmplitude = 0.5f; // Amplitude of snake oscillation movement.

    public enum FireType { Straight, Snake } // Enum defining firing types.
    public FireType fireType = FireType.Straight; // Default firing type set to straight.

    void Awake()
    {
        Instance = this;  // Assigns this script to the static instance for global access.
    }

    void Start()
    {
        mainCam = Camera.main; // Get reference to the main camera.
        currentBullet = bullet1; // Set the default bullet to bullet1 (red bullet).
    }

    void Update()
    {
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition); // Converts the mouse position to world space.
        mousePos.z = transform.position.z; // Ensures the mouse position matches the playerfs z-coordinate.

        Vector3 direction = mousePos - transform.position; // Calculates the direction vector from the player to the mouse.

        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Converts the direction vector to a rotation angle in degrees.
        transform.rotation = Quaternion.Euler(0, 0, rotZ); // Applies the calculated rotation to the player object.

        if (!canFire) // Checks if the player is currently on a cooldown.
        {
            timer += Time.deltaTime; // Increment the timer by the time elapsed since the last frame.
            if (timer > timeBetweenFiring) // If the cooldown time has passed:
            {
                canFire = true; // Allow firing again.
                timer = 0; // Reset the timer.
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) // Switch to straight firing mode when "1" key is pressed.
        {
            fireType = FireType.Straight;
            timeBetweenFiring = 0.3f; // Set firing cooldown for straight shots.
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) // Switch to snake firing mode when "2" key is pressed.
        {
            fireType = FireType.Snake;
            timeBetweenFiring = 0.1f; // Set faster firing cooldown for snake shots.
        }

        if (Input.GetKeyDown(KeyCode.E)) // Switch to blue bullet when "E" key is pressed.
        {
            if (!powerUpActive)
            {
                currentBullet = bullet2;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Q)) // Switch to red bullet when "Q" key is pressed.
        {
            if (!powerUpActive)
            {
                currentBullet = bullet1;
            }
        }

        if (Input.GetMouseButton(0) && canFire) // If left mouse button is held and firing is allowed:
        {
            canFire = false; // Disable further firing until cooldown is reset.

            Transform bulletPrefab = Instantiate(currentBullet, bulletTransform.position, Quaternion.identity); // Instantiate the bullet at the firing position.
            if (bulletParent != null)
            {
                bulletPrefab.SetParent(bulletParent); // Set the bullet's parent for organization.
            }

            BulletScript bulletScript = bulletPrefab.GetComponent<BulletScript>(); // Get the BulletScript attached to the bullet.
            if (bulletScript != null)
            {
                bulletScript.fireType = fireType; // Set the firing type for the bullet.
                bulletScript.snakeFrequency = snakeFrequency; // Pass snake frequency to the bullet.
                bulletScript.snakeAmplitude = snakeAmplitude; // Pass snake amplitude to the bullet.
            }
        }
    }

    private void LateUpdate()
    {
        if (powerUpObtained) // If a power-up was collected:
        {
            StartCoroutine(Powerup()); // Start the power-up effect coroutine.
            powerUpObtained = false; // Reset the power-up flag.
        }
    }

    public IEnumerator Powerup()
    {
        float currentShootRate = timeBetweenFiring; // Store the current firing rate.
        float newShootRate = currentShootRate / 2; // Halve the firing cooldown to double the firing rate.

        timeBetweenFiring = newShootRate; // Apply the new firing rate.
        currentBullet = bullet3; // Switch to the power-up bullet.

        yield return new WaitForSeconds(5f); // Wait for 5 seconds while power-up is active.

        timeBetweenFiring = currentShootRate; // Restore the original firing rate.
        currentBullet = bullet1; // Revert to the default bullet.
        powerUpActive = false; // Mark the power-up as inactive.
    }
}

