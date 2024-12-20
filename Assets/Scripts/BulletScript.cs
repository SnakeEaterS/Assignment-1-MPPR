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
    private SpriteRenderer spriteRenderer;
    private Shooting shooting;  // Reference to the Shooting script

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        shooting = Shooting.Instance;

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
            // Snake movement
            float oscillation = Mathf.Sin(Time.time * snakeFrequency) * snakeAmplitude;
            Vector2 snakeVelocity = direction * maxForce + perpendicular * oscillation;
            rb.velocity = snakeVelocity;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collided object is an enemy
        if (collision.CompareTag("Blue Enemy") || collision.CompareTag("Red Enemy")) // If the bullet hits a valid target (red/blue enemy), it will continue
        {
            if (gameObject.CompareTag("Powerup")) // If the power-up is currently active, shoots deadly bullets that insta-kills any enemy
            {
                // Destroys the enemy instantly and the bullet itself
                Destroy(gameObject);
                Destroy(collision.gameObject);
            }
            // Check if the bullet's tag color matches the enemy's tag color
            else if ((collision.CompareTag("Red Enemy") && gameObject.CompareTag("Red Bullet")) || (collision.CompareTag("Blue Enemy") && gameObject.CompareTag("Blue Bullet")))
            {
                Enemy enemy = collision.GetComponent<Enemy>();
                if (fireType == Shooting.FireType.Straight)
                {
                    int damage1 = 10;
                    enemy.TakeDamage(damage1);
                    Destroy(gameObject);
                }
                else
                {
                    int damage2 = 5;
                    enemy.TakeDamage(damage2);
                }
            }
        }

        else if (collision.CompareTag("Powerup")) // Power up increases firing rate by 2x for 5 seconds
        {
            shooting.powerUpObtained = true; // Sets the power up obtained status to true so it can run the power-up coroutine in the Shooting.cs script
            shooting.powerUpActive = true;
            // Destroys both the bullet and the power-up
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }
}
    
