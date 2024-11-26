using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public Shooting.FireType fireType; // Firing type set by Shooting script
    public float maxForce = 10f;
    public float accelerationTime = 1f;
    public float lifeTime = 5f;
    public float snakeFrequency = 5f; // Frequency of snake oscillation
    public float snakeAmplitude = 0.5f; // Amplitude of snake oscillation

    private Rigidbody2D rb;
    private Vector2 direction;
    private Vector2 perpendicular;
    private float currentTime = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Calculate the direction the bullet will move
        Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        target.z = transform.position.z;
        direction = (target - transform.position).normalized;

        // Perpendicular direction for snake movement
        perpendicular = new Vector2(-direction.y, direction.x);

        if (fireType == Shooting.FireType.Straight)
        {
            // Set constant velocity immediately for straight shot
            rb.velocity = direction * maxForce;
        }

        // Destroy the bullet after its lifetime expires
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        if (fireType == Shooting.FireType.Snake)
        {
            currentTime += Time.deltaTime;

            // Calculate force for snake shot with easing
            float t = Mathf.Clamp01(currentTime / accelerationTime);
            float force = Mathf.Lerp(0, maxForce, t * t); // Quadratic easing

            // Snake movement
            float oscillation = Mathf.Sin(Time.time * snakeFrequency) * snakeAmplitude;
            Vector2 snakeVelocity = direction * force + perpendicular * oscillation;
            rb.velocity = snakeVelocity;
        }
    }
}
