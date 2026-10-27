using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float stopDistance = 0.6f;

    [SerializeField] private int contactDamage = 10;
    [SerializeField] private float damageInterval = 1f;

    private float nextDamageTime = 0f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (target == null)
            return;

        Vector2 targetPosition = target.position;
        Vector2 direction = targetPosition - rb.position;
        float distance = direction.magnitude;

        if (distance <= stopDistance)
            return;

        float moveDistance = Mathf.Min(moveSpeed * Time.fixedDeltaTime, distance - stopDistance);

        rb.MovePosition(rb.position + direction.normalized * moveDistance);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stopDistance);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (target == null)
            return;

        if (other.attachedRigidbody == null || other.attachedRigidbody.transform != target)
            return;

        if (Time.time < nextDamageTime)
            return;

        if (!target.TryGetComponent<Health>(out Health health))
            return;

        if (health.Isdead)
            return;

        nextDamageTime = Time.time + damageInterval;
        health.TakeDamage(contactDamage);
    }
}
