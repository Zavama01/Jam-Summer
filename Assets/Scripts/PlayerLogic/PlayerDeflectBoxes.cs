using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerDeflectBoxes : MonoBehaviour
{
    private PlayerInput _playerInput;

    [SerializeField] private Vector2 _boxOffset = Vector2.zero;
    [SerializeField] private Vector2 _boxSize = Vector2.one;
    [SerializeField] private float _boxAngle = 0f;
    [SerializeField] private LayerMask _targetMask;

    private InputAction _deflectAction;

    private void Awake()
    {
        _playerInput.GetComponent<PlayerInput>();
        _deflectAction = _playerInput.actions["Deflect"];
    }

    private void Update()
    {
        if (_deflectAction != null && _deflectAction.WasPressedThisFrame())
        {
            DeflectSlash();
        }
    }

    private void DeflectSlash()
    {
        Vector2 boxCenter = (Vector2)transform.position + _boxOffset;

        Collider2D[] hits = Physics2D.OverlapBoxAll(
            boxCenter,
            _boxSize,
            _boxAngle,
            _targetMask
        );

        if (hits.Length == 0)
        {
            Debug.Log("Nessun collider nel box");
            return;
        }

        foreach (Collider2D hit in hits)
        {
            Debug.Log("Dentro al box: " + hit.name);
        }

        EventBus<ProjectilePayload>.Raise("OnDeflect", new ProjectilePayload());
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        Vector3 center = transform.position + (Vector3)_boxOffset;
        Quaternion rotation = Quaternion.Euler(0f, 0f, _boxAngle);

        Gizmos.matrix = Matrix4x4.TRS(center, rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, _boxSize);
    }
}

