using UnityEngine;
using TMPro;

public class WinZone : MonoBehaviour
{
    [SerializeField] private WeaponShooting weaponShooting; // Reference to WeaponShooting

    private void OnTriggerEnter(Collider other)
    {
        // Check if the entering collider is the player
        if (other.CompareTag("Player"))
        {
            // Assuming your PlayerController script is on the same GameObject as the CharacterController
            PlayerController playerController = other.GetComponent<PlayerController>();

            if (playerController != null)
            {
                // Call a method in PlayerController to handle win condition
                playerController.ActivateWinText();

                // Stop shooting in WeaponShooting
                if (weaponShooting != null)
                {
                    weaponShooting.StopShooting();
                }
            }
        }
    }
}
