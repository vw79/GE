using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform _model;
    [SerializeField] private float _speed = 5;
    [SerializeField] private float _turnSpeed = 1000;
    private Rigidbody _rb;
    private Animator _animator;  
    private Vector3 _input;

    private void Awake()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        _rb = playerObject.GetComponent<Rigidbody>();
        _animator = playerObject.GetComponent<Animator>();  
        _animator.Play("Idle");  
    }

    private void Update()
    {
        GatherInput();
        Look();
        Animate();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void GatherInput()
    {
        _input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
    }

    private void Look()
    {
        if (_input == Vector3.zero) return;

        Quaternion rot = Quaternion.LookRotation(_input.ToIso(), Vector3.up);
        _model.rotation = Quaternion.RotateTowards(_model.rotation, rot, _turnSpeed * Time.deltaTime);
    }

    private void Move()
    {
        _rb.MovePosition(transform.position + _input.ToIso() * _input.normalized.magnitude * _speed * Time.deltaTime);
    }

    private void Animate()
    {
        bool isMoving = _input.magnitude > 0;
        _animator.SetBool("IsMoving", isMoving);
    }
}

public static class Helpers
{
    private static Matrix4x4 _isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
    public static Vector3 ToIso(this Vector3 input) => _isoMatrix.MultiplyPoint3x4(input);
}

