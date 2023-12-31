using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] int towerCost = 20;
    [SerializeField] float buildDelay = 1f;

    void Start() 
    {
        StartCoroutine(Build());
    }

    public bool CreateTower(Tower tower, Vector3 position)
    {
        Bank bank = FindObjectOfType<Bank>();
        
        if (bank == null) { return false; }

        if ((bank.CurrentBalance - towerCost) <= 0) { return false; }
        
        bank.Withdraw(towerCost);
        Instantiate(tower.gameObject, position, Quaternion.identity);
        return true;
    }

    IEnumerator Build()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);

            foreach (Transform grandchild in child)
            {
                grandchild.gameObject.SetActive(false);
            }

        }

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
            
            yield return new WaitForSeconds(buildDelay);

            foreach (Transform grandchild in child)
            {
                grandchild.gameObject.SetActive(true);
            }
            
        }
    }
}

