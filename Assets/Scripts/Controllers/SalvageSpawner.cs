using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using UnityEngine;

//this script goes on DayNightCycleController
//this spawns salvage during the day cycle and despawns it during the night
public class SalvageSpawner : MonoBehaviour
{
    public List<GameObject> activeSalvage;

    [SerializeField] GameObject salvagePrefab;

    [SerializeField] List<GameObject> centerOfSpawnArea;
    private List<Vector3> spawnPoints;

    // Start is called before the first frame update

    void Start(){
        spawnPoints = new List<Vector3>();
        foreach(GameObject s in centerOfSpawnArea){
            spawnPoints.Add(s.gameObject.transform.position);
            //Debug.Log("adding " +s );
        }
    }

    public void EnterDayCycle(){ //day cycle begins, salvage spawns
        //Debug.Log("got here");
        if(spawnPoints == null){ //patch job on intial day cycle causing errors
            Patch();
        }

        int numToSpawn = 5;
        foreach (Vector3 spot in spawnPoints){
            for(int a =0; a<numToSpawn;a++){
                //Debug.Log(spot+ " salvage spawned");

                GameObject n = Instantiate(salvagePrefab); //create salvage

                Vector3 pos = Random.insideUnitCircle * 10; //place around spawn point
                 pos += spot;
                 n.transform.position = new Vector3(pos.x, .5f, pos.z);

                activeSalvage.Add(n); // add to list
        }
        }

    }

    public void EnterNightCycle(){ //night cycle begins, salvage despawns
        foreach( GameObject s in activeSalvage){
            Destroy(s);
        }
    }

    private void Patch(){ //converts gameobjects to spawn locations vector3
        spawnPoints = new List<Vector3>();
        foreach(GameObject s in centerOfSpawnArea){
            spawnPoints.Add(s.gameObject.transform.position);
            Debug.Log("adding " +s );
        }
    }
}
