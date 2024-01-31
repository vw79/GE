using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform _model;
    [SerializeField] private float _maxSpeed = 5f;
    [SerializeField] private float _acceleration = 10f;
    [SerializeField] private float _deceleration = 10f;
    [SerializeField] private float _turnSpeed = 1000f;
    [SerializeField] private float _dashSpeed = 20f;
    [SerializeField] private float _dashDuration = 0.2f;
    [SerializeField] private float _dashCooldown = 1f;

    public float activeTime = 0.2f;
    private MeshTrail _meshTrail;

    private Rigidbody _rb;
    private Animator _animator;
    private Vector3 _input;
    private float _currentSpeed;
    private bool _isDashing;

    private float _dashTimeLeft;
    private float _dashCooldownTimer;
    private Vector3 _lastInputDirection;

    private void ResetMovement()
    {
        _currentSpeed = 0;
    }

    private void OnDisable()
    {
        ResetMovement();
        _animator.SetBool("IsMoving", false);
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _animator.Play("Idle");
        _meshTrail = GetComponent<MeshTrail>();
        
    }

    private void Update()
    {
        GatherInput();
        HandleDashInput();
        Look();
        Animate();
    }

    private void FixedUpdate()
    {
        if (_isDashing)
        {
            Dash();
        }
        else
        {
            Move();
        }
    }

    private void GatherInput()
    {
        _input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        if (_input.magnitude > 0)
        {
            _lastInputDirection = _input; // Update the last input direction whenever there's input
        }
    }

    public Vector3 GetLastInputDirection()
    {
        return _lastInputDirection;
    }

    // Public property to access the turn speed
    public float TurnSpeed
    {
        get { return _turnSpeed; }
    }


    private void HandleDashInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !_isDashing && _dashCooldownTimer <= 0 && _currentSpeed >= 0.1)
        {
            _isDashing = true;
            _dashTimeLeft = _dashDuration;
            _dashCooldownTimer = _dashCooldown;
        }

        if (_isDashing)
        {
            _dashTimeLeft -= Time.deltaTime;
            if (_dashTimeLeft <= 0)
            {
                _isDashing = false;
            }
        }

        if (_dashCooldownTimer > 0)
        {
            _dashCooldownTimer -= Time.deltaTime;
        }
    }

    private void Dash()
    {
        StartCoroutine(_meshTrail.ActivateTrail(activeTime));
        _rb.MovePosition(transform.position + _input.ToIso().normalized * _dashSpeed * Time.deltaTime);
    }

    private void Look()
    {
        if (_input == Vector3.zero) return;

        Vector3 isometricDirection = _input.ToIso().normalized;
        Quaternion targetRotation = Quaternion.LookRotation(isometricDirection, Vector3.up);

        _model.rotation = Quaternion.RotateTowards(_model.rotation, targetRotation, _turnSpeed * Time.deltaTime);
    }


    private void Move()
    {
        if (_input.magnitude > 0)
        {
            _currentSpeed = Mathf.Min(_currentSpeed + _acceleration * Time.deltaTime, _maxSpeed);
        }
        else if (_currentSpeed > 0)
        {
            _currentSpeed = Mathf.Max(_currentSpeed - _deceleration * Time.deltaTime, 0);
        }

        if (_currentSpeed > 0)
        {
            _rb.MovePosition(transform.position + _input.ToIso() * _currentSpeed * Time.deltaTime);
        }
    }

    private void Animate()
    {
        _animator.SetBool("IsMoving", _input.magnitude > 0);
    }


}

public static class Helpers
{
    private static readonly Matrix4x4 IsoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));

    public static Vector3 ToIso(this Vector3 input)
    {
        return IsoMatrix.MultiplyPoint3x4(input);
    }
}