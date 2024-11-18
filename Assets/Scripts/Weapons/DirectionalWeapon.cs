using UnityEngine;

public class DirectionalWeapon : Weapon
{   
    [SerializeField] private GameObject prefab;
    private float spawnCounter;

    void Update()
    {
        spawnCounter -= Time.deltaTime;
        if (spawnCounter <= 0){
            spawnCounter = stats[weaponLevel].cooldown;
            for (int i = 0; i < stats[weaponLevel].amount; i++){
                Instantiate(prefab, transform.position, transform.rotation, transform);
            }
        }
    }
}
