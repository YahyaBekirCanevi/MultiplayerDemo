using UnityEngine;
using FishNet.Object;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : NetworkBehaviour
{
    private CharacterController controller;
    private Vector3 movementDirection = Vector3.zero;

    [Header("Base Setup")]
    [SerializeField] private float walkSpeed = 6f;
    [SerializeField] private float runSpeed = 12f;
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField, ReadOnly] private bool isGrounded = false;
    [SerializeField, ReadOnly] private Vector3 lastActivePosition;
    private Camera playerCamera;

    private void OnEnable()
    {
        if (base.IsOwner)
        {
            playerCamera = Camera.main;
            playerCamera.transform.parent.GetComponent<CameraController>().Player = transform;
            controller = GetComponent<CharacterController>();
            GameManagementController.Instance.ToggleMenu(false);
        }
        else GetComponent<Player>().ActivatePlayer(GetComponent<Player>(), false);
    }
    private void Update()
    {
        SwitchCursorLock();
        CheckGrounded();
        ReplaceFallenPlayer();
    }
    private void SwitchCursorLock()
    {
        if (!GameManagementController.Instance) return;
        Cursor.visible = GameManagementController.Instance.isMenuOpen;
        Cursor.lockState = GameManagementController.Instance.isMenuOpen ? CursorLockMode.None : CursorLockMode.Locked;
    }
    private void CheckGrounded()
    {
        RaycastHit hit;
        isGrounded = Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit, 10, groundLayer);
        isGrounded = isGrounded && hit.distance < 2.5f && hit.distance > 0;
    }
    private void ReplaceFallenPlayer()
    {
        if (isGrounded)
        {
            lastActivePosition = transform.position + Vector3.up - movementDirection * .6f;
        }
        if (GameManagementController.Instance &&
            transform.position.y < GameManagementController.Instance.lowestPoint)
        {
            transform.position = lastActivePosition;
        }
        else
        {
            CorrespondPlayerMovement();
        }
    }
    private void CorrespondPlayerMovement()
    {
        if (playerCamera && !GameManagementController.Instance.isMenuOpen)
        {
            movementDirection =
            playerCamera.transform.forward * Input.GetAxis("Vertical")
            + playerCamera.transform.right * Input.GetAxis("Horizontal");
            movementDirection.y = 0;
        }
        else
        {
            movementDirection = Vector3.zero;
        }

        if (movementDirection != Vector3.zero)
            transform.forward = movementDirection.normalized;
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        movementDirection = movementDirection.normalized * currentSpeed;
        movementDirection.y = gravity;
        controller.Move(movementDirection * Time.deltaTime);
    }
}
