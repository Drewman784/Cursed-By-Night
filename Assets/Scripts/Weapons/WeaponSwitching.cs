using TMPro;
using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI gunUI;
    public int selectedWeapon = 0;
    private bool currMenu;

    
    void Start()
    {
        currMenu =false;
        SelectWeapon();
    }
    void Update()
    {

        int previousSelectedWeapon = selectedWeapon;


        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (selectedWeapon >= transform.childCount - 1)
                selectedWeapon = 0;
            else
            selectedWeapon++;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (selectedWeapon <= 0)
                selectedWeapon = transform.childCount -1;
            else
                selectedWeapon--;
        }

        /*if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedWeapon = 0;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedWeapon = 1;
        }*/

        if (previousSelectedWeapon != selectedWeapon)
        {
            SelectWeapon();
        }
    }

    
    void SelectWeapon()
    {
        int i = 0;
       foreach(Transform weapon in transform)
        {
            if (i == selectedWeapon)
                weapon.gameObject.SetActive(true);
            else
                weapon.gameObject.SetActive(false);
            i++;
        } 
            if(selectedWeapon != 4){
                transform.GetChild(selectedWeapon).GetComponent<GunSystem>().UpdateAmmo();
            } else{
                transform.GetChild(0).GetComponent<GunSystem>().HideAmmo();
            }
            
            //updates ui to display which gun is selected
            string gunName = ""; 
            /*switch(selectedWeapon){
                case 0: gunName ="1911 Model"; break;
                case 1: gunName ="Rifle"; break;
                case 2: gunName ="Machine Gun"; break;
                case 3: gunName ="Shotgun"; break;
                case 4: gunName ="Melee"; break;
                default: gunName = "error"; break;
            }*/

            gunName = transform.GetChild(selectedWeapon).GetComponent<GunSystem>().gunName;
            gunUI.text = gunName;
            GiveWeaponMenuState(currMenu);
    }

    public void GiveWeaponMenuState(bool menu){
        currMenu = menu;
        Debug.Log("wp: " + menu);
        if(selectedWeapon!=4){
            transform.GetChild(selectedWeapon).GetComponent<GunSystem>().SetMenu(menu);
        }
    }

    public void GiveWeapon(){ //called when new weapon aquired
        int index = transform.childCount-1;
        GunSystem newG = transform.GetChild(index).GetComponent<GunSystem>();
        GunSystem refG = transform.GetChild(0).GetComponent<GunSystem>();

        newG.fpsCam = refG.fpsCam;
        newG.attackPoint = refG.attackPoint;
        newG.rayHit = refG.rayHit;
        newG.whatIsEnemy = refG.whatIsEnemy;
        newG.CamTransform = refG.CamTransform;
        newG.impactEffect = refG.impactEffect;

        newG.text = refG.text;

        newG.enabled = true;
        

        selectedWeapon = transform.childCount-1;
        SelectWeapon();
    }
}
