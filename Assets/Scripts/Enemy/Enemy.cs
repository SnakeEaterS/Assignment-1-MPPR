using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public BezierCurve bezierCurve; // Reference to the Bézier curve
    public float speed;            // Speed along the curve
    public int health;             // Health points for the enemy
    public int maxHealth;
    private float t = 0f;          // Progress along the curve (0 to 1)
   
    [SerializeField] HealthBar healthBar;

    private void Awake()
    {
        healthBar = GetComponentInChildren<HealthBar>();
    }

    void Start()
    {
        health = maxHealth;
        healthBar.UpdateHealthBar(health, maxHealth);
        if (bezierCurve == null)
        {
            Debug.LogError("BezierCurve is not assigned!");
        }
    }

    void Update()
    {
        if (bezierCurve == null)
            return;

        // Move along the Bézier curve
        t += speed * Time.deltaTime / bezierCurve.GetLength();

        // If the enemy reaches the end of the curve, the player loses
        if (t >= 1f)
        {
            OnReachEnd();
            return;
        }

        // Update the position based on the curve
        transform.position = bezierCurve.GetPoint(t);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        healthBar.UpdateHealthBar(health, maxHealth);

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject); // Destroy the enemy when health reaches zero
    }

    private void OnReachEnd()
    {
        Debug.Log("Enemy reached the end of the curve. Player loses!");
        GameManager.Instance.PlayerLose();
        Destroy(gameObject); // Destroy the enemy
    }
}