using UnityEngine;
using UnityEngine.InputSystem;



public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private float speed = 10f;

    private InputAction moveAction;

    private void Awake()
    {
        moveAction = playerInput.actions["Move"];
    }

    private void Update()
    {
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        Vector3 delta = new Vector3(moveInput.x, moveInput.y, 0f) * speed * Time.deltaTime;
        transform.position += delta;
    }
}


