using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other) {
        Debug.Log("hitbox: "+other.gameObject.tag);
        if(other.gameObject.CompareTag("Player")){
            //deal damage to player I guess
        } else if(other.gameObject.CompareTag("DefenseInteractable")){
            other.gameObject.GetComponentInParent<DefenseBase>().RecieveDamage(1);
            //other.gameObject.GetComponent<DefenseBase>().RecieveDamage(1);
        }
    }
}
