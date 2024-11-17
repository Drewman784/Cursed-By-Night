using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AI;
//using UnityEditor.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] public float health = 20f;
    [SerializeField] public float damage = 10f;

    [SerializeField] public float lookRadius = 10f;

    [SerializeField] public float attackRadius = 5f;

    [SerializeField] private int eyeballDropCount;

    [SerializeField] private GameObject eyeballPrefab;

    private bool hitColorOn;

    private float hitCt;

    NavMeshAgent agent;
    public Transform Target;
    [SerializeField] public float MoveSpeed = 10;
    private GameObject player;
    private Player movement;

    private void Awake()
    {
        player = GameObject.Find("Player");
        movement = player.GetComponent<Player>();
    }

    private void Start()
    {
        Target = PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();

        //hit recoil 
        hitCt = 0;
        hitColorOn = false;
    }

    private void Update()
    {
        //damage color check

        if(hitColorOn){ //after being hit, return to usual color
            hitCt += Time.deltaTime;
            if(hitCt >=1){
                hitColorOn = false;
                gameObject.transform.GetChild(2).gameObject.transform.GetComponent<Renderer>().material.color = Color.white;
            }
        }


        float distance = Vector3.Distance(Target.position, transform.position);

        if(distance<= attackRadius){ //if enemy is close enough to player, attack
            GetComponent<Animator>().SetTrigger("Attack");
        }

        if (distance <= lookRadius) //if enemy is close enough to see player, follow player
        {
            agent.SetDestination(Target.position);
        }

        //look at target
        transform.LookAt(Target.position);

        // Movement towards the target
        Vector3 moveDir = (Target.position - transform.position).normalized;
        transform.position += (moveDir * MoveSpeed * Time.deltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    public void TakeDamage(float amount) //enemy takes damage
    {
        health -= amount;
        Debug.Log("Enemy Health: " + health); // Debug log for health tracking
        Debug.Log("HIT FOR: " + amount);
        hitColorOn = true; //change color when hit
        hitCt = 0;
        gameObject.transform.GetChild(2).gameObject.transform.GetComponent<Renderer>().material.color = Color.red;

        if (health <= 0f)
        {
            Die();
        }
    }

    private void Die() //enemy dies
    {
        GameObject c = GameObject.Find("DayNightCycleController");  //THIS IS CURRENTLY WHERE ENEMYSPAWNER IS. JUST NEED THE GAMEOBJECT
        c.GetComponent<EnemySpawner>().RemoveEnemyFromActiveList(this.gameObject);
        for(int a =0; a< eyeballDropCount;a++){
            GameObject b = Instantiate(eyeballPrefab);
            b.transform.position = transform.position;
        }
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.CompareTag("DefenseInteractable")){
            Debug.Log("enemy collision w def");
            GetComponent<Animator>().SetTrigger("Attack");
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Trap Attack")){
            TakeDamage(other.gameObject.GetComponentInParent<DefenseBase>().DealDamage());
        } /**else if(other.gameObject.CompareTag("DefenseInteractable")){
            Debug.Log("enemy trigger w def");
            GetComponent<Animator>().SetTrigger("Attack");
        }*/
    }
}
