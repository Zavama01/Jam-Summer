using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDeflectBoxes : MonoBehaviour
{
    [Serializable]
    public class DeflectBoxData
    {
        [Header("Shape")]
        public Vector2 localOffset = Vector2.right;
        public Vector2 size = Vector2.one;
        public float localAngle = 0f;

        [Header("Deflect")]
        public Vector2 localDeflectDirection = Vector2.right;
    }

    public enum FacingDirection
    {
        Right,
        Up,
        Left,
        Down
    }

    private PlayerInput _playerInput;
    private InputAction _deflectAction;

    [Header("Detection")]
    [SerializeField] private List<DeflectBoxData> _boxes = new();
    [SerializeField] private LayerMask _targetMask;

    [Header("Facing")]
    [SerializeField] private FacingDirection _currentFacing = FacingDirection.Right;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();

        if (_playerInput != null)
            _deflectAction = _playerInput.actions["Deflect"];
    }

    private void OnEnable()
    {
        _deflectAction?.Enable();

    }

    private void OnDisable()
    {
        _deflectAction?.Disable();
    }

    private void Update()
    {
        if (_deflectAction != null && _deflectAction.WasPressedThisFrame())
        {
            DeflectSlash();
        }
    }

    public void SetFacing(Vector2 input)
    {
        if (input == Vector2.zero) return;

        if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
        {
            _currentFacing = input.x > 0f ? FacingDirection.Right : FacingDirection.Left;
        }
        else
        {
            _currentFacing = input.y > 0f ? FacingDirection.Up : FacingDirection.Down;
        }
    }

    public void SetFacing(FacingDirection newFacing)
    {
        _currentFacing = newFacing;
    }

    private void DeflectSlash()
    {
        if (_boxes == null || _boxes.Count == 0)
        {
            Debug.LogWarning("Nessun DeflectBox configurato.");
            return;
        }

        bool hitSomething = false;

        float facingAngle = GetFacingAngle();
        Quaternion facingRotation = Quaternion.Euler(0f, 0f, facingAngle);

        foreach (DeflectBoxData box in _boxes)
        {
            Vector2 rotatedOffset = facingRotation * box.localOffset;
            Vector2 worldDeflectDirection = (facingRotation * box.localDeflectDirection).normalized;
            float worldAngle = box.localAngle + facingAngle;

            Vector2 boxCenter = (Vector2)transform.position + rotatedOffset;

            Collider2D[] hits = Physics2D.OverlapBoxAll(
                boxCenter,
                box.size,
                worldAngle,
                _targetMask
            );

            if (hits.Length == 0)
                continue;

            hitSomething = true;

            foreach (Collider2D hit in hits)
            {
                Debug.Log($"Dentro al box: {hit.name}");

                EventBus<ProjectilePayload>.Raise(
                    "OnDeflect",
                    new ProjectilePayload
                    {
                        direction = worldDeflectDirection
                    }
                );
            }
        }

        if (!hitSomething)
        {
            Debug.Log("Nessun collider in nessun box");
        }
    }

    private float GetFacingAngle()
    {
        return _currentFacing switch
        {
            FacingDirection.Right => 0f,
            FacingDirection.Up => 90f,
            FacingDirection.Left => 180f,
            FacingDirection.Down => 270f,
            _ => 0f
        };
    }

    private void OnDrawGizmos()
    {
        if (_boxes == null) return;

        Gizmos.color = Color.red;

        float facingAngle = GetFacingAngle();
        Quaternion facingRotation = Quaternion.Euler(0f, 0f, facingAngle);

        foreach (DeflectBoxData box in _boxes)
        {
            Vector2 rotatedOffset = facingRotation * box.localOffset;
            float worldAngle = box.localAngle + facingAngle;

            Vector3 center = transform.position + (Vector3)rotatedOffset;
            Quaternion rotation = Quaternion.Euler(0f, 0f, worldAngle);

            Gizmos.matrix = Matrix4x4.TRS(center, rotation, Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, box.size);
        }

        Gizmos.matrix = Matrix4x4.identity;
    }
}