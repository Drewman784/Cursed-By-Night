using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this script goes on the eyeball gameobjects
public class EyeballScript : MonoBehaviour
{
    private bool added;
    // Start is called before the first frame update
    void Start()
    {
        added = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.CompareTag("Player")){
            //GetComponent<SphereCollider>().isTrigger = true;
           // Debug.Log("switching to trigger");
            if(added == false){
            other.gameObject.GetComponent<PlayerInventory>().AddLootCurrency(1);
            added = true;
            }
            Debug.Log("adding currency");
            Destroy(this.gameObject);

        }
    }

/**
    private void OnTriggerEnter(Collider other) { //when player walks into object, up salvage count
        if(other.gameObject.CompareTag("Player")){
            other.gameObject.GetComponent<PlayerInventory>().AddLootCurrency(1);
            Debug.Log("adding currency");
            Destroy(this.gameObject);
        }
    }**/
}
