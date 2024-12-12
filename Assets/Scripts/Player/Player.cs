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
//using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    private float velocity;
    public float normalmoveSpeed;
    public float sprintSpeed;
    //[SerializeField] private bool canSprint;

    //private Rigidbody rb;
    private CharacterController cc;
    private bool sprinting;

    //VARIABLES FOR REPAIR - MAYBE MOVE
    private DefenseBase defenseInteractableObject;
    private bool defenseInteractable;

    public bool dayCycle;

    //private bool objPreviewed;

    private bool inMenu;

    private bool previewing;

    //[Header("Weapon (Controls)")]
    //[SerializeField] List<GameObject> gunList; //this is for enabling/disabling shooting in menus
    private WeaponSwitching weaponSw;

    void Start()
    {
        inMenu = false;
        
        weaponSw = transform.GetChild(0).transform.GetChild(1).GetComponent<WeaponSwitching>();
        //rb = GetComponent<Rigidbody>();
        cc = gameObject.GetComponent<CharacterController>();
        sprinting = false;

        previewing = false;

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

        //trial run of new preview system
        if (previewing){
            PlaceObject();
        }
    }

    void Update()
    {
        
            //if (Input.GetKeyDown(KeyCode.Q)) //switched from escape key
            //{
            //    SceneManager.LoadScene("Main_Menu");
            //}


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
                     GetComponent<PlayerInventory>().PlayPickupSound(GetComponent<PlayerInventory>().SelectFromInventory());
                     defenseInteractableObject.gameObject.GetComponent<BoxCollider>().enabled = false;
                     defenseInteractableObject.gameObject.GetComponentInChildren<BoxCollider>().enabled = false;
                     defenseInteractableObject.DismissPopUp();
                     defenseInteractableObject.gameObject.SetActive(false); // make object inactive
                     defenseInteractable = false; //clear object from current possible interactions
                     defenseInteractableObject = null;
                     //objPreviewed = false;

                }
                
            } //ALTERED VER BELOW
        } /*else if(Input.GetKeyDown(KeyCode.R)){ //finish placing defense
            if(!GetComponent<PlayerInventory>().InventoryIsEmpty() && dayCycle){ //check that inventory isn't empty and right cycle for moving objects
                if(objPreviewed){ //has the object been previewed?
                    GetComponent<PlayerInventory>().SelectFromInventory().GetComponent<BoxCollider>().enabled = true;
                    GetComponent<PlayerInventory>().SelectFromInventory().GetComponentInChildren<BoxCollider>().enabled = true;
                    GetComponent<PlayerInventory>().PlayPlacementSound(GetComponent<PlayerInventory>().SelectFromInventory());
                    GetComponent<PlayerInventory>().RemoveFromInventory();
                    objPreviewed =false;
                    Debug.Log("object placed!");
                } else if(PlaceObject()){ //put it down without previewing
                 GetComponent<PlayerInventory>().SelectFromInventory().GetComponent<BoxCollider>().enabled = true;
                 GetComponent<PlayerInventory>().SelectFromInventory().GetComponentInChildren<BoxCollider>().enabled = true;
                 GetComponent<PlayerInventory>().PlayPlacementSound(GetComponent<PlayerInventory>().SelectFromInventory());
                    GetComponent<PlayerInventory>().RemoveFromInventory();
                    //Debug.Log("object placed!");
                    objPreviewed = false;
                }
            }
        }*/ else if(Input.GetKeyDown(KeyCode.C)){ //preview defense
            if(!GetComponent<PlayerInventory>().InventoryIsEmpty() && dayCycle){
                if(previewing){
                    GetComponent<PlayerInventory>().SelectFromInventory().GetComponent<BoxCollider>().enabled = true;
                    GetComponent<PlayerInventory>().SelectFromInventory().GetComponentInChildren<BoxCollider>().enabled = true;
                    GetComponent<PlayerInventory>().PlayPlacementSound(GetComponent<PlayerInventory>().SelectFromInventory());
                    GetComponent<PlayerInventory>().RemoveFromInventory();
                    //objPreviewed =false;
                    //Debug.Log("placing now");
                    PlaceObject();
                    previewing = false;
                } else{
                    previewing = true;
                }

            }
        } else if(Input.GetKeyDown(KeyCode.V) && previewing){//rotate defense 
             GetComponent<PlayerInventory>().SelectFromInventory().transform.Rotate(0,90f,0);
        }

        //UNALTERED VER OF ABOVE
        /*else if(Input.GetKeyDown(KeyCode.R)){ //finish placing defense
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
        }*/
        }

    }

    private void ApplyRotation()
    {
        
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
            }else{ 
                defObj.RepairPopUp();
            }
        } else{
            defObj.RepairPopUp();
        }

        if(!defObj.IsMoveable()){//if it's not moveable, ensure player can keep going through it
            defObj.GoIntangible();
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
                //Debug.Log("YEAH");
                //LineRenderer line = new LineRenderer();
                //line.SetPosition(0, ray.origin);
                //line.SetPosition(1, hitData.point);
                if (hitData.collider.gameObject != toPlace && hitData.collider.gameObject.CompareTag("Environment")) //did it hit a valid place
                {
                    toPlace.SetActive(true);
                    GameObject model = toPlace.gameObject.transform.GetChild(0).gameObject;
                    //toPlace.transform.position = hitData.point + new Vector3(0, toPlace.GetComponent<MeshRenderer>().bounds.extents.y/2, 0);
                    //float h =  model.GetComponent<BoxCollider>().size.y/2 * model.transform.GetChild(0).gameObject.transform.localScale.y;
                    float h =  toPlace.transform.localScale.y/2;
                    //Debug.Log("height: "+ h + " ground at: " + hitData.point.y);
                    toPlace.transform.position = hitData.point + new Vector3(0,h, 0);
                    //Debug.Log("placed at: " + toPlace.transform.position);
                    //objPreviewed = true;
                               return true;
                } else{
                    //Debug.Log("raycast failed: tag - "+ hitData.collider.gameObject.tag);
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

    if(weaponSw == null){
        weaponSw = transform.GetChild(0).transform.GetChild(1).GetComponent<WeaponSwitching>();
    }
        inMenu = menu;

        if(inMenu){
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        } else{
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        Debug.Log("menu state: " + menu);
        weaponSw.GiveWeaponMenuState(menu);
        /*
       foreach(GameObject g in gunList){ //enable/disable shooting
            g.GetComponent<Pistol>().SetMenu(menu);
             if(g.isActive() == false){
                g.SetActive(true);
                g.GetComponent<Pistol>().SetMenu(menu);
                g.SetActive(false);
            } else{
                g.SetActive(true);
                g.GetComponent<Pistol>().SetMenu(menu);
            }
        }*/
    }
}
