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

    [SerializeField] Material inactiveMat;

    [SerializeField] bool moveable; //is this a stationary defense like a door or window (should not be moved around house)

    [SerializeField] int trapDamageDealt; //how much does this damage the enemy?

    [SerializeField] int trapActiveLength; //length of time trap is active

    [SerializeField] AudioClip repairSound;

    [SerializeField] AudioClip breakSound;

    [SerializeField] AudioClip trapSound;

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

    public Material GetInactiveMaterial(){
        return inactiveMat;
    }

    public bool IsMoveable(){
        return moveable;
    }

    public int GetTrapDamage(){
        return trapDamageDealt;
    }

    public int GetTrapLength(){
        return trapActiveLength;
    }

    public AudioClip GetBreakClip(){ // returns the defense's break sound
        return breakSound;
    }

    public AudioClip GetRepairSound(){//returns sound for repairing defense
        return repairSound;
    }

    public AudioClip GetTrapSound(){ // returns sound for trap triggering
        return trapSound;
    }

}
