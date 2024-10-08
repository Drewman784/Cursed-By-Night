using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DefenseScriptableBase : ScriptableObject
{ 
    [SerializeField] private int currentHealth;

    [SerializeField] GameObject visualPrefab;
    [SerializeField] string nameOfDefense;
    [SerializeField] int healthMax;
    [SerializeField] string resourceRepairType;  //currently generic is in use, this is a placeholder
    [SerializeField] int resourceRepairNum;
  
    [SerializeField] Material damagedMat;
    [SerializeField] Material repairedMat;

    public GameObject GetVisualPrefab(){
        return visualPrefab;
    }
    public string GetNameOfDefense(){
        return nameOfDefense;
    }

    public int GetCurrentHealth(){
        return currentHealth;
    }

    public void SetCurrentHealth(int newHealth){
        currentHealth = newHealth;
    }

    public int GetHealthMax(){
        return healthMax;
    }

    public int GetResourceRepairNum(){
        return resourceRepairNum;
    }

    public Material GetDamagedMat(){
        return damagedMat;
    }

    public Material GetRepairedMaterial(){
        return repairedMat;
    }

}
