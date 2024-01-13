using UnityEngine;

public class ObstacleMover : MonoBehaviour
{
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    [SerializeField] private float movementSpeed = 2.0f;  // Adjust this speed as needed
    [SerializeField] private Animator animator;

    private void Update()
    {
        // Calculate the ping-pong fraction of the journey covered
        float pingPongTime = Mathf.PingPong(Time.time * movementSpeed / Vector3.Distance(startPoint.position, endPoint.position), 1.0f);

        // Move the obstacle along the path using ping-pong
        transform.position = Vector3.Lerp(startPoint.position, endPoint.position, pingPongTime);

        // Calculate the look direction based on ping-pong time
        Vector3 lookDirection = Vector3.Lerp(startPoint.position, endPoint.position, pingPongTime);
        lookDirection.y = transform.position.y; // Keep the same height level
        transform.LookAt(lookDirection);

        // Update the speed parameter in the Animator based on pingPongTime
        float speedParam = Mathf.Lerp(0.0f, 1.0f, pingPongTime);
        animator.SetFloat("Speed", speedParam);
    }
}
