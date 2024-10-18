using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEditor.UI;
using UnityEngine;

//this script goes on the salvage gameobjects, isTrigger should be checked
public class SalvageScript : MonoBehaviour
{
    [SerializeField] int salvageToAdd;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) { //when player walks into object, up salvage count
        if(other.gameObject.CompareTag("Player")){
            other.gameObject.GetComponent<PlayerInventory>().AddSalvageCount(salvageToAdd);
            Destroy(this.gameObject);
        }
    }
}
