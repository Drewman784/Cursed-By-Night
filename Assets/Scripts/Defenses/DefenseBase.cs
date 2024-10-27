using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    private Material inactiveMat;

    private bool functioning;

    private GameObject popupCanvas;

    private bool popup;
    

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
        inactiveMat = defDetails.GetInactiveMaterial();
        currentHealth = defDetails.GetCurrentHealth();

        popupCanvas = transform.GetChild(1).gameObject;

        DismissPopUp();
        popup = false;

    }

    // Update is called once per frame
    void Update()
    {
        if(popup){ //keep the popup facing the camera
            //this references: https://discussions.unity.com/t/how-would-i-make-text-pop-up-above-an-object/694452/2
            popupCanvas.transform.LookAt(Camera.main.transform.position);
            //popupCanvas.transform.LookAt(new Vector3(Camera.main.transform.position.x,0, Camera.main.transform.position.z));
            popupCanvas.transform.Rotate(0,180,0);
        }
    }
    private void OnTriggerEnter(Collider other) { //when someone enters the range of the defense

        if(other.CompareTag("Player")){ //if player, give script info to move/use obj
            other.GetComponent<Player>().EnterDefenseRange(this);

        } else if(other.CompareTag("Enemy")){ //if enemy, trap springs
            TriggerEffect(other.gameObject);
        } else if (other.CompareTag("Attack")){ //if enemy attack, take damage
            RecieveDamage(1); //BASE TAKES DAMAGE - CURRENTLY SET TO 1
        }
    }

    private void OnTriggerExit(Collider other) { //dismiss popup and remove self from player script
        if(other.CompareTag("Player")){
            other.GetComponent<Player>().DisengageDefenseInteractableObject(this);
            Debug.Log("exit range");
            DismissPopUp();
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

    private void TriggerEffect(GameObject enemy){ // trap effect
        if(functioning){
        //FILL IN WITH WHATEVER
        }
    }

    public void RecieveDamage(int damage){ //defense takes damage (<-currently unused)
        currentHealth = currentHealth - damage;
        Debug.Log("CURRENT HEALTH: " + currentHealth);

        visualElement.GetComponent<MeshRenderer>().material = damagedMat;

        if(currentHealth <0){
            currentHealth = 0;
            functioning = false;
            //maybe switch to third material -> broken?
            visualElement.GetComponent<BoxCollider>().enabled = false; //<- disables the collider when health is zero

            visualElement.GetComponent<MeshRenderer>().material = inactiveMat;
        }

        
    }

    public void RepairPopUp(){ //brings up a popup with information about repairs
        if(currentHealth<healthMax){
            popupCanvas.SetActive(true);
            popupCanvas.GetComponentInChildren<TextMeshProUGUI>().text = "Press F to Repair";
            popup = true;
        }
    }

    public void MovePopUp(){ //brings up a popup with information about inventory
        popupCanvas.SetActive(true);
        popupCanvas.GetComponentInChildren<TextMeshProUGUI>().text = "Press E to Pick Up";
        popup = true;
    }

    public void DismissPopUp(){ //puts popup away
        popupCanvas.SetActive(false);
        popup = false;
    }

    public string GetDefenseName(){
        return defDetails.GetNameOfDefense();
    }
}
