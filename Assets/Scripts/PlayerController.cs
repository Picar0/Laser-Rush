using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 500f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private GameObject winText;
    [SerializeField] private GameObject failText;

    private PlayerControls playerControls;
    private CharacterController controller;
    private bool groundedPlayer;
    private Vector3 playerVelocity;


    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();
            playerControls.Player.Enable();
        }

        if (controller == null)
        {
            controller = GetComponent<CharacterController>();
        }
    }

    private void OnDisable()
    {
        playerControls.Player.Disable();
    }

    private void Update()
    {
        HandleMovement();
        HandleRotation();
        StayOnGround();
    }

    private void StayOnGround()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
    }

    private void HandleMovement()
    {
        Vector2 moveInput = playerControls.Player.Move.ReadValue<Vector2>();
        Vector3 moveDirection = new Vector3(moveInput.x, 0f, moveInput.y).normalized;

        // Apply gravity to the vertical component of playerVelocity
        if (!groundedPlayer)
        {
            playerVelocity.y += Physics.gravity.y * Time.deltaTime;
        }

        // Move the character controller with consideration to the current vertical velocity
        controller.Move((moveDirection * moveSpeed + playerVelocity) * Time.deltaTime);

        // Check if the player is grounded after the movement
        groundedPlayer = controller.isGrounded;

        // Reset the vertical velocity if grounded
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
    }


    private void HandleRotation()
    {
        Vector2 mouseInput = playerControls.Player.Mouse.ReadValue<Vector2>();

        Ray ray = Camera.main.ScreenPointToRay(mouseInput);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Visualize the ray in the Scene view
            Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red);

            Vector3 targetPosition = hit.point;
            targetPosition.y = transform.position.y; // Keep the same height
            Vector3 lookDirection = targetPosition - transform.position;

            if (lookDirection.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);

                // Smoothly interpolate between current rotation and target rotation
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }


    public void ActivateWinText()
    {
        if (winText != null)
        {
            winText.SetActive(true);
            Invoke("RestartScene", 1f); // Restart scene after 1 second
        }
    }

    public void ActivateFailedText()
    {
        if (failText != null)
        {
            failText.SetActive(true);
            Invoke("RestartScene", 1f); // Restart scene after 1 second
        }
    }

    private void RestartScene()
    {
        // Reload the current scene
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }
}
