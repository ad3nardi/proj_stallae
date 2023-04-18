using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unit_visCollision : MonoBehaviour
{
    public float maxCollisionHeight = 5f; // Maximum height to increase when colliding
    public float collisionDuration = 1f; // Duration for the object to reach the maximum height
    public float returnDuration = 1f; // Duration for the object to return to its original height

    private bool isColliding = false; // Flag to track collision status
    private bool hasReturned = false; // Flag to track if object has returned to original height
    private Vector3 originalPosition; // Original position of the object
    private float currentCollisionTime = 0f; // Current time for collision duration
    private float currentReturnTime = 0f; // Current time for return duration

    private void Start()
    {
        originalPosition = transform.position; // Store the original position of the object
    }

    private void Update()
    {
        // If currently colliding and collision duration not reached, increase Y position
        if (isColliding && currentCollisionTime < collisionDuration)
        {
            float newY = Mathf.Lerp(originalPosition.y, originalPosition.y + maxCollisionHeight, currentCollisionTime / collisionDuration);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
            currentCollisionTime += Time.deltaTime;
        }
        // If currently colliding and collision duration reached, reset current collision time and flag
        else if (isColliding)
        {
            currentCollisionTime = 0f;
        }

        // If not colliding and return duration not reached, decrease Y position
        if (!isColliding && currentReturnTime < returnDuration && hasReturned)
        {
            float newY = Mathf.Lerp(originalPosition.y + maxCollisionHeight, originalPosition.y, currentReturnTime / returnDuration);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
            currentReturnTime += Time.deltaTime;
        }
        // If not colliding and return duration reached, reset current return time and flag
        else if (!isColliding && hasReturned)
        {
            currentReturnTime = 0f;
            hasReturned = false;
        }
    }

    private void FixedUpdate()
    {
        // Check for collision using OverlapSphere
        Collider[] colliders = Physics.OverlapSphere(transform.position, transform.localScale.magnitude);
        bool wasColliding = isColliding; // Store previous collision status
        isColliding = false;
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject != gameObject)
            {
                isColliding = true;
                break;
            }
        }

        // Update hasReturned flag
        if (!isColliding && transform.position.y <= originalPosition.y)
        {
            hasReturned = true;
        }

        // Keep the game object at the maximum height while colliding
        if (isColliding && !wasColliding)
        {
            transform.position = new Vector3(transform.position.x, originalPosition.y + maxCollisionHeight, transform.position.z);
        }
    }
}
