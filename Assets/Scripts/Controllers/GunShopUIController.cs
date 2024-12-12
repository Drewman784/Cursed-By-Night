using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GunShopUIController : MonoBehaviour
{
    public bool inRange; // is the player in range of the shopkeeper
    private bool shopScreenOpen; // is the shop window open

    [SerializeField] private GameObject GeneralUI;

    [SerializeField] GameObject popupCanvas;
    [SerializeField] private GameObject shopScreen;
    [SerializeField] private TextMeshProUGUI LootCurrencyText;

    [SerializeField] private List<GameObject> soldItems;

    [SerializeField] List<GameObject> ButtonsInOrder;
    // Start is called before the first frame update

    private bool day;
    void Start()
    {
        inRange = false;
        CloseShopScreen();
        popupCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(inRange && day){ //if in range
            if(Input.GetKeyDown(KeyCode.Q)){ //player presses S
                if(shopScreenOpen){ 
                    CloseShopScreen(); //if in shop, exit shop
                } else{
                    OpenShopScreen(); //otherwise enter shop
                }
            }

            //keep the popup facing the camera
            //this references: https://discussions.unity.com/t/how-would-i-make-text-pop-up-above-an-object/694452/2
            //popupCanvas.transform.LookAt(Camera.main.transform.position);
            //popupCanvas.transform.LookAt(new Vector3(Camera.main.transform.position.x,0, Camera.main.transform.position.z));
            //popupCanvas.transform.Rotate(0,180,0);

            //referenced from: https://www.reddit.com/r/Unity3D/comments/cj7niq/rotating_an_object_to_face_player_on_only_y_axis/
            float movementStrength = 100f;
            Vector3 lookDir = popupCanvas.transform.position - Camera.main.transform.position;
            float radians = Mathf.Atan2(lookDir.x, lookDir.z);
            float degrees = radians * Mathf.Rad2Deg;

            float str = Mathf.Min(movementStrength * Time.deltaTime, 1);
            Quaternion targetRotation = Quaternion.Euler(0, degrees + 180, 0);
            popupCanvas.transform.rotation =  Quaternion.Slerp(popupCanvas.transform.rotation, targetRotation, str);
           // popupCanvas.transform.GetChild(0).transform.rotation = Quaternion.Euler(0, degrees + 180, 0);
            
            
        
        }
    }

    private void OnTriggerEnter(Collider other) { //if player enters range
        Debug.Log("enter " + other.gameObject.tag);
        if(other.gameObject.CompareTag("Player") && day){
            Debug.Log("check");
            inRange = true;
            popupCanvas.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other) { //if player exits range
        if(other.gameObject.CompareTag("Player")){
            inRange = false;
            popupCanvas.SetActive(false);
        }
    }

    public void OpenShopScreen(){
        shopScreenOpen = true;
        shopScreen.SetActive(true);
        UpdateShopScreen();

        GeneralUI.SetActive(false);
        GameObject.Find("Player").gameObject.GetComponent<Player>().SetMenu(true);
        Time.timeScale = 0;
    }

    public void CloseShopScreen(){
        shopScreenOpen = false;
        shopScreen.SetActive(false);
        
        GeneralUI.SetActive(true);
        GameObject.Find("Player").gameObject.GetComponent<Player>().SetMenu(false);
        Time.timeScale = 1;
    }

    public void BuyItem(int index){ //purchase a weapon from the shop
        int price;
        switch(index){
            case 0: price = 15; break;
            case 1: price = 30; break;
            case 2: price = 45; break;
            default: price = 1; break;
        }
        PlayerInventory inv = GameObject.Find("Player").GetComponent<PlayerInventory>();

        if(inv.GetLootCurrency()>=price){ //if player has enough money
            GameObject g = Instantiate(soldItems[index]); //spawn weapon outside shop

            //set position
            g.transform.position = GameObject.Find("Player").transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).transform.position;
            g.transform.rotation = GameObject.Find("Player").transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).transform.rotation;
            g.transform.parent = GameObject.Find("Player").transform.GetChild(0).transform.GetChild(1).transform;
            GameObject.Find("Player").transform.GetChild(0).transform.GetChild(1).GetComponent<WeaponSwitching>().GiveWeapon();
            inv.SetLootCurrency(inv.GetLootCurrency() - price); //pay price

            ButtonsInOrder[index].SetActive(false);
            UpdateShopScreen();
        }
    }

    private void UpdateShopScreen(){ //updates the shop screen
        PlayerInventory inv = GameObject.Find("Player").GetComponent<PlayerInventory>();

        LootCurrencyText.text = "Held Eyeballs: " + inv.GetLootCurrency(); //show current num of eyeballs
    }

    public void ShopDay(){  // shop enters day cycle, available
        day = true;
        GameObject shopguy = transform.GetChild(0).gameObject;
        shopguy.SetActive(true);
    }

    public void ShopNight(){//shop enters night cycle, no longer available
        day = false;
        GameObject shopguy = transform.GetChild(0).gameObject;
        shopguy.SetActive(false);
    }
}
