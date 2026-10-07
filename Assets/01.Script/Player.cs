using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 5f;


    private Vector2 movement;

    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    private Animator _animation;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animation = GetComponent<Animator>();

    }

    private void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        movement = new Vector2(horizontalInput, verticalInput);


        if (movement.magnitude > 0)
            movement.Normalize();

        if (horizontalInput < 0)
            _spriteRenderer.flipX = true;
        else if (horizontalInput > 0)
            _spriteRenderer.flipX = false;

        _animation.SetFloat("Speed", movement.magnitude);
    }

    private void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

}
