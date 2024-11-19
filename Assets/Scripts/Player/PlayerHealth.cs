using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{

    [SerializeField] private float player_starthp;
    [SerializeField] public float player_hp;
    [SerializeField] public float Zombie_Damage;
    [SerializeField] public float Enemy_2_Damage;

    [SerializeField] private float amountHealedPerDay;

    public Image healthBar;

    private static TextMeshProUGUI HealthText;

    // Start is called before the first frame update
    void Start()
    {
        // Game Start HP
        player_hp = player_starthp;

        HealthText = GameObject.Find("Health_Text").GetComponent<TextMeshProUGUI>();
        HealthText.SetText($"HP: {player_hp} ");
    }

    //Varying damage the player takes from enemies
    private void OnCollisionEnter(Collision other) 
    {
        Debug.Log ("Hit" + other.gameObject.tag);
        if (other.gameObject.CompareTag("Enemy"))
        {
            player_hp -= Zombie_Damage;
            Player_TakeDamage();
        }
        /**if (other.gameObject.CompareTag("Enemy 2"))
        {
            player_hp -= Enemy_2_Damage;
        }*/
    }

    private void OnTriggerEnter(Collider other){ //I THINK WE SHOULD USE THIS ONE
        Debug.Log ("Hit" + other.gameObject.tag);
        if (other.gameObject.CompareTag("Attack"))
        {
            player_hp -= Zombie_Damage;
            Player_TakeDamage();
        }
    }

    public void Player_TakeDamage()
    {
        if (player_hp <= 0f)
        {
            Player_Perish();
        }

        healthBar.fillAmount = player_hp / player_starthp; //update ui
    }

    void Player_Perish()
    {
        SceneManager.LoadScene("Main_Menu");
    }

    // Update is called once per frame
    void Update()
    {
        Player_TakeDamage();

        HealthText.SetText($"HP: {player_hp} ");
    }

    public void HealPlayer(){ //heal player by small amount every dawn
        player_hp += amountHealedPerDay;

        if(player_hp >= player_starthp){ //cap health
            player_hp = player_starthp;
        }

        healthBar.fillAmount = player_hp / player_starthp; //update ui
    }
}
