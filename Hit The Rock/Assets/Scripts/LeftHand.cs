using UnityEngine;

public class LeftHand : MonoBehaviour
{
    [SerializeField]
    private GameObject gun;
    private AudioSource pickUp;

    private void Start()
    {
        pickUp = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Ammo")
        {
            gun.GetComponent<Gun>().GetAmmo();
            pickUp.Play();
            Destroy(col.gameObject);
        }
    }
}
