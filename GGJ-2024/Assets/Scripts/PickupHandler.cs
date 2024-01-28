using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PickupHandler : MonoBehaviour
{
    //[SerializeField] private GameObject _pickupGameObject;
    private Rigidbody2D _pickupRigidbody2D;
    private bool _isHoldingPickup = false;
    private Vector2 _mousePosition;
    [SerializeField]private float speed = 5f;

    //DEBUG ONLY
    private Vector3 _startPosition;

    void Start()
    {
        _startPosition = transform.root.position;

        _pickupRigidbody2D = GetComponent<Rigidbody2D>();

        if (_pickupRigidbody2D == null)
        {
            Debug.LogError("PickupHandler: _pickupRigidbody2D is null");
        }
    }

    void OnMouseDown()
    {
        if (!_isHoldingPickup)
        {
            _isHoldingPickup = true;
            //_pickupRigidbody2D.gravityScale = 0;
        }
    }

    void OnMouseDrag()
    {
        if (_isHoldingPickup)
        {
            // Convert mouse position to world space
            Vector2 _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // Set the object's position to the mouse position
            Vector2 _moveTowardsMouse = _mousePosition - _pickupRigidbody2D.position;
            //_pickupRigidbody2D.velocity = _moveTowardsMouse * speed;
            _pickupRigidbody2D.MovePosition(_mousePosition);
        }
    }

    void OnMouseUp()
    {
        if (_isHoldingPickup)
        {
            _isHoldingPickup = false;
            //_pickupRigidbody2D.gravityScale = 1;

            // Calculate throw direction based on initial and final mouse positions
            Vector2 throwDirection = (_pickupRigidbody2D.position - _mousePosition).normalized;

            // Apply a force to the object to simulate throwing
            float throwForce = 10.0f; // Adjust this value as needed
            _pickupRigidbody2D.velocity = throwDirection * throwForce;

            EventManager.Instance.OnRelease?.Invoke();
        }
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            // reset the pickup's position to the start position
            transform.root.position = _startPosition;

            // reset the pickup's velocity
            _pickupRigidbody2D.velocity = Vector2.zero;

            // reset the pickup's gravity
            //_pickupRigidbody2D.gravityScale = 1;
        }
    }
}
