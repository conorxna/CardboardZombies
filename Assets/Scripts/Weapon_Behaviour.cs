using System.Collections;
using UnityEngine;

public class Weapon_Behaviour : MonoBehaviour
{
    // Inspector variables
    [SerializeField] private GameObject muzzleFlashObject;
    [SerializeField] private float muzzleFlashDuration = 0.1f;
    [SerializeField] private Vector3 muzzleOffset;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameObject bloodSplat;
    [SerializeField] private Player player;
    [SerializeField] private float offsetX;
    [SerializeField] private float offsetY;
    [SerializeField] private float offsetZ;

    // Private variables
    private Weapon_Template weaponTemplate;
    private bool isShooting = false;
    private bool isReloading = false;
    private int shotsLeftInClip = 10;
    private GameObject currentWeaponObject;
    private Animator animator;
    private GameObject muzzleObject;

    // Init
    private void Start()
    {
        muzzleObject = GameObject.Instantiate(muzzleFlashObject, gameObject.transform);
        muzzleObject.transform.localPosition = muzzleOffset;
        muzzleObject.SetActive(false);
    }

    // Load in new weapon
    public void SwapWeapon(Weapon_Template newWeaponTemplate)
    {
        if (currentWeaponObject != null) Destroy(currentWeaponObject);
        weaponTemplate = newWeaponTemplate;
        audioSource.clip = newWeaponTemplate.equipSound;
        audioSource.Play();
        audioSource.clip = newWeaponTemplate.shootSound;
        shotsLeftInClip = newWeaponTemplate.shotsInClip;
        currentWeaponObject = GameObject.Instantiate(newWeaponTemplate.prefab, gameObject.transform);
        currentWeaponObject.transform.localRotation = Quaternion.Euler(0, 180f, 0);
        currentWeaponObject.transform.localPosition = new Vector3(offsetX, offsetY, offsetZ);
        animator = currentWeaponObject.GetComponent<Animator>();
        animator.runtimeAnimatorController = newWeaponTemplate.animatorController;
        Debug.Log(currentWeaponObject.name);
    }

    // Shoot if weapon ready
    public void TryShoot()
    {
        if (!isShooting && !isReloading)
        {
            if (Input.GetMouseButtonDown(0) || weaponTemplate.isAutomatic)
            {
                // Shoot weapon
                BulletRaycast();
                shotsLeftInClip--;
                audioSource.Play();
                StartCoroutine(MuzzleFlash());

                // Reload if needed
                if (shotsLeftInClip > 0)
                    StartCoroutine(DelayBetweenShots());
                else
                    StartCoroutine(DelayForReload());
            }
        }
    }

    // Flash texture in front of gun
    IEnumerator MuzzleFlash()
    {
        muzzleObject.SetActive(true);
        muzzleObject.transform.localRotation = Quaternion.Euler(0f, 0f, Random.Range(0, 360f));
        yield return new WaitForSeconds(muzzleFlashDuration);
        muzzleObject.SetActive(false);
    }

    // Handle bullet collisions
    private void BulletRaycast()
    {
        RaycastHit objectHit;
        Vector3 forward = gameObject.transform.TransformDirection(Vector3.forward);
        Debug.DrawRay(gameObject.transform.position, forward * weaponTemplate.range, Color.green);
        if (Physics.Raycast(gameObject.transform.position, forward, out objectHit, weaponTemplate.range))
        {
            if (objectHit.transform.tag == "Enemy")
            { 
                GameObject obj = GameObject.Instantiate(bloodSplat, objectHit.point, Quaternion.identity);
                obj.transform.LookAt(Vector3.zero, Vector3.up);
                objectHit.transform.gameObject.GetComponent<Enemy_Behaviour>().HitEnemy(weaponTemplate.hitForce, weaponTemplate.damage);
                player.AddScore(1);
            }
        }
    }

    // Limit rate of fire 
    IEnumerator DelayBetweenShots()
    {
        isShooting = true;
        animator.SetBool("isShooting", true);
        yield return new WaitForSecondsRealtime(60f / weaponTemplate.roundsPerMinute);
        isShooting = false;
        if (!weaponTemplate.isAutomatic || !Input.GetMouseButtonDown(0) || isReloading) 
            animator.SetBool("isShooting", false);
    }

    // Wait for gun to reload
    IEnumerator DelayForReload()
    {
        isReloading = true;
        animator.SetBool("isReloading", true);
        audioSource.clip = weaponTemplate.reloadSound;
        audioSource.Play();
        yield return new WaitForSecondsRealtime(weaponTemplate.reloadTime);
        isReloading = false;
        animator.SetBool("isReloading", false);
        shotsLeftInClip = weaponTemplate.shotsInClip;
        audioSource.clip = weaponTemplate.shootSound;
    }
}
