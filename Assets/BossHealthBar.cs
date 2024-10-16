using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    public Slider bossslider ;
    public Gradient bossgradient ;

    public Image fill ;

    public void SetMaxHealth(int health)
    {
        bossslider.maxValue = health ;
        bossslider.value = health ;
        fill.color = bossgradient.Evaluate(1f);
    }

    public void SetHealth(int health)
    {
        bossslider.value = health ;

        fill.color =bossgradient.Evaluate(bossslider.normalizedValue);
    }
}
