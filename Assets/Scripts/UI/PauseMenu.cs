using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;


    // store state of Enemy and animator componenet value in key value pair
    private Dictionary<GameObject, bool> enemyState;
    private Dictionary<GameObject, bool> animatorState;

    // Start is called before the first frame update
    void Start()
    {
        enemyState = new Dictionary<GameObject, bool>();
        animatorState = new Dictionary<GameObject, bool>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;

        // start all animations
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Enemies");
        foreach (GameObject target in targets)
        {
            target.GetComponent<Enemy>().enabled = enemyState[target];
            target.GetComponent<Animator>().enabled = animatorState[target];
        }

        // start all player animations
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject target in players)
        {
            target.GetComponent<PlayerController>().enabled = true;
            target.GetComponent<Animator>().enabled = true;
        }
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;

        // stop all animations
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Enemies");

        foreach (GameObject target in targets)
        {
            enemyState.Add(target, target.GetComponent<Enemy>().enabled);
            animatorState.Add(target, target.GetComponent<Animator>().enabled);

            if (target.GetComponent<Enemy>().enabled == true)
            {
                target.GetComponent<Enemy>().enabled = false;
            }
            if (target.GetComponent<Animator>().enabled == true)
            {
                target.GetComponent<Animator>().enabled = false;
            }
        }

        // stop all player animations
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject target in players)
        {
            target.GetComponent<PlayerController>().enabled = false;
            target.GetComponent<Animator>().enabled = false;
        }
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
