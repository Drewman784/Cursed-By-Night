using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//THIS SCRIPT ATTATCHES TO THE HITBOX OF THE DEFENSE
public class DefenseBase : MonoBehaviour
{
    private GameObject visualElement; //the visible/rendered gameobject (this is the hitbox)
    private int currentHealth;
    [SerializeField] DefenseScriptableBase defDetails;
    private GameObject visualPrefab;
    private string nameOfDefense;
    private int healthMax;
    private string resourceRepairType; //currently generic is in use, this is a placeholder
    private int resourceRepairNum;
  
    private Material damagedMat;
    private Material repairedMat;

    private bool functioning;

    // Start is called before the first frame update
    void Start()
    {
        visualElement = transform.GetChild(0).gameObject;

        //import from scriptable object
        visualPrefab = defDetails.GetVisualPrefab();
        nameOfDefense = defDetails.GetNameOfDefense();
        healthMax = defDetails.GetHealthMax();
        resourceRepairNum = defDetails.GetResourceRepairNum();
        damagedMat = defDetails.GetDamagedMat();
        repairedMat = defDetails.GetRepairedMaterial();
        currentHealth = defDetails.GetCurrentHealth();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other) { //when someone enters the range of the defense
        Debug.Log("range 1");
        if(other.CompareTag("Player")){
            other.GetComponent<Player>().EnterDefenseRange(this);
            Debug.Log("range2");
        } else if(other.CompareTag("Enemy")){
            TriggerEffect(other.gameObject);
        }
    }

    public void TryToRepair(PlayerInventory inven){ //when the player tries to repair the defense
        Debug.Log("trying to repair");
        if(currentHealth<healthMax){
            if(inven.GetSalvageCount() >= resourceRepairNum){ // check if enough resources
                currentHealth = healthMax;
                inven.SetSalvageCount(inven.GetSalvageCount() - resourceRepairNum);
                visualElement.GetComponent<MeshRenderer>().material = repairedMat;
                Debug.Log("repaired!");
                functioning = true;
                visualElement.GetComponent<BoxCollider>().enabled = true;
            }
        }
    }

    private void TriggerEffect(GameObject enemy){ 
        if(functioning){
        //FILL IN WITH WHATEVER
        }
    }

    public void RecieveDamage(int damage){ //defense takes damage (<-currently unused)
        currentHealth = currentHealth - damage;
        if(currentHealth <0){
            currentHealth = 0;
            functioning = false;
            //maybe switch to third material -> broken?
            visualElement.GetComponent<BoxCollider>().enabled = false; //<- disables the collider when health is zero
        }

        
    }
}
