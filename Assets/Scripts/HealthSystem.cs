using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler Dead;

    [SerializeField] private int health = 100;
    
    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    public void Damage(int damageAmount)
    {
        health -= damageAmount;
        if (health < 0)
        {
            health = 0;
        }

        if (health == 0)
        {
            Die();
        }

        print("Damage:" + health);
    }

    private void Die()
    {
        OnDead();
    }

    private void OnDead()
    {
        Dead?.Invoke(this, EventArgs.Empty);
    }

}
