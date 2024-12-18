using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AI;
//using UnityEditor.UI;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] public float health = 20f;
    [SerializeField] public float damage = 10f;

    [SerializeField] public float lookRadius = 10f;

    [SerializeField] public float attackRadius = 5f;

    [SerializeField] public float MoveSpeed = 10;

    [Header("Drops")]
    [SerializeField] private int eyeballDropCount;

    [SerializeField] private GameObject eyeballPrefab;

    [Header("Sounds")]

    [SerializeField] private List<AudioClip> idleSounds;
    [SerializeField] AudioClip attackSound;
    [SerializeField] AudioClip damageSound;
    [SerializeField] AudioClip deathSound;

    [SerializeField] Material hitMat;

    private bool hitColorOn;

    private float hitCt;

    NavMeshAgent agent;
    public Transform Target;

    private GameObject player;
    private Player movement;

    private Material regMat;
    private bool immobile;

    private float noiseCt;

    public int soundIntervalRangeStart;
    public int soundIntervalRangeEnd;

    private int soundInterval;

    private void Awake()
    {
        player = GameObject.Find("Player");
        movement = player.GetComponent<Player>();
        regMat = gameObject.transform.GetChild(2).gameObject.GetComponent<SkinnedMeshRenderer>().material;
    }

    private void Start()
    {
        Target = PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();

        //hit recoil 
        hitCt = 0;
        hitColorOn = false;
        immobile = false;
        noiseCt = 0;
        soundInterval = Random.Range(soundIntervalRangeStart,soundIntervalRangeEnd);
    }

    private void Update()
    {
        //damage color check

        if(hitColorOn){ //after being hit, return to usual color
            hitCt += Time.deltaTime;
            if(hitCt >=1){
                hitColorOn = false;
                //gameObject.transform.GetChild(2).gameObject.transform.GetComponent<Renderer>().material.color = Color.white;
                gameObject.transform.GetChild(2).gameObject.GetComponent<SkinnedMeshRenderer>().material = regMat;
            }
        }

        noiseCt+=Time.deltaTime;
        if(noiseCt>=soundInterval){ // play one of the 'idle' sounds every given interval
            noiseCt = 0;
            int s = Random.Range(0,idleSounds.Count);
            GetComponent<AudioSource>().PlayOneShot(idleSounds[s]);
        }


        float distance = Vector3.Distance(Target.position, transform.position);

        if(distance<= attackRadius){ //if enemy is close enough to player, attack
            //GetComponent<Animator>().SetTrigger("Attack");
        }

        if (distance <= lookRadius) //if enemy is close enough to see player, follow player
        {
            agent.SetDestination(Target.position);
        }

        //look at target
        transform.LookAt(Target.position);

        if(!immobile){
            // Movement towards the target
            Vector3 moveDir = (Target.position - transform.position).normalized;
            transform.position += (moveDir * MoveSpeed * Time.deltaTime);
        }

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
        //gameObject.transform.GetChild(2).gameObject.transform.GetComponent<Renderer>().material.color = Color.red;
        gameObject.transform.GetChild(2).gameObject.GetComponent<SkinnedMeshRenderer>().material = hitMat;

        GetComponent<Animator>().SetTrigger("Hit"); // trigger hit animation
        GetComponent<AudioSource>().PlayOneShot(damageSound);
        immobile = true;

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
            //GetComponent<Animator>().SetTrigger("Attack");
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

    private void BeginRecoil(){
        immobile = true;
        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }

    public void RecoilDone(){ //animation calls this, lets enemy move again
        immobile = false;
        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
    }
}
