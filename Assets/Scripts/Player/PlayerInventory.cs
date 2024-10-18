using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private int salvage; //generic repair material, discuss in class
    [SerializeField] private int lootCurrency; //generic loot currency

    //private GameObject holding;
    [SerializeField] private List<GameObject> heldDefenses; //list of defenses held by player
    
    private int heldIndex;
    // Start is called before the first frame update

    [SerializeField] private GameObject InstructionsPanel; //ui prompt
    private bool popup; //is instructionspanel active
    private float ct;

    void Start()
    {
        heldDefenses = new List<GameObject>();
        //holding = null;
        DismissInstructions();
    }

    // Update is called once per frame
    void Update()
    {
        if(popup){
            ct+= Time.deltaTime;
            if(ct>=10){
                DismissInstructions();
            }
        }
    }

    public int GetSalvageCount(){ //returns salvage count
        return salvage;
    }

    public void SetSalvageCount(int newCount){ //modifies salvage count
        salvage = newCount;
    }

    public void AddSalvageCount(int add){
        salvage += add;
    }

    public int GetLootCurrency(){ //returns loot currency
        return lootCurrency;
    }

    public void SetLootCurrency(int value){ //modifies loot currency
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

    public bool InventoryIsFull(){ //returns true if inventory is full
        if(heldDefenses.Count() == 6){ //MAX COUNT SET TO 6 <- CAN MODIFY
            return true;
        } else{
            return false;
        }
    }

    public void AddToInventory(GameObject toBeHeld){ //attempts to add an item to inventory
        Debug.Log("add to inventory!");
        if(!InventoryIsFull()){
            //heldDefenses[heldDefenses.Count()-1] = toBeHeld;
            heldDefenses.Add(toBeHeld);
        }
        heldIndex = heldDefenses.Count()-1;
        BringUpInstructions();
    }

    public void RemoveFromInventory(){ //removes item from inventory
        //do stuff here
        Debug.Log("remove from inventory!");
        heldDefenses.RemoveAt(heldIndex);
        heldIndex--;
        if(InventoryIsEmpty()){
            DismissInstructions();
        }
    }

    public void ChangeIndex(int index){ //NOT IMPLEMENTED changes what item in inventory is selected
        heldIndex = index;
    }

    public bool InventoryIsEmpty(){//returns true if inventory is empty
        if(heldDefenses.Count() == 0){
            return true;
        } else{
            return false;
        }
    }

    public GameObject SelectFromInventory(){//returns inventory item from index
        return heldDefenses[heldIndex];
    }

    private void BringUpInstructions(){ //brings up ui instructions panel
        InstructionsPanel.SetActive(true);
        popup = true;
        ct = 0;
    }

    private void DismissInstructions(){ //deactivates ui instructions panel
        InstructionsPanel.SetActive(false);
        popup = false;
    }
}
