using UnityEngine;
using Cinemachine;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject menuButton;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject enemies;
    [SerializeField] private GameObject gameplayCam;
    [SerializeField] private AudioSource backgroundMusic;

    private void Awake()
    {
        backgroundMusic.Play();
    }

    public void StartGame()
    {
        menu.SetActive(false);
        menuButton.SetActive(false);
        player.SetActive(true);
        enemies.SetActive(true);
        gameplayCam.SetActive(true);

    }

    public void Quit()
    {
        Application.Quit();
    }




}
