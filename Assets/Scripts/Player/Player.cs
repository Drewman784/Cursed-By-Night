using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float moveSpeed;
    public float normalmoveSpeed;
    public float sprintSpeed;
    //[SerializeField] private bool canSprint;

    private Rigidbody2D rb;
    private bool sprinting;

    //VARIABLES FOR REPAIR - MAYBE MOVE
    private DefenseBase defenseInteractableObject;
    private bool defenseInteractable;

    private bool dayCycle;

    private bool objPreviewed;

    private bool inMenu;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprinting = false;

        //REPAIR VARS
        defenseInteractable = false;
        defenseInteractableObject = null;

        SetMenu(false);
    }

    void FixedUpdate()
    {
        //Movement Mechanics
        float horizontalAxis = Input.GetAxis("Horizontal");
        float verticalAxis = Input.GetAxis("Vertical");

        var camera = Camera.main;
        var forward = camera.transform.forward;
        var right = camera.transform.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();
        var moveDirection = forward * verticalAxis + right * horizontalAxis;

        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }

    void Update()
    {

        
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene("Main_Menu");
            }


            if (!inMenu){
        // Sprint Mechanics
        if (Input.GetKey(KeyCode.LeftShift)) 
        {
            if (!sprinting) 
            {
                moveSpeed += sprintSpeed;
                sprinting = true; 
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftShift)) 
        {
            moveSpeed = normalmoveSpeed;
            sprinting = false;
        }

        //REPAIR // INVENTORY CODE
        if(defenseInteractable){
            if(Input.GetKeyDown(KeyCode.F)){ //try to repair nearby defense

                defenseInteractableObject.TryToRepair(GetComponent<PlayerInventory>());

            } else if(Input.GetKeyDown(KeyCode.E) && dayCycle){ //pick up nearby defense

                if(!GetComponent<PlayerInventory>().InventoryIsFull() && defenseInteractableObject.IsMoveable()){ //check that there's room in the inventory + object not stationary
                     //defenseInteractableObject.gameObject.transform.parent = invenPoint.transform; //parent object to player 

                     GetComponent<PlayerInventory>().AddToInventory(defenseInteractableObject.gameObject); //add to inventory
                     defenseInteractableObject.gameObject.GetComponent<BoxCollider>().enabled = false;
                     defenseInteractableObject.gameObject.GetComponentInChildren<BoxCollider>().enabled = false;
                     defenseInteractableObject.DismissPopUp();
                     defenseInteractableObject.gameObject.SetActive(false); // make object inactive
                     defenseInteractable = false; //clear object from current possible interactions
                     defenseInteractableObject = null;
                     objPreviewed = false;

                }
                
            } 
        }else if(Input.GetKeyDown(KeyCode.R)){ //finish placing defense
            if(!GetComponent<PlayerInventory>().InventoryIsEmpty() && dayCycle){ //check that inventory isn't empty and right cycle for moving objects
                if(objPreviewed){ //has the object been previewed?
                    GetComponent<PlayerInventory>().SelectFromInventory().GetComponent<BoxCollider>().enabled = true;
                     GetComponent<PlayerInventory>().SelectFromInventory().GetComponentInChildren<BoxCollider>().enabled = true;
                    GetComponent<PlayerInventory>().RemoveFromInventory();
                    objPreviewed =false;
                    Debug.Log("object placed!");
                } else if(PlaceObject()){ //put it down without previewing
                 GetComponent<PlayerInventory>().SelectFromInventory().GetComponent<BoxCollider>().enabled = true;
                 GetComponent<PlayerInventory>().SelectFromInventory().GetComponentInChildren<BoxCollider>().enabled = true;
                    GetComponent<PlayerInventory>().RemoveFromInventory();
                    Debug.Log("object placed!");
                    objPreviewed = false;
                }
            }
        } else if(Input.GetKey(KeyCode.C)){ //place defense
            if(!GetComponent<PlayerInventory>().InventoryIsEmpty() && dayCycle){
                Debug.Log("placing now");
                PlaceObject();
            }
        }
        }

    }

    //REPAIR CODE
    public void EnterDefenseRange(DefenseBase defObj) // player enters interaction range with a defense object
    {
        defenseInteractableObject = defObj;
        defenseInteractable = true;
        //Debug.Log("entered range");
        if(dayCycle){
            if(defObj.IsMoveable()){ //check if immobile object such as window or door
                defObj.MovePopUp();
            }else{ //if it's not moveable, ensure player can keep going through it
                defObj.RepairPopUp();
                defObj.GoIntangible();
            }
        } else{
            defObj.RepairPopUp();
        }
    }

    public void DisengageDefenseInteractableObject(DefenseBase def){ //player exits interaction range with defense object
        if(defenseInteractableObject = def){
            defenseInteractableObject = null;
            defenseInteractable = false;
        }
    }

    //INVENTORY CODE
    //this method references: https://discussions.unity.com/t/displaying-the-ray-line/406529/8
    //this method also references: https://gamedevbeginner.com/raycasts-in-unity-made-easy/
    private bool PlaceObject() //player sends raycast to see where an object would be placed
    {
        if (!GetComponent<PlayerInventory>().InventoryIsEmpty())//check that inventory isnt empty
        {

            GameObject toPlace = GetComponent<PlayerInventory>().SelectFromInventory(); //get item from index

            Ray ray = new Ray(Camera.main.transform.GetChild(1).transform.position + new Vector3(0, .5f, 0), Camera.main.transform.forward * 20);
            RaycastHit hitData;
            //Physics.Raycast(ray, out hitData);
            Debug.DrawRay(Camera.main.transform.GetChild(1).transform.position + new Vector3(0, .5f, 0), Camera.main.transform.forward * 20, Color.green);


            if (Physics.Raycast(ray, out hitData) /**&& hitData.distance<=15*/) //did the raycast hit something
            {
                Debug.Log("YEAH");
                //LineRenderer line = new LineRenderer();
                //line.SetPosition(0, ray.origin);
                //line.SetPosition(1, hitData.point);
                if (hitData.collider.gameObject != toPlace && hitData.collider.gameObject.CompareTag("Environment")) //did it hit a valid place
                {
                    toPlace.SetActive(true);
                    toPlace.transform.position = hitData.point + new Vector3(0, toPlace.GetComponent<MeshRenderer>().bounds.extents.y/2, 0);
                    objPreviewed = true;
                               return true;
                } else{
                    Debug.Log("raycast failed: tag - "+ hitData.collider.gameObject.tag);
                     return false;
                }
     
            }
            else
            { 
                Debug.Log("raycast failed");
                return false;
            }

        }
        else
        {
            return false;
        }

    }

    public void SetDayCycle(bool day){ //updates the cycle
        dayCycle = day;
        Debug.Log("Player day =" + day+"!");

        if(!dayCycle &&defenseInteractable){ //if in range of a defense that's been set as not solid during day
            if(defenseInteractableObject.GetComponent<DefenseBase>().IsMoveable() == false){
                defenseInteractableObject.GetComponent<DefenseBase>().GoTangible(); // make it solid again
            }
        }
    }

    public void SetMenu(bool menu){ //if the player is in menu, disable movement + enable mouse
        inMenu = menu;

        if(inMenu){
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        } else{
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
