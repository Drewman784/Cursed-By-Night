using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualComponentScript : MonoBehaviour
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
        Debug.Log("viz: "+ other.gameObject.tag);
        if(other.gameObject.CompareTag("Attack")){
            GetComponentInParent<DefenseBase>().RecieveDamage(1);
        }
    }
}
