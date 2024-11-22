using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    

    public float maxForce = 10f; // Maximum speed
    public float accelerationTime = 1f; // Time to reach max speed
    public float lifeTime = 5f; // Bullet lifetime in seconds
    public float snakeFrequency = 5f; // Frequency of the snake oscillation
    public float snakeAmplitude = 0.5f; // Amplitude of the snake oscillation

    private Vector3 mousePos;
    private Camera mainCam;
    private Rigidbody2D rb;
    private float currentTime = 0f; // Tracks the time for acceleration
    private Vector2 direction;
    private Vector2 perpendicular;

    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();

        // Calculate initial direction
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = transform.position.z; // Ensure same z-plane
        direction = (mousePos - transform.position).normalized;

        // Calculate perpendicular direction for snake movement
        perpendicular = new Vector2(-direction.y, direction.x);

        // Destroy the bullet after its lifetime expires
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // Increment the current time
        currentTime += Time.deltaTime;

        // Calculate the force based on an easing function
        float t = Mathf.Clamp01(currentTime / accelerationTime);
        float easedForce = Mathf.Lerp(0, maxForce, t * t); // Quadratic easing out

        // Snake oscillation offset
        float oscillation = Mathf.Sin(Time.time * snakeFrequency) * snakeAmplitude;

        // Apply direction and oscillation
        Vector2 snakeVelocity = direction * easedForce + perpendicular * oscillation;
        rb.velocity = snakeVelocity;
    }
}
