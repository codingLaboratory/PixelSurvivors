using UnityEngine;

public class SpinWeapon : Weapon
{
    public GameObject prefab;
    private float spawnCounter;

    void Update()
    {
        spawnCounter -= Time.deltaTime;
        if (spawnCounter <= 0){
            spawnCounter = stats[weaponLevel].cooldown;

            for (int i = 0; i < stats[weaponLevel].amount; i++){
                GameObject spawnedWeapon = Instantiate(prefab, transform.position, transform.rotation, transform);
                float rotation = 360f / stats[weaponLevel].amount * i;
                spawnedWeapon.GetComponent<SpinWeaponPrefab>().SetRotationOffset(rotation);
            }

        }
    }
}
