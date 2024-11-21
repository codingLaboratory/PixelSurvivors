using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;

    [SerializeField] private float moveSpeed;
    public Vector3 playerMoveDirection;
    public Vector3 lastMoveDirection;
    public float playerMaxHealth;
    public float playerHealth;

    public int experience;
    public int currentLevel;
    public int maxLevel;

    [SerializeField] private List<Weapon> inactiveWeapons;
    public List<Weapon> activeWeapons;
    [SerializeField] private List<Weapon> upgradeableWeapons;
    public List<Weapon> maxLevelWeapons;

    private bool isImmune;
    [SerializeField] private float immunityDuration;
    [SerializeField] private float immunityTimer;

    public List<int> playerLevels;
    
    void Awake(){
        if (Instance != null && Instance != this){
            Destroy(this);
        } else {
            Instance = this;
        }
    }

    void Start(){
        lastMoveDirection = new Vector3(0, -1);
        for (int i = playerLevels.Count; i < maxLevel; i++){
            playerLevels.Add(Mathf.CeilToInt(playerLevels[playerLevels.Count - 1] * 1.1f + 15));
        }
        playerHealth = playerMaxHealth;
        UIController.Instance.UpdateHealthSlider();
        UIController.Instance.UpdateExperienceSlider();
        AddWeapon(Random.Range(0, inactiveWeapons.Count));
    }

    // Update is called once per frame
    void Update()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");
        playerMoveDirection = new Vector3(inputX, inputY).normalized;

        if (playerMoveDirection == Vector3.zero){
            animator.SetBool("moving", false);
        } else if (Time.timeScale != 0) {
            animator.SetBool("moving", true);
            animator.SetFloat("moveX", inputX);
            animator.SetFloat("moveY", inputY);
            lastMoveDirection = playerMoveDirection;
        }

        if (immunityTimer > 0){
            immunityTimer -= Time.deltaTime;
        } else {
            isImmune = false;
        }
    }

    void FixedUpdate(){
        rb.linearVelocity = new Vector3(playerMoveDirection.x * moveSpeed, playerMoveDirection.y * moveSpeed);
    }

    public void TakeDamage(float damage){
        if (!isImmune){
            isImmune = true;
            immunityTimer = immunityDuration;
            playerHealth -= damage;
            UIController.Instance.UpdateHealthSlider();
            if (playerHealth <= 0){
                gameObject.SetActive(false);
                GameManager.Instance.GameOver();
            }
        }
    }

    public void GetExperience(int experienceToGet){
        experience += experienceToGet;
        UIController.Instance.UpdateExperienceSlider();
        if (experience >= playerLevels[currentLevel - 1]){
            LevelUp();
        }
    }

    public void LevelUp(){
        experience -= playerLevels[currentLevel - 1];
        currentLevel++;
        UIController.Instance.UpdateExperienceSlider();
        //UIController.Instance.levelUpButtons[0].ActivateButton(activeWeapon);

        upgradeableWeapons.Clear();

        if (activeWeapons.Count > 0){
            upgradeableWeapons.AddRange(activeWeapons);
        }
        if (inactiveWeapons.Count > 0){
            upgradeableWeapons.AddRange(inactiveWeapons);
        }
        for (int i = 0; i < UIController.Instance.levelUpButtons.Length; i++){
            if (upgradeableWeapons.ElementAtOrDefault(i) != null){
                UIController.Instance.levelUpButtons[i].ActivateButton(upgradeableWeapons[i]);
                UIController.Instance.levelUpButtons[i].gameObject.SetActive(true);
            } else {
                UIController.Instance.levelUpButtons[i].gameObject.SetActive(false);
            }
        }

        UIController.Instance.LevelUpPanelOpen();
    }

    private void AddWeapon(int index){
        activeWeapons.Add(inactiveWeapons[index]);
        inactiveWeapons[index].gameObject.SetActive(true);
        inactiveWeapons.RemoveAt(index);
    }

    public void ActivateWeapon(Weapon weapon){
        weapon.gameObject.SetActive(true);
        activeWeapons.Add(weapon);
        inactiveWeapons.Remove(weapon);
    }

    public void IncreaseMaxHealth(int value){
        playerMaxHealth += value;
        playerHealth = playerMaxHealth;
        UIController.Instance.UpdateHealthSlider();

        UIController.Instance.LevelUpPanelClose();
        AudioController.Instance.PlaySound(AudioController.Instance.selectUpgrade);
    }

    public void IncreaseMovementSpeed(float multiplier){
        moveSpeed *= multiplier;

        UIController.Instance.LevelUpPanelClose();
        AudioController.Instance.PlaySound(AudioController.Instance.selectUpgrade);
    }
}
