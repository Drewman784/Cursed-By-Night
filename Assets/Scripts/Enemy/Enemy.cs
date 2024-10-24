using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AI;
using UnityEditor.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] public float health = 20f;
    [SerializeField] public float damage = 10f;

    [SerializeField] public float lookRadius = 10f;

    [SerializeField] public float attackRadius = 5f;

    [SerializeField] private int eyeballDropCount;

    [SerializeField] private GameObject eyeballPrefab;

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
    }

    private void Update()
    {
        float distance = Vector3.Distance(Target.position, transform.position);

        if(distance<= attackRadius){ //if enemy is close enough to player, attack
            GetComponent<Animator>().SetTrigger("Attack");
        }

        if (distance <= lookRadius) //if enemy is close enough to see player, follow player
        {
            agent.SetDestination(Target.position);
        }

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
        Debug.Log("enemy collision");
        if(other.gameObject.CompareTag("DefenseInteractable")){
            GetComponent<Animator>().SetTrigger("Attack");
        }
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("enemy trigger");
        if(other.gameObject.CompareTag("DefenseInteractable")){
            GetComponent<Animator>().SetTrigger("Attack");
        }
    }

    public void EnemyAttacks(){
        GetComponent<Animator>().SetTrigger("Attack");
    }
}
