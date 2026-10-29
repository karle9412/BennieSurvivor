using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float moveSpeed = 5f;

    [SerializeField] private int contactDamage = 10;
    [SerializeField] private float damageInterval = 1f;

    private float nextDamageTime = 0f;
    private bool isContactingTarget = false;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (target == null || !target.gameObject.activeInHierarchy)
            return;

        if (isContactingTarget)
        {
            TryContactDamage();
            return;
        }

        Vector2 targetPosition = target.position;
        Vector2 direction = targetPosition - rb.position;
        float distance = direction.magnitude;

        float moveDistance = Mathf.Min(moveSpeed * Time.fixedDeltaTime, distance);

        rb.MovePosition(rb.position + direction.normalized * moveDistance);
    }

    private void TryContactDamage()
    {
        if (Time.time < nextDamageTime)
            return;

        if (!target.TryGetComponent<Health>(out Health health))
            return;

        if (health.Isdead)
            return;

        nextDamageTime = Time.time + damageInterval;
        health.TakeDamage(contactDamage);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (target == null)
            return;

        if (collision.rigidbody == null || collision.rigidbody.transform != target)
            return;

        isContactingTarget = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (target == null)
            return;

        if (collision.rigidbody == null || collision.rigidbody.transform != target)
            return;

        isContactingTarget = false;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
