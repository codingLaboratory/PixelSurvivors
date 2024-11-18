using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance;

    public AudioSource pause;
    public AudioSource unpause;
    public AudioSource enemyDie;
    public AudioSource selectUpgrade;
    public AudioSource areaWeaponSpawn;
    public AudioSource areaWeaponDespawn;
    public AudioSource spinWeaponSpawn;
    public AudioSource spinWeaponDespawn;
    public AudioSource directionalWeaponSpawn;
    public AudioSource directionalWeaponHit;
    public AudioSource gameOver;

    private void Awake(){
        if (Instance != null && Instance != this){
            Destroy(this);
        } else {
            Instance = this;
        }
    }

    public void PlaySound(AudioSource sound){
        sound.Stop();
        sound.Play();
    }

    public void PlayModifiedSound(AudioSource sound){
        sound.pitch = Random.Range(0.7f, 1.3f);
        sound.Stop();
        sound.Play();
    }
}
