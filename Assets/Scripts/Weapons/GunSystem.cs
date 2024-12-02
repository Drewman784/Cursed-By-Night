using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GunSystem : MonoBehaviour
{
    //Gun stats
    public int damage;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    int bulletsLeft, bulletsShot;

    //bools
    bool shooting, readyToShoot, reloading;

    //Reference
    public Camera fpsCam;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask whatIsEnemy;
    public Transform CamTransform;
    public GameObject impactEffect;

    //Graphics
    public TextMeshProUGUI text;

    //Audio
    public AudioSource outOfAmmoSound; // Assign an AudioSource in the Inspector

    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
        UpdateAmmo();
    }

    private void Update()
    {
        MyInput();

        //SetText
        //text.SetText(bulletsLeft + " / " + magazineSize);
    }

    private void MyInput()
    {
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) {
            //Debug.Log("reload called");
            Reload();
        }

           
        // Check if shooting is attempted when there are no bullets left
        if (readyToShoot && shooting && !reloading)
        {
            if (bulletsLeft > 0)
            {
                bulletsShot = bulletsPerTap;
                Shoot();
                UpdateAmmo();
            }
            else
            {
                // Play out-of-ammo sound
                if (outOfAmmoSound != null && !outOfAmmoSound.isPlaying)
                {
                    outOfAmmoSound.Play();
                }
            }
        }
    }

    private void Shoot()
    {
        readyToShoot = false;

        //Spread
        //float x = Random.Range(-spread, spread);
        //float y = Random.Range(-spread, spread);

        //Calculate Direction
        //Vector3 direction = fpsCam.transform.forward; //+ new Vector3(x, y, 0);
        Vector3 direction = fpsCam.transform.GetChild(1).gameObject.transform.forward;

        // Single Raycast with Enemy Layer Mask
        //if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range, whatIsEnemy))
        if (Physics.Raycast(fpsCam.transform.GetChild(1).gameObject.transform.position, direction, out rayHit, range, whatIsEnemy))
        {
            Debug.Log("GUN: " + rayHit.collider.name + " " + rayHit.collider.tag);

            Enemy target = rayHit.collider.GetComponent<Enemy>();
            if (target != null)
            {
                // Apply Damage
                target.TakeDamage(damage);
                //Debug.DrawLine(CamTransform.position, rayHit.point, Color.green, 1f);
                Debug.DrawLine(CamTransform.GetChild(1).transform.position, rayHit.point, Color.green, 1f);
            }
            else
            {
                //Debug.DrawRay(CamTransform.position, CamTransform.forward * 100f, Color.red, 1f);
                Debug.DrawRay(CamTransform.GetChild(1).transform.position, CamTransform.forward * 100f, Color.red, 1f);
            }
        }

        bulletsLeft--;
        bulletsShot--;

        Invoke("ResetShot", timeBetweenShooting);

        if (bulletsShot > 0 && bulletsLeft > 0)
            Invoke("Shoot", timeBetweenShots);
    }

    private void ResetShot()
    {
        readyToShoot = true;
    }

    private void Reload()
    {
        //Debug.Log("rStart");
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        //Debug.Log("rFin");
        bulletsLeft = magazineSize;
        reloading = false;

        UpdateAmmo();
    }

    public void UpdateAmmo(){ //updates display of how many bullets left
        text.gameObject.SetActive(true);
        text.SetText(bulletsLeft + " / " + magazineSize);
    }

    public void HideAmmo(){ //hides ammo ui (for melee)
        text.gameObject.SetActive(false);
    }
}
