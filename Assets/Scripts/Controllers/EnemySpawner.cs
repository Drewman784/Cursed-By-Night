using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> activeEnemies;

    [SerializeField] GameObject enemyPrefab;

    //[SerializeField] GameObject tempEnemyHolder;
    // Start is called before the first frame update

    private int daynum;
    void Start()
    {
     activeEnemies = new List<GameObject>();  
     daynum = 1; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnEnemies(){
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
                    g.transform.position = new Vector3(4,1.5f,-20);
                    Debug.Log("placed?");
                    break;
                case 2: //spawn point on day 2
                    g.transform.position = new Vector3(-6,1.5f,-20);
                    break;
                case 3: //etc
                    g.transform.position = new Vector3(-17,1.5f,-20f);
                    break;
                case 4: 
                    g.transform.position = new Vector3(-21,1.5f,-33f);
                    break;
                case 5: 
                    g.transform.position = new Vector3(8.5f,1.5f,-33f);
                    break;
                case 6: 
                    g.transform.position = new Vector3(-3.5f,1.5f,-58f);
                    break;
                case 7: 
                    g.transform.position = new Vector3(-14,1.5f,-58);
                    break;
            }
            num--;
        }
        daynum++;
        //tempEnemyHolder.SetActive(true);
    }

    public void DeleteEnemies(){
        foreach(GameObject g in activeEnemies){
            //activeEnemies.Remove(g);
            Destroy(g);
        }
        activeEnemies = new List<GameObject>();
        //tempEnemyHolder.SetActive(false);
    }

    public void RemoveEnemyFromActiveList(GameObject g){
        activeEnemies.Remove(g);
    }
}
