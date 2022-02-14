using System.Collections;
using UnityEngine;

public class Enemy_Behaviour : MonoBehaviour
{
    // Inspector variables
    [SerializeField] private float enemySpeed;
    [SerializeField] private int enemyHealth;
    [SerializeField] private AudioClip hitEnemySound;
    [SerializeField] private AudioClip enemyDieSound;
    [SerializeField] private AudioClip enemyAttackSound;
    [SerializeField] private Animator animator;

    // Private variables
    private bool isAlive = true;
    private int health = 10;
    private bool isHit = false;
    private Rigidbody rigidbody;
    private AudioSource audioSource;
    private Collider collider;

    // Init
    void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody>();   
        audioSource = gameObject.GetComponent<AudioSource>();
        collider = gameObject.GetComponent<Collider>();
    }

    // Start attack player
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.other.tag == "MainCamera")
        animator.SetBool("isAttacking", true);
    }

    // Attacking player
    private void OnCollisionStay(Collision collision)
    {
        if (collision.other.tag == "MainCamera")
        {
            audioSource.clip = enemyAttackSound;
            if (!audioSource.isPlaying) audioSource.Play();
        }
    }

    // Main loop
    void Update()
    {
        if (isAlive && !isHit)
        {
            // Move toward player
            Vector3 position = -gameObject.transform.position;
            Vector2 dist = new Vector2(position.x, position.z).normalized * enemySpeed;
            Vector3 velocity = new Vector3(dist.x, rigidbody.velocity.y, dist.y);
            rigidbody.velocity = velocity;

            // Rotate toward player
            gameObject.transform.LookAt(Vector3.zero, Vector3.up);
            Vector3 angles = gameObject.transform.rotation.eulerAngles;
            gameObject.transform.rotation = Quaternion.Euler(new Vector3(0f, angles.y, angles.z));
        }
    }
    
    // Hurt enemy
    public void HitEnemy(int hitForce, int damage)
    {
        health -= damage;
        audioSource.clip = hitEnemySound;
        audioSource.Play();
        if (health <= 0)  StartCoroutine(KillEnemy());
    }

    // Kill enemy after n seconds
    IEnumerator KillEnemy()
    {
        isAlive = false;
        animator.SetBool("isDead", true);
        audioSource.clip = enemyDieSound;
        audioSource.Play();
        rigidbody.useGravity = false;
        collider.enabled = false;
        yield return new WaitForSeconds(5f);
        Destroy(this.gameObject);
    }
}
