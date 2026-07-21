using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private float speed = 10f;

    private InputAction moveAction;
    private Rigidbody2D rb;
    private Vector2 moveInput;

    private void Awake()
    {
        moveAction = playerInput.actions["Move"];
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();

        if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
        {
            moveInput = new Vector2(Mathf.Sign(input.x), 0f);
        }
        else if (Mathf.Abs(input.y) > 0f)
        {
            moveInput = new Vector2(0f, Mathf.Sign(input.y));
        }
        else
        {
            moveInput = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        Vector2 delta = moveInput * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + delta);
    }
}