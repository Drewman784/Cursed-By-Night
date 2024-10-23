using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{

    [SerializeField] private float player_starthp;
    [SerializeField] public float player_hp;
    [SerializeField] public float Zombie_Damage;
    [SerializeField] public float Enemy_2_Damage;

    // Start is called before the first frame update
    void Start()
    {
        // Game Start HP
        player_hp = player_starthp;
    }

    //Varying damage the player takes from enemies
    private void OnCollisionEnter(Collision other) 
    {
        if (other.gameObject.CompareTag("Zombie Enemy"))
        {
            player_hp -= Zombie_Damage;
        }
        /**if (other.gameObject.CompareTag("Enemy 2"))
        {
            player_hp -= Enemy_2_Damage;
        }*/
    }

    public void Player_TakeDamage()
    {
        if (player_hp <= 0f)
        {
            Player_Perish();
        }
    }

    void Player_Perish()
    {
        SceneManager.LoadScene("Main_Menu");
    }

    // Update is called once per frame
    void Update()
    {
        Player_TakeDamage();
    }
}
