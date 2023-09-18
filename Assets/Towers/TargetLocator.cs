using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLocator : MonoBehaviour
{
    [SerializeField] Transform weapon;
    [SerializeField] float range = 15f;
    [SerializeField] ParticleSystem projectileParticles;
    Transform target;
    
    void Update()
    {
        LocateClosestTarget();
        AimWeapon();
    }

    void LocateClosestTarget()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>(); // create an array of all enemies in scene
        Transform closestTarget = null; // initialise a closest target variable
        float maxDistance = Mathf.Infinity; // initialise the max distance variable

        foreach (Enemy enemy in enemies)
        {
            float targetDistance = Vector3.Distance(transform.position, enemy.transform.position); // get the distance to that enemy

            if(targetDistance < maxDistance) // if the distance to the current enemy is less than our max
            {
                closestTarget = enemy.transform; // then the closest target is our current target
                maxDistance = targetDistance;
            }
        }

        target = closestTarget;
    }

    void AimWeapon()
    {
        if (target == null) { return; }
        
        weapon.LookAt(target);

        float targetDistance = Vector3.Distance(transform.position, target.transform.position);

        if (targetDistance < range)
        {
            Attack(true);
        }
        else
        {
            Attack(false);
        }
    }

    void Attack(bool isActive)
    {
        var emissionModule = projectileParticles.emission;
        emissionModule.enabled = isActive;
    }
}
