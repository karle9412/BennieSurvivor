using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float stopDistance = 0.6f;

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
}
