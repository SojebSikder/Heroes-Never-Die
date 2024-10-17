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

        // check if all objects is visible
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Enemies");

        Camera camera = Camera.main;
        foreach (GameObject target in targets)
        {
            if (IsVisible(camera, target))
            {
                target.GetComponent<Enemy>().enabled = true;
                target.GetComponent<Animator>().enabled = true;
            }
        }


    }

    public void DoSlowmotion()
    {
        Time.timeScale = slowdonwnFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }


    private bool IsVisible(Camera camera, GameObject target)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
        var point = target.transform.position;

        foreach (var plane in planes)
        {
            if (plane.GetDistanceToPoint(point) < 0)
            {
                return false;
            }
        }
        return true;
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

    public void GoMainMenu()
    {
        SceneManager.LoadScene("GameoverMenu");
    }


}
