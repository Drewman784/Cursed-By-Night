using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private int salvage; //generic repair material, discuss in class
    [SerializeField] private int lootCurrency; //generic loot currency

    int Realsalvage;
    int ReallootCurrency;

    //private GameObject holding;
    [SerializeField] private List<GameObject> heldDefenses; //list of defenses held by player
    
    private int heldIndex;
    // Start is called before the first frame update

    [SerializeField] private GameObject InstructionsPanel; //ui prompt
    private bool popup; //is instructionspanel active
    private float ct;

    private static TextMeshProUGUI LootText;
    private static TextMeshProUGUI SalvageText;

    [SerializeField] private List<GameObject> inventoryPanels;

    void Start()
    {
        heldDefenses = new List<GameObject>();
        //holding = null;
        DismissInstructions();

        //Helps display currency counts
        Realsalvage = salvage;
        ReallootCurrency = lootCurrency;

        //Displays currency counts
        LootText = GameObject.Find("Eyes_Text").GetComponent<TextMeshProUGUI>();
        LootText.SetText($"Eyes Collected: {ReallootCurrency} ");

        SalvageText = GameObject.Find("Salvage_Text").GetComponent<TextMeshProUGUI>();
        SalvageText.SetText($"Salvage Collected: {Realsalvage} ");

        /**
        GameObject invenHolder = GameObject.Find("InventoryPanel").gameObject; //load inventory panels
        Debug.Log("found?: " + GameObject.Find("InventoryPanel").gameObject); 
        inventoryPanels = new List<GameObject>();
        for (int i = 0; i<6; i++){
            Debug.Log("found panel: "+ i);
            inventoryPanels[i] = invenHolder.transform.GetChild(i).gameObject;
        }**/

        UpdateUI();
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

        if(Input.GetKeyDown(KeyCode.Alpha1)){ //when player presses number key, change selected inventory item
            ChangeIndex(0);
        } else if(Input.GetKeyDown(KeyCode.Alpha2)){
            ChangeIndex(1);
        } else if(Input.GetKeyDown(KeyCode.Alpha3)){
            ChangeIndex(2);
        } else if(Input.GetKeyDown(KeyCode.Alpha4)){
            ChangeIndex(3);
        } else if(Input.GetKeyDown(KeyCode.Alpha5)){
            ChangeIndex(4);
        } else if(Input.GetKeyDown(KeyCode.Alpha6)){
            ChangeIndex(5);
        } 

    }

    public int GetSalvageCount(){ //returns salvage count
        return salvage;
    }

    public void SetSalvageCount(int newCount){ //modifies salvage count
        salvage = newCount;
        UpdateUI();
    }

    public void AddSalvageCount(int add){ //adds parameter amount to salvage count
        salvage += add;
        Realsalvage += add;
        UpdateUI();
    }

    public int GetLootCurrency(){ //returns loot currency
        return lootCurrency;
    }

    public void SetLootCurrency(int value){ //modifies loot currency
        lootCurrency = value;
        UpdateUI();
    }

    public void AddLootCurrency(int add){ //adds parameter amount to loot currency
        lootCurrency+=add;
        ReallootCurrency += add;
        UpdateUI();
    }

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
        UpdateUI();
    }

    public void RemoveFromInventory(){ //removes item from inventory
        //do stuff here
        Debug.Log("remove from inventory!");
        heldDefenses.RemoveAt(heldIndex);
        heldIndex--;
        if(InventoryIsEmpty()){
            DismissInstructions();
        }
        UpdateUI();
    }

    public void ChangeIndex(int index){ //changes what item in inventory is selected

        if(heldDefenses.Count-1>=index){ //check that inventory has given index
             heldIndex = index;
        } else{
            heldIndex = heldDefenses.Count()-1; //otherwise select last inventory item
        }
        UpdateUI();
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

    public void UpdateUI(){
        //Update Currencies
        ReallootCurrency = lootCurrency;
        Realsalvage = salvage;
        LootText.SetText($"Eyes Collected: {ReallootCurrency} ");
        SalvageText.SetText($"Salvage Collected: {Realsalvage} ");

        Debug.Log("inv: "+ heldDefenses.Count());

        if(!InventoryIsEmpty()){
            for(int a = 0; a < heldDefenses.Count(); a++){ //show used inventory panels
                inventoryPanels[a].SetActive(true);
                inventoryPanels[a].GetComponent<UnityEngine.UI.Image>().color = Color.grey;

                //show details
                TextMeshProUGUI texmp = inventoryPanels[a].transform.GetChild(1).GetComponent<TextMeshProUGUI>();
                texmp.text = heldDefenses[a].GetComponent<DefenseBase>().GetDefenseName();
            }

            for(int b = heldDefenses.Count(); b<6;b++){ //hide unused inventory panels
                inventoryPanels[b].SetActive(false);
            }

            //show selected inventory item
            inventoryPanels[heldIndex].GetComponent<UnityEngine.UI.Image>().color = Color.white;
        } else{
            foreach(GameObject e in inventoryPanels){
                e.SetActive(false);
            }
        }
    }
}
