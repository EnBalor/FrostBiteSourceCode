using System;
using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Moverment")]
    public float moveSpeed;
    public float jumpPower;
    private Vector2 curMovementInput;
    public LayerMask groundLayerMask;

    [Header("Look")]
    public Transform cameraContainer;
    public float minLook;
    public float maxLook;
    private float camCurXRot;
    public float lookSensitivity;
    private Vector2 mouseDelta;
    public bool canLook = true;

    public Action inventory;
    private Rigidbody _rigidbody;
    private bool isWalk = false;
    private Coroutine footStepCoroutine;
    private Coroutine windBlowCoroutine;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        windBlowCoroutine = StartCoroutine(Windblow());
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        CameraLook();
    }

    void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed;
        dir.y = _rigidbody.velocity.y;

        _rigidbody.velocity = dir;
    }

    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minLook, maxLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
            
            if(!isWalk)
            {
                isWalk = true;
                footStepCoroutine = StartCoroutine(FootStep());
            }
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
            isWalk = false;

            if(footStepCoroutine != null)
            {
                StopCoroutine(footStepCoroutine);
                footStepCoroutine = null;
            }
        }
        
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            _rigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
            AudioManager.instance.PlaySFX("Jump");
        }
    }

    bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
                new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.05f), Vector3.down),
                new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.05f), Vector3.down),
                new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.05f), Vector3.down),
                new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.05f), Vector3.down)
        };

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 1f, groundLayerMask))
            {
                return true;
            }
        }

        return false;
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            inventory?.Invoke();
            AudioManager.instance.PlaySFX("OpenInven");
            ToggleCursor();
        }
    }

    public void ToggleCursor()
    {
        bool onMouse = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = onMouse ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !onMouse;
    }

    private IEnumerator FootStep()
    {
        while(isWalk)
        {
            AudioManager.instance.PlaySFX("snow_footstep");

            yield return new WaitForSeconds(0.5f);
        }
    }

    private IEnumerator Windblow()
    {
        while(true)
        {
            AudioManager.instance.PlaySFX("Wind");
            yield return new WaitForSeconds(291f);
        }
    }
}



    

