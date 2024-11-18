using UnityEngine;

public class DirectionalWeaponPrefab : MonoBehaviour
{
    private DirectionalWeapon weapon;
    private Rigidbody2D rb;
    private Vector3 direction;
    private float duration;

    void Start()
    {   
        weapon = GameObject.Find("Directional Weapon").GetComponent<DirectionalWeapon>();
        direction = PlayerController.Instance.lastMoveDirection;
        duration = weapon.stats[weapon.weaponLevel].duration;
        rb = GetComponent<Rigidbody2D>();
        float randomAngle = Random.Range(-0.2f, 0.2f);
        rb.linearVelocity = new Vector3(
            direction.x * weapon.stats[weapon.weaponLevel].speed + randomAngle, direction.y * weapon.stats[weapon.weaponLevel].speed + randomAngle);
        //Destroy(gameObject, weapon.stats[weapon.weaponLevel].duration);
        AudioController.Instance.PlaySound(AudioController.Instance.directionalWeaponSpawn);
    }


    void Update()
    {
        duration -= Time.deltaTime;
        if (duration <= 0){
            transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.zero, Time.deltaTime * 5);
            if (transform.localScale.x == 0f){
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision){
        if (collision.gameObject.CompareTag("Enemy")){
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.TakeDamage(weapon.stats[weapon.weaponLevel].damage);
            AudioController.Instance.PlaySound(AudioController.Instance.directionalWeaponHit);
        }
    }
}
