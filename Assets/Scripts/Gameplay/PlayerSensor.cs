using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSensor : MonoBehaviour
{
    private int colCount = 0;
    private float disableTimer;

    private void OnEnable()
    {
        colCount = 0;
    }

    public bool State()
    {
        if (disableTimer > 0)
        {
            return false;
        }
        return colCount > 0;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        colCount++;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        colCount--;
    }

    // Update is called once per frame
    void Update()
    {
        disableTimer -= Time.deltaTime;
    }

    public void Disable(float duration)
    {
        disableTimer = duration;
    }
}
