using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController _controller;
    private Animator _animator;

    private Vector3 playerVelocity;
    private bool groundedPlayer;

    [SerializeField]
    private float playerSpeed = 10.0f;
    [SerializeField]
    private float jumpHeight = 25.0f;
    [SerializeField]
    private float gravityValue = -9.81f;

    private Dictionary<Vector3, float> _rotationMap;
    private float movement = 0f;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();

        InitializeRotationMap();

    }

    void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        groundedPlayer = _controller.isGrounded;

        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        movement = Mathf.Abs(move.x) + Mathf.Abs(move.z);

        if (movement != 0)
        {
            var rotation = -1.0f;

            _rotationMap.TryGetValue(move, out rotation);

            if (rotation != -1.0f)
                transform.rotation = Quaternion.Euler(0, rotation, 0);
        }

        if (move != Vector3.zero)
        {
            _controller.Move(move * Time.deltaTime * playerSpeed);

            gameObject.transform.forward = move;
        }

        if (Input.GetButtonDown("Jump") && groundedPlayer)
            playerVelocity.y += jumpHeight;

        playerVelocity.y += gravityValue * Time.deltaTime;
        _controller.Move(playerVelocity * Time.deltaTime);

        _animator.SetFloat("Movement", movement);
        _animator.SetBool("Grounded", _controller.isGrounded);
    }

    private void InitializeRotationMap()
    {
        _rotationMap = new Dictionary<Vector3, float>();

        _rotationMap.Add(new Vector3(0, 0, 1), 0);
        _rotationMap.Add(new Vector3(0, 0, -1), 180);
        _rotationMap.Add(new Vector3(1, 0, 0), 90);
        _rotationMap.Add(new Vector3(-1, 0, 0), -90);
        _rotationMap.Add(new Vector3(-1, 0, 1), -45);
        _rotationMap.Add(new Vector3(1, 0, 1), 45);
        _rotationMap.Add(new Vector3(1, 0, -1), 135);
        _rotationMap.Add(new Vector3(-1, 0, -1), -135);
    }
}