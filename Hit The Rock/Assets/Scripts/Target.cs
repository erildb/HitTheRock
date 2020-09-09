using System.Collections;
using UnityEngine;
public class Target : MonoBehaviour
{
    [SerializeField]
    private GameObject impact;

    private float health;
    private AudioSource crash;

    private void Start()
    {
        health = PlayerPrefs.GetInt("targetHealth");
        crash = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (transform.position.y <= -1)
        {
            Destruction();
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            GameObject impactshoot = Instantiate(impact, transform.position, Quaternion.LookRotation(transform.rotation.eulerAngles));
            Destroy(impactshoot, 2f);
            Destruction();
            GameObject.Find("flaregun").GetComponent<Gun>().score++;
            GameObject.Find("flaregun").GetComponent<Gun>().GameScore++;
        }
    }

    private IEnumerator OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Platform" || col.gameObject.tag == "Islands" || col.gameObject.tag == "Islands2")
        {
            crash.Play();
        }

        if (col.gameObject.tag == "Player")
        {
            gameObject.GetComponent<Collider>().enabled = false;
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            yield return new WaitForSeconds(0.5f);
            Destruction();
        }
    }

    private void Destruction()
    {
        Destroy(gameObject);
    }
}