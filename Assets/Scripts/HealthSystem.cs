using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler Dead;
    public event EventHandler Damaged;

    [SerializeField] private int health = 100;
    
    private int healthMax;

    private void Awake()
    {
        healthMax = health;
    }

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

        OnDamaged();

        if (health == 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnDead();
    }

    public float GetHealthNormalized()
    {
        return (float)health / healthMax;
    }

    private void OnDead()
    {
        Dead?.Invoke(this, EventArgs.Empty);
    }

    private void OnDamaged()
    {
        Damaged?.Invoke(this, EventArgs.Empty);
    }

}
