using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlatformDestroyer : MonoBehaviour
{
    [SerializeField]
    private Image HealthBar;

    private Vector3 StartPosition = new Vector3();
    private Quaternion StartRotate;
    private bool create = false;
    private float PlatformHealth = 100;
    private float Starthealth = 100;

    private void Start()
    {
        StartPosition = transform.position;
        StartRotate = transform.rotation;
        PlatformHealth = Starthealth;
    }

    private void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "Enemy")
        {
            PlatformHealth -= PlayerPrefs.GetInt("platformDamage");
            HealthBar.fillAmount = PlatformHealth / Starthealth;
            if (PlatformHealth <= 0)
            {
                PlatformFall();
            }
        }
    }

    private void Update()
    {
        if (create == true)
        {
            transform.position = Vector3.Lerp(this.transform.position, StartPosition, Time.fixedDeltaTime * 2f);
            transform.rotation = Quaternion.Lerp(transform.rotation, StartRotate, Time.fixedDeltaTime * 2f);
            Invoke("SetCreateFalse", 2.5f);
        }

        if (transform.position.y <= -15)
        {
            Destruction();
        }
    }

    private void PlatformFall()
    {
        Invoke("DisableCollider", 1f * Time.fixedDeltaTime);
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
    }

    private void DisableCollider()
    {
        GetComponent<Collider>().enabled = false;
    }

    private void Destruction()
    {
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }

    public void CreatePlatform()
    {
        create = true;
        PlatformHealth = Starthealth;
        HealthBar.fillAmount = 100;
    }

    private void SetCreateFalse()
    {
        create = false;
        GetComponent<Collider>().enabled = true;
    }
}
