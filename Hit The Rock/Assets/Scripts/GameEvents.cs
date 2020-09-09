using UnityEngine;

public class GameEvents : MonoBehaviour
{
    [SerializeField]
    private GameObject[] platforms;
    [SerializeField]
    private GameObject ammo;
    [SerializeField]
    private AudioSource movingMountain;
    [SerializeField]
    private bool pause = false;
    [SerializeField]
    private Material mountainSkybox;
    [SerializeField]
    private Material sunSkybox;
    [SerializeField]
    private Material forestSkybox;
    [SerializeField]
    private GameObject pointer;
    [SerializeField]
    private GameObject teleporter;

    private Vector3 spawnplace;
    private float pos;

    void Start()
    {
        if (PlayerPrefs.GetString("skybox") == "sky")
        {
            RenderSettings.skybox = mountainSkybox;
        }
        else if (PlayerPrefs.GetString("skybox") == "sun")
        {
            RenderSettings.skybox = sunSkybox;
        }
        else
        {
            RenderSettings.skybox = forestSkybox;
        }

        movingMountain = GetComponent<AudioSource>();
        movingMountain.Play();
        InvokeRepeating("SpawnAmmmo", 10f, 20f);
    }

    void Update()
    {
        platforms = GameObject.FindGameObjectsWithTag("Platform");
        pos = Random.Range(2f, 4f);

        if (Input.GetKey(KeyCode.JoystickButton2) || Input.GetKey(KeyCode.LeftShift))
        {
            Time.timeScale = 0.5f;
        }
        else
        {
            if(!pause)
            {
                Time.timeScale = 1f;
            }
        }

        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.JoystickButton3))
        {
            if (pause)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    void SpawnAmmmo()
    {
        for (int x = 0; x < platforms.Length; x++)
        {
            if (platforms[x].gameObject.transform.position.y == 0)
            {
                spawnplace = platforms[x].transform.position;
            }
        }

        pos = Random.Range (-4f, 4f);
        Vector3 spawnPosition = new Vector3 (spawnplace.x + pos,spawnplace.y + 1f, spawnplace.z + pos);
        GameObject ammoInst = Instantiate (ammo, spawnPosition, Quaternion.identity);
        Destroy(ammoInst, 15f);
    }

    void Resume()
    {
        Time.timeScale = 1f;
        pause = false;
    }

    void Pause()
    {
        Time.timeScale = 0f;
        pause = true;
    }
}
