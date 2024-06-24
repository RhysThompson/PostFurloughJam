using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class HealthManager : MonoBehaviour
{
    public int MaxHealth = 40;
    private int Health;
    public Slider HealthBar;

    public UnityEvent SelfDestructFunction;

    // Start is called before the first frame update
    void Start()
    {
        Health = MaxHealth;
        if (HealthBar)
        {
            HealthBar.maxValue = MaxHealth;
            HealthBar.value = MaxHealth;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage(int damage)
    {
        if (Health == 1234)
            return;

        Health -= damage;
        if (HealthBar)
        {
            HealthBar.value = Health;
            HealthBar.maxValue = MaxHealth;
        }
        if (Health <= 0)
        {
            Health = -1234;
            SelfDestructFunction.Invoke();
        }
    }
}
