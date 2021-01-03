using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitions : MonoBehaviour
{
    public string sceneToLoad;
    public Vector2 playerPosition;
    public PlayerVectorValue playerStorage;
    private Animator anime;

    private void Start()
    {
        anime = GetComponent<Animator>();
    }

    public void startGame()
    {
        playerStorage.initialValue = playerPosition;
        SceneManager.LoadScene("Castle");
        anime.SetTrigger("FadeOut");
    }

    public void playerDied()
    {
        //set player position in fairy cave
        //playerStorage.initialValue.x = 
        playerStorage.initialValue = new Vector2(-1.17f, 0.91f);
        SceneManager.LoadScene("FairyDomain");
        anime.SetTrigger("FadeOut");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerStorage.initialValue = playerPosition;
            SceneManager.LoadScene(sceneToLoad);
            anime.SetTrigger("FadeOut");
        }
    }
}
