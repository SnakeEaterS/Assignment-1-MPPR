using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    private Camera mainCam;
    private Vector3 mousePos;
    public GameObject bullet;
    public Transform bulletTransform;
    public bool canFire;
    private float timer;
    public float timeBetweenFiring;

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

        if (!canFire)
        {
            timer += Time.deltaTime;
            if (timer > timeBetweenFiring)
            {
                canFire = true;
                timer = 0;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            canFire  =false;
            Instantiate(bullet, bulletTransform.position, Quaternion.identity);
        }
    }
}
