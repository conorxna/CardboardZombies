using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class Weapon_Template : ScriptableObject
{
    // Weapon data
    public string name = "Weapon";
    public bool isAutomatic = false;
    public float roundsPerMinute = 60;
    public float reloadTime = 2f;
    public int shotsInClip = 10;
    public int hitForce = 1;
    public int damage = 100;
    public int range = 50;
    public GameObject prefab;
    public AudioClip shootSound;
    public AudioClip reloadSound;
    public AudioClip equipSound;
    public AnimatorController animatorController;
}
