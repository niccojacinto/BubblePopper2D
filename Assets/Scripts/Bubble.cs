using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bubble : MonoBehaviour
{

    public float lifetime = 1f;
    public bool useLifetime = false;

    Rigidbody2D rb2d;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();

    }

    void FixedUpdate()
    {
        Main.Instance.CheckBounds(this);
    }

    private void Start()
    {
        if (useLifetime)
        {
            StartCoroutine(DestroyAfterDelayCR());
        }

    }


    public void Wobble()
    {
        StartCoroutine(WobbleCR());
    }

    private void OnMouseDown()
    {
        Main.Instance.PopOne();
        Pop();
    }

    public void Pop()
    {
        BubbleSpawner.Instance.Pop(this);
        SoundManager.Instance.PlayPop();
    }

    IEnumerator DestroyAfterDelayCR()
    {
        yield return new WaitForSeconds(lifetime);
        Pop();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Get the current velocity.
        Vector3 velocity = rb2d.velocity.normalized;

        // Introduce some randomness to the reflection direction.
        float randomAngle = Random.Range(-45f, 45f); // Adjust the range based on your preference.

        // Calculate the reflected velocity with a random angle added.
        Vector3 reflectedVelocity = Quaternion.Euler(0, 0, randomAngle) * velocity * rb2d.velocity.magnitude;

        // Apply the reflected velocity to the rigidbody.
        rb2d.velocity = reflectedVelocity;
    }

    IEnumerator WobbleCR()
    {
        Vector2 startScale = transform.localScale;

        float elapsed = 0;

        while (true)
        {
            float xScale = 0.2f * Mathf.Sin(10 * elapsed + 0.2f) + startScale.x;
            float yScale = 0.2f * Mathf.Sin(10 * elapsed) + startScale.y;
            elapsed += Time.deltaTime;
            transform.localScale = new Vector2(xScale, yScale);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        
    }
}
