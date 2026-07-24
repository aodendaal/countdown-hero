using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private enum GameState { None, Menu, Game }
    private GameState CurrentState = GameState.None;



    [SerializeField] private AudioSource musicSource;
    [Header("Tracks")]
    [SerializeField] private AudioClip menuTrack;
    [SerializeField] private AudioClip gameTrack;

    public void PlayMenuTrack()
    {
        if (CurrentState == GameState.Menu) return;

        CurrentState = GameState.Menu;
        musicSource.clip = menuTrack;
        musicSource.Play();
    }

    public void PlayGameTrack()
    {
        if (CurrentState == GameState.Game) return;

        CurrentState = GameState.Game;
        musicSource.clip = gameTrack;
        musicSource.Play();
    }

    public void StopMusic()
    {
        CurrentState = GameState.None;
        musicSource.Stop();
    }
}
