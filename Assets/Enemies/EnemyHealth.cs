using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))] 
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int maxHP = 5;

    [SerializeField] [Tooltip("Adds amount to maxHP when enemy dies.")] 
    int difficultyRamp = 1;

    int currentHP = 0;

    Enemy enemy;
    

    void Start() 
    {
        enemy = GetComponent<Enemy>();
    }
    
    void OnEnable()
    {
        currentHP = maxHP;
    }

    void OnParticleCollision(GameObject other) 
    {
        ProcessHit();
    }

    void ProcessHit()
    {
        currentHP --;

        if (currentHP <= 0)
        {
            enemy.RewardGold();
            gameObject.SetActive(false);
            maxHP += difficultyRamp;
        }
    }
}
