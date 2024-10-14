using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> activeEnemies;

    [SerializeField] GameObject enemyPrefab;

    [SerializeField] GameObject tempEnemyHolder;
    // Start is called before the first frame update
    void Start()
    {
     activeEnemies = new List<GameObject>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnEnemies(int num){
        /**for(int a = 0; a<num; a++){
            GameObject g = Instantiate(enemyPrefab);
            g.transform.position = new Vector3(-8,-.5f,-25);
            activeEnemies.Add(g);
        }*/

        tempEnemyHolder.SetActive(true);
    }

    public void DeleteEnemies(){
        /*foreach(GameObject g in activeEnemies){
            activeEnemies.Remove(g);
            Destroy(g);
        }*/
        tempEnemyHolder.SetActive(false);
    }
}
