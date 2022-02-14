using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Inspector variables
    [SerializeField] private RectTransform healthFillTransform;
    [SerializeField] private Weapon_Template[] weaponsAvailable;
    [SerializeField] private Weapon_Template currentWeapon;
    [SerializeField] private GameObject weaponGameObject;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Weapon_Behaviour weaponScript;
    [SerializeField] private Game_Manager gameManager;

    // Private variables
    private int score = 0;
    private float health = 100f;
    private bool playerAlive = true;
    private int currentWeaponID;
    private AudioSource weaponAudioSource;
    private MeshFilter weaponMeshFilter;
    private MeshRenderer weaponMeshRenderer;

    // Init
    void Start()
    {
        currentWeaponID = 0;
        weaponAudioSource = weaponGameObject.GetComponent<AudioSource>();
        weaponMeshFilter = weaponGameObject.GetComponent<MeshFilter>();
        weaponMeshRenderer = weaponGameObject.GetComponent<MeshRenderer>();
        weaponScript.SwapWeapon(currentWeapon);
    }

    // Main loop
    void Update()
    {
        if (playerAlive)
        {
            // Shoot
            if (Input.GetMouseButton(0))
            {
                weaponScript.TryShoot();
            }

            // Swap weapon - Testing
            if (Input.GetMouseButtonDown(1))
            {
                currentWeaponID++;
                if (currentWeaponID >= weaponsAvailable.Length) currentWeaponID = 0;
                weaponScript.SwapWeapon(weaponsAvailable[currentWeaponID]);
            }
        }
    }

    // Damage player
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            health -= 0.1f;
            healthFillTransform.transform.localScale = new Vector3(health / 100f, 1f, 1f);

            if (health <= 0)
            {
                playerAlive = false;
                gameManager.EndGame();
            }
        }
    }

    // Increase player score
    public void AddScore(int value)
    {
        score += value;
        scoreText.text = score.ToString();
    }
}
