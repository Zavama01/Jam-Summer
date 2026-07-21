using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class DeflectBoxData
{
    public Vector2 boxOffset = Vector2.zero;
    public Vector2 boxSize = Vector2.one;
    public float boxAngle = 0f;
    public Vector2 deflectDirection = Vector2.right;
}

public class PlayerDeflectBoxes : MonoBehaviour
{
    private PlayerInput _playerInput;
    private InputAction _deflectAction;

    [SerializeField] private List<DeflectBoxData> _boxes = new();
    [SerializeField] private LayerMask _targetMask;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
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

    private void DeflectSlash()
    {
        bool foundAnyHit = false;

        foreach (var box in _boxes)
        {
            Vector2 boxCenter = (Vector2)transform.position + box.boxOffset;

            Collider2D[] hits = Physics2D.OverlapBoxAll(
                boxCenter,
                box.boxSize,
                box.boxAngle,
                _targetMask
            );

            if (hits.Length <= 0) continue;

            foundAnyHit = true;

            foreach (Collider2D hit in hits)
            {
                Debug.Log("Dentro al box: " + hit.name);

                EventBus<ProjectilePayload>.Raise(
                    "OnDeflect",
                    new ProjectilePayload { direction = box.deflectDirection }
                );
            }
        }

        if (!foundAnyHit)
        {
            Debug.Log("Nessun collider in nessun box");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (_boxes == null) return;

        foreach (var box in _boxes)
        {
            Vector3 center = transform.position + (Vector3)box.boxOffset;
            Quaternion rotation = Quaternion.Euler(0f, 0f, box.boxAngle);

            Gizmos.matrix = Matrix4x4.TRS(center, rotation, Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, box.boxSize);
        }

        Gizmos.matrix = Matrix4x4.identity;
    }
}