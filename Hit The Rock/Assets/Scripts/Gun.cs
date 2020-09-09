using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Gun : MonoBehaviour
{
    // References like score can be replaced with Scriptable Objects 
    #region Variables
    public float damage = 10f;
    public ParticleSystem particle;
    public Animator animator;
    public Text ammoLeft;
    public GameObject impact;
    public Transform shootDirection;
    public Text Score;
    public int score;
    public int GameScore = 0;

    private GameObject[] stones;
    private AudioSource shootsound;
    private Quaternion StartRotate;
    private bool respawn = false;
    private bool StopFall = false;
    private bool isReloading = false;
    private int lifes = 2;
    private int maxAmmo = 10;
    private int currentAmmo;
    private int fullAmmo = 50;
    private float reloadTime = 1f;
    private Ray shootRay = new Ray();
    private RaycastHit hit;
    private Vector3 StartPosition = new Vector3();
    private Vector3 temp;
    private Vector3 startp;

    [SerializeField]
    private GameObject[] platforms;
    [SerializeField]
    private GameObject[] Healths;
    [SerializeField]
    private Image[] Hearts;
    [SerializeField]
    private GameObject SpawnPosition;
    [SerializeField]
    private GameObject fpscontroller;
    [SerializeField]
    private GameObject fpscontrollerCollider;
    [SerializeField]
    private GameObject movementAmplifier;
    [SerializeField]
    private GameObject m_shotPrefab;
    [SerializeField]
    private GameObject MovementAI3;
    [SerializeField]
    private GameObject MovementAI4;
    #endregion

    void Start()
    {
        startp = transform.position;
        temp = new Vector3(-14.71087f, 5f, -14.71087f);
        StartRotate = fpscontroller.transform.rotation;
        ammoLeft.text = maxAmmo + " / " + maxAmmo + "\n" + fullAmmo;
        shootsound = GetComponent<AudioSource>();
        currentAmmo = maxAmmo;
    }

    void Update()
    {
        if (score > 10)
        {
            MovementAI3.SetActive(true);
        }

        if (score > 20)
        {
            MovementAI4.SetActive(true);
        }

        //Oculus
        if (Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            SceneManager.LoadScene("Menu");
        }

        if (isReloading)
        {
            return;
        }

        if (currentAmmo <= 0 && fullAmmo > 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (fpscontroller.transform.position.y <= -1 && StopFall == false)
        {
            movementAmplifier.gameObject.SetActive(false);
        }

        if (fpscontroller.transform.position.y <= -5 && StopFall == false)
        {
            LoseLife();
            StopFall = true;
        }

        Score.text = GameScore.ToString();
        platforms = GameObject.FindGameObjectsWithTag("Platform");
        Healths = GameObject.FindGameObjectsWithTag("Healths");

        //Oculus
        if (Input.GetKeyDown(KeyCode.JoystickButton15))
        {
            Shoot();
        }

        stones = GameObject.FindGameObjectsWithTag("Enemy");
    }

    private IEnumerator Reload()
    {
        isReloading = true;

        yield return new WaitForSeconds(reloadTime);

        if (fullAmmo > 0)
        {
            currentAmmo = maxAmmo;
            fullAmmo -= maxAmmo;
            ammoLeft.text = currentAmmo + " / " + maxAmmo + "\n" + fullAmmo;
            isReloading = false;
        }
    }

    public void GetAmmo()
    {
        fullAmmo += 50;

        if (currentAmmo == 0)
        {
            currentAmmo = maxAmmo;
            fullAmmo -= maxAmmo;
        }

        ammoLeft.text = currentAmmo + " / " + maxAmmo + "\n" + fullAmmo;
    }

    public void Shoot()
    {
        ammoLeft.text = currentAmmo - 1 + " / " + maxAmmo + "\n" + fullAmmo;
        currentAmmo--;
        shootsound.Play();
        shootRay.origin = shootDirection.transform.position;
        shootRay.direction = shootDirection.transform.forward;
        GameObject go = GameObject.Instantiate(m_shotPrefab, shootDirection.transform.position + new Vector3(0, 0, 0), shootDirection.transform.rotation) as GameObject;
        Destroy(go, 3f);

        if (Physics.Raycast(shootRay, out hit, 100f))
        {
            if (hit.transform.gameObject.name == "Menu")
            {
                SceneManager.LoadScene("Menu");
            }

            Target target = hit.transform.GetComponent<Target>();

            if (target != null)
            {
                if (target.tag == "Enemy")
                {
                    target.TakeDamage(damage);
                    if (score % 5 == 0)
                    {
                        RespawnPlatform();
                    }
                }
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * 80);
            }

            GameObject impactshoot = Instantiate(impact, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactshoot, 0.1f);
        }
    }

    public void LoseLife()
    {
        RespawnPlatform();
        if (lifes < 0)
        {
            Invoke("GameOver", 2f * Time.fixedDeltaTime);
        }
        else
        {
            Hearts[lifes].enabled = false;
            lifes--;
        }

        Invoke("RespawnPlayer", 2.5f * Time.fixedDeltaTime);
    }

    private void RespawnPlatform()
    {
        for (int x = 0; x < platforms.Length; x++)
        {
            if (platforms[x].gameObject.transform.position.y < -2)
            {
                platforms[x].GetComponent<PlatformDestroyer>().CreatePlatform();
                score = 0;
                break;
            }
        }
    }

    private void RespawnPlayer()
    {
        for (int x = 0; x < platforms.Length; x++)
        {
            if (platforms[x].gameObject.transform.position.y == 0)
            {
                StartPosition = platforms[x].transform.position;
                break;
            }
        }

        fpscontroller.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        fpscontroller.transform.position = StartPosition + new Vector3(0f, 0f, 0f);
        StartGame();
    }

    private void StartGame()
    {
        SpawnPosition.GetComponent<Collider>().enabled = true;
        StopFall = false;
        Invoke("RestartTime", 0.5f);
    }

    private void RestartTime()
    {
        movementAmplifier.gameObject.SetActive(true);
    }

    private void GameOver()
    {
        SceneManager.LoadScene("Menu");
    }
}
