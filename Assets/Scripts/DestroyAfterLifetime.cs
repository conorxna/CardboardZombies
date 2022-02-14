using System.Collections;
using UnityEngine;

public class DestroyAfterLifetime : MonoBehaviour
{
    // Inspector variables
    [SerializeField] private float lifeTime = 0.5f;

    // Init
    void Start()
    {
        StartCoroutine(DestoryAfterLifetime());
    }

    // Destroy after n seconds
    IEnumerator DestoryAfterLifetime()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(this.gameObject);
    }
}
