using UnityEngine;

public class Gun1911 : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public Camera fpsCam;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range)) 
        {
            Debug.Log(hit.transform.name);

           // Enemy enemy = hit.transform.GetComponet<Enemy>();
          //  if (enemy != null)
          //  {
          //      enemy.TakeDamage(damage);
          //  }
        }
    }
}
