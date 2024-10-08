using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private int salvage; //generic repair material, discuss in class
    [SerializeField] private int lootCurrency;

    //private GameObject holding;
    [SerializeField] private List<GameObject> heldDefenses;
    
    private int heldIndex;
    // Start is called before the first frame update
    void Start()
    {
        heldDefenses = new List<GameObject>();
        //holding = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetSalvageCount(){
        return salvage;
    }

    public void SetSalvageCount(int newCount){
        salvage = newCount;
    }

    public int getLootCurrency(){
        return lootCurrency;
    }

    public void setLootCurrency(int value){
        lootCurrency = value;
    }
    /**
    public GameObject GetHolding(){
        GameObject hold = holding;
        holding = null;
        return hold;
    }

    public bool IsHolding(){
        return holding != null;
    }

    public void SetHolding(GameObject toBeHeld){
        holding = toBeHeld;
    }*/

    public bool InventoryIsFull(){
        if(heldDefenses.Count() == 6){
            return true;
        } else{
            return false;
        }
    }

    public void AddToInventory(GameObject toBeHeld){
        Debug.Log("add to inventory!");
        if(!InventoryIsFull()){
            //heldDefenses[heldDefenses.Count()-1] = toBeHeld;
            heldDefenses.Add(toBeHeld);
        }
        heldIndex = heldDefenses.Count()-1;
    }

    public void RemoveFromInventory(){
        //do stuff here
        Debug.Log("remove from inventory!");
        heldDefenses.RemoveAt(heldIndex);
        heldIndex--;
    }

    public void ChangeIndex(int index){
        heldIndex = index;
    }

    public bool InventoryIsEmpty(){
        if(heldDefenses.Count() == 0){
            return true;
        } else{
            return false;
        }
    }

    public GameObject SelectFromInventory(){
        return heldDefenses[heldIndex];
    }
}
