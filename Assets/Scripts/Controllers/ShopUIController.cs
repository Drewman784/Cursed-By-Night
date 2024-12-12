using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopUIController : MonoBehaviour
{
    public bool inRange; // is the player in range of the shopkeeper
    private bool shopScreenOpen; // is the shop window open

    [SerializeField] private GameObject GeneralUI;

    [SerializeField] GameObject popupCanvas;
    [SerializeField] private GameObject shopScreen;
    [SerializeField] private TextMeshProUGUI LootCurrencyText;

    [SerializeField] private List<GameObject> soldItems;
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
        if(other.gameObject.CompareTag("Player") && day){
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
    }

    public void CloseShopScreen(){
        shopScreenOpen = false;
        shopScreen.SetActive(false);
        
        GeneralUI.SetActive(true);
        GameObject.Find("Player").gameObject.GetComponent<Player>().SetMenu(false);
    }

    public void BuyItem(int index){ //purchase an item from the shop
        int price;
        switch(index){
            case 0: price = 2; break;
            case 1: price = 5; break;
            case 2: price = 3; break;
            case 3: price = 3; break;
            case 4: price = 5; break;
            case 5: price = 2; break;
            case 6: price = 5; break;
            case 7: price = 10; break;
            case 8: price = 6; break;
            case 9: price = 4; break;
            default: price = 1; break;
        }
        PlayerInventory inv = GameObject.Find("Player").GetComponent<PlayerInventory>();

        if(inv.GetLootCurrency()>=price){ //if player has enough money
            GameObject g = Instantiate(soldItems[index]); //spawn item outside shop
            g.transform.position = transform.position + new Vector3(0,0,3);
            inv.SetLootCurrency(inv.GetLootCurrency() - price); //pay price

            if(inv.InventoryIsFull()){ //inventory is full  
                CloseShopScreen(); //close shop window
            } else{ //inventory not full
                inv.AddToInventory(g); //adds item to inventory
                UpdateShopScreen(); //update screen
            }
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
