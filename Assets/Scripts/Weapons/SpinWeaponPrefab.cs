using UnityEngine;

public class SpinWeaponPrefab : MonoBehaviour
{
    private SpinWeapon weapon;
    private float duration;
    private Vector3 targetSize;
    [SerializeField] private GameObject projectile;

    void Start()
    {
        weapon = GameObject.Find("Spin Weapon").GetComponent<SpinWeapon>();
        duration = weapon.stats[weapon.weaponLevel].duration;
        //Destroy(gameObject, duration);
        targetSize = Vector3.one;
        transform.localScale = Vector3.zero;
        projectile.transform.localPosition = new Vector3(0f, weapon.stats[weapon.weaponLevel].range, 0f);
        AudioController.Instance.PlaySound(AudioController.Instance.spinWeaponSpawn);
    }

    void Update()
    {   
        // rotate
        transform.rotation = Quaternion.Euler(0f, 0f, transform.rotation.eulerAngles.z + (90 * Time.deltaTime * weapon.stats[weapon.weaponLevel].speed));
        // grow
        transform.localScale = Vector3.MoveTowards(transform.localScale, targetSize, Time.deltaTime * 3);
        // shrink
        duration -= Time.deltaTime;
        if (duration <= 0){
            targetSize = Vector3.zero;
            if (transform.localScale.x == 0f){
                AudioController.Instance.PlaySound(AudioController.Instance.spinWeaponDespawn);
                Destroy(gameObject);
            }
        }
    }

    public void SetRotationOffset(float rotationOffset){
        transform.rotation = Quaternion.Euler(0f, 0f, rotationOffset);
    }
}
