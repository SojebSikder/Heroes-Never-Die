using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float slowdonwnFactor = 0.05f;
    public float slowdownLength = 10f; //2f

    public static GameManager Instance;

    // player info
    public int currentHealth;

    private void Awake()
    {
        // start of new code
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        // end of new code

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        Time.timeScale += (1f / slowdownLength) * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
    }

    public void DoSlowmotion()
    {
        Time.timeScale = slowdonwnFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    public void SaveGame()
    {
        SaveSystem.SaveGame(this);
    }

    public void LoadGame()
    {
        GameData data = SaveSystem.LoadGame();
        currentHealth = data.currentHealth;
    }

    // go to next level
    public void GoLevel2()
    {
        SceneManager.LoadScene("Scene2");
    }


}
