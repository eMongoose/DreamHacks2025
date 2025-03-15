using UnityEngine;

public class DestroyParticleSystem : MonoBehaviour
{

    //public GameObject particleSystemObject;
    public float delay = 3f; // Time before destruction

    void Start()
    {
        Destroy(gameObject, delay);
    }
}
