using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> activeEnemies;

    [SerializeField] GameObject enemyPrefab;

    //[SerializeField] GameObject tempEnemyHolder;
    // Start is called before the first frame update

    [SerializeField] List<GameObject> enemySpawnLocations;
    private List<Vector3> spawnpoints;
    private int daynum;

    private bool day;
    void Start()
    {
     activeEnemies = new List<GameObject>();  
     daynum = 1; 
     day = true;

     spawnpoints = new List<Vector3>();
     foreach(GameObject loc in enemySpawnLocations){
        spawnpoints.Add(loc.transform.position);
     }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnEnemies(){
        day = false;
        /**
        for(int a = 0; a<num; a++){
            GameObject g = Instantiate(enemyPrefab);
            g.transform.position = new Vector3(-8,-.5f,-25);
            activeEnemies.Add(g);
        }**/
        int num = daynum;

        if(num>7){
            num = 7;
        }

        while(num>0){
            GameObject g = Instantiate(enemyPrefab);
            activeEnemies.Add(g);
            Debug.Log("spawned?");
            switch(num)
            {
                case 1: //spawnpoint on day 1
                    g.transform.position = spawnpoints[0];
                    Debug.Log("placed?");
                    Debug.Log("spot: " +spawnpoints[0]);
                    Debug.Log("loc: " + g.transform.position);
                    break;
                case 2: //spawn point on day 2
                    g.transform.position = spawnpoints[1];
                    break;
                case 3: //etc
                    g.transform.position = spawnpoints[2];
                    break;
                case 4: 
                    g.transform.position = spawnpoints[3];
                    break;
                case 5: 
                    g.transform.position = spawnpoints[4];
                    break;
                case 6: 
                    g.transform.position = spawnpoints[5];
                    break;
                case 7: 
                    g.transform.position = spawnpoints[6];
                    break;
            }
            num--;
        }

        //tempEnemyHolder.SetActive(true);
    }

    public void DeleteEnemies(){
        day = true;

        foreach(GameObject g in activeEnemies){
            //activeEnemies.Remove(g);
            Destroy(g);
        }
        activeEnemies = new List<GameObject>();

        daynum++;
        //tempEnemyHolder.SetActive(false);
    }

    public void RemoveEnemyFromActiveList(GameObject g){
        activeEnemies.Remove(g);

        if(activeEnemies.Count == 0 & !day){ //if enemies are defeated but it's still night, spawn more
            SpawnEnemies();
        }
    }
}
