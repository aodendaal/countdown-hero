using Unity.Cinemachine;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField] private GameObject startMenu;
    [SerializeField] private GameObject timeOutMenu;

    [Header("Audio Source")]
    [SerializeField] private AudioSource audioSource;

    [Header("Sounds")]
    [SerializeField] private AudioClip startClip;
    [SerializeField] private AudioClip tryAgainClip;

    public void OnStartGame()
    {
        audioSource.clip = startClip;
        audioSource.Play();

        startMenu.SetActive(false);
        timeOutMenu.SetActive(false);

        GameStateManager.Instance.StartNewGame();
    }

    public void OnTryAgain()
    {
        audioSource.clip = tryAgainClip;
        audioSource.Play();

        startMenu.SetActive(false);
        timeOutMenu.SetActive(false);

        GameStateManager.Instance.StartNewGame();
    }

    public void OnQuitGame()
    {
        Application.Quit();
    }

}
