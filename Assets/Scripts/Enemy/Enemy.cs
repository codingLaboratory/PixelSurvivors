using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D rb;
    private Vector3 direction;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float damage;
    [SerializeField] private float health;
    [SerializeField] private int experienceToGive;
    [SerializeField] private float pushTime;

    private float pushCounter;

    [SerializeField] private GameObject destroyEffect;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (PlayerController.Instance.gameObject.activeSelf){
            // face the player
            if (PlayerController.Instance.transform.position.x > transform.position.x){
                spriteRenderer.flipX = true;
            } else {
                spriteRenderer.flipX = false;
            }
            // push back
            if (pushCounter > 0){
                pushCounter -= Time.deltaTime;
                if (moveSpeed > 0){
                    moveSpeed = -moveSpeed;
                }
                if (pushCounter <= 0){
                    moveSpeed = Mathf.Abs(moveSpeed);
                }
            }
            // move towards the player
            direction = (PlayerController.Instance.transform.position - transform.position).normalized;
            rb.linearVelocity = new Vector2(direction.x * moveSpeed, direction.y * moveSpeed);
        } else {
            rb.linearVelocity = Vector2.zero;
        }
    }

    void OnCollisionStay2D(Collision2D collision){
        if (collision.gameObject.CompareTag("Player")){
            PlayerController.Instance.TakeDamage(damage);
        }
    }

    public void TakeDamage(float damage){
        health -= damage;
        DamageNumberController.Instance.CreateNumber(damage, transform.position);
        pushCounter = pushTime;
        if (health <= 0){
            Destroy(gameObject);
            Instantiate(destroyEffect, transform.position, transform.rotation);
            PlayerController.Instance.GetExperience(experienceToGive);
            AudioController.Instance.PlayModifiedSound(AudioController.Instance.enemyDie);
        }
    }
}
