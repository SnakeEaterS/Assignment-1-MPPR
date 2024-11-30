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
    private CircleCollider2D circleCollider;
    private Vector2 direction;
    private Vector2 perpendicular;
    private float currentTime = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();

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
        // Destroy(gameObject, lifeTime);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collided object is an enemy
        if (collision.CompareTag("Blue Enemy") || collision.CompareTag("Red Enemy")) // If the bullet hits a valid target (red/blue enemy), it will continue
        {
            // Check if the bullet's tag color matches the enemy's tag color
            if ((collision.CompareTag("Red Enemy") && gameObject.CompareTag("Red Bullet")) || (collision.CompareTag("Blue Enemy") && gameObject.CompareTag("Blue Bullet")))
            {
                Enemy enemy = collision.GetComponent<Enemy>();
                if (fireType == Shooting.FireType.Straight)
                {
                    int damage1 = 10;
                    enemy.TakeDamage(damage1);
                    Debug.Log($"{collision.gameObject.name} took {damage1} damage from Straight fire. Remaining health: {enemy.health}");
                    Destroy(gameObject);
                }
                else
                {
                    int damage2 = 5;
                    enemy.TakeDamage(damage2);
                    Debug.Log($"{collision.gameObject.name} took {damage2} damage from Snake fire. Remaining health: {enemy.health}");
                }
            }
        }
    }
}
