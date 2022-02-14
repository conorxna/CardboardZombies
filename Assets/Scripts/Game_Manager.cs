using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_Manager : MonoBehaviour
{
    // Inspector variables
    [SerializeField] private GameObject gameOverTextObject;
    [SerializeField] private float restartDelay = 5f;
    [SerializeField] private AudioClip startSound;
    [SerializeField] private AudioClip dieSound;
    [SerializeField] private AudioSource audio;

    // Init
    void Start()
    {
        audio.clip = startSound;
        audio.Play();
    }
    
    // Set game over state
    public void EndGame()
    {
        audio.clip = dieSound;
        audio.Play();
        Time.timeScale = 0.3f;
        gameOverTextObject.SetActive(true);
        StartCoroutine(EndAfterDelay());
    }
    
    // Reset game after n seconds
    public static IEnumerator EndAfterDelay()
    {
        yield return new WaitForSeconds(5f);
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
