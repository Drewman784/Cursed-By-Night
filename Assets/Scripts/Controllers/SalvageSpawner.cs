using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

//this script goes on DayNightCycleController
//this spawns salvage during the day cycle and despawns it during the night
public class SalvageSpawner : MonoBehaviour
{
    public List<GameObject> activeSalvage;

    [SerializeField] GameObject salvagePrefab;

    [SerializeField] Vector3 centerOfSpawnArea;

    // Start is called before the first frame update

    public void EnterDayCycle(){ //day cycle begins, salvage spawns
        int numToSpawn = 5;
        for(int a =0; a<numToSpawn;a++){

            GameObject n = Instantiate(salvagePrefab); //create salvage

            Vector3 pos = Random.insideUnitCircle * 10; //place around spawn point
            pos += centerOfSpawnArea;
            n.transform.position = new Vector3(pos.x, .5f, pos.z);

            activeSalvage.Add(n); // add to list
        }
    }

    public void EnterNightCycle(){ //night cycle begins, salvage despawns
        foreach( GameObject s in activeSalvage){
            Destroy(s);
        }
    }
}
