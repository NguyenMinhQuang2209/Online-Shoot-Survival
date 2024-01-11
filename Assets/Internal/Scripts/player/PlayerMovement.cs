using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.XR;

public class PlayerMovement : NetworkBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float runSpeed = 3f;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    float currentSpeed = 0f;
    private Animator animator;
    private Transform character;


    public override void OnNetworkSpawn()
    {
        rb = GetComponent<Rigidbody2D>();
        foreach (Transform child in transform)
        {
            if (child.TryGetComponent<Animator>(out animator))
            {
                character = child;
                break;
            }
        }
        if (CameraCompoundController.instance != null)
        {
            if (virtualCamera.TryGetComponent<CinemachineConfiner2D>(out var compound))
            {
                compound.m_BoundingShape2D = CameraCompoundController.instance.compoundCamera;
            }
        }

        if (SceneController.instance != null)
        {
            SceneController.instance.ChangeSceneEvent += HandleChangeSceneEvent;
        }

        if (IsOwner)
        {
            virtualCamera.enabled = true;
            virtualCamera.Priority = 1;

        }
        else
        {
            virtualCamera.enabled = false;
            virtualCamera.Priority = 0;
        }
    }

    private void HandleChangeSceneEvent(object sender, EventArgs e)
    {
        if (CameraCompoundController.instance != null)
        {
            if (virtualCamera.TryGetComponent<CinemachineConfiner2D>(out var compound))
            {
                compound.m_BoundingShape2D = CameraCompoundController.instance.compoundCamera;
            }
        }
    }

    private void FixedUpdate()
    {
        if (!IsOwner)
        {
            return;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Movement(new(horizontal, vertical));
    }
    private void Movement(Vector2 input)
    {

        if (GameController.instance != null && !GameController.instance.CanMove())
        {
            return;
        }

        currentSpeed = moveSpeed;
        bool running = false;
        if (input.sqrMagnitude >= 0.1f)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                currentSpeed = runSpeed;
                running = true;
            }
            character.rotation = Quaternion.Euler(0f, input.x < 0f ? 180f : 0f, 0f);
            rb.MovePosition(rb.position + currentSpeed * Time.fixedDeltaTime * input.normalized);
        }
        if (animator != null)
        {
            animator.SetFloat("Speed", input.sqrMagnitude >= 0.1f ? 1f : 0f);
            animator.SetBool("Run", running);
        }
    }
}
