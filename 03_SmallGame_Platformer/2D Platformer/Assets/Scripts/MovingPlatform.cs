using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float platformSpeed = 2f;
    public List<Transform> positions;

    private int currentIndex = 0;
    private Transform nextPosition;
    private Rigidbody2D rb;
    public Vector2 platformVelocity;

    private Vector2 lastPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (positions != null && positions.Count > 1)
        {
            currentIndex = 1;
            nextPosition = positions[currentIndex];
        }

        lastPosition = rb.position;
    }

    void FixedUpdate()
    {
        MovePlatform();

        platformVelocity = (rb.position - lastPosition) / Time.fixedDeltaTime; // meters per second
        lastPosition = rb.position;

        
    }

    void MovePlatform()
    {
        if (nextPosition == null)
            return;

        Vector2 direction = (nextPosition.position - transform.position).normalized;
        Vector2 targetPos = rb.position + direction * platformSpeed * Time.fixedDeltaTime;

        float distance = Vector2.Distance(rb.position, nextPosition.position);

        if (distance < 0.05f)
        {
            currentIndex = (currentIndex + 1) % positions.Count;
            nextPosition = positions[currentIndex];
        }

        rb.MovePosition(targetPos);
    }
}
