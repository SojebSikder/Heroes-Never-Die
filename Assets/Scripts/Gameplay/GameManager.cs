using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float slowdonwnFactor = 0.05f;
    public float slowdownLength = 10f; //2f

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
}
