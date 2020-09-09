using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField]
    private Button easy;
    [SerializeField]
    private Button medium;
    [SerializeField]
    private Button hard;
    [SerializeField]
    private Image sky;
    [SerializeField]
    private Image sun;
    [SerializeField]
    private Image forest;
    [SerializeField]
    private Transform shootDirection;

    private AudioSource shootsound;
    private Ray shootRay = new Ray();
    private RaycastHit hit;
    private string skybox = null;
    private int targetHealth = 0;
    private int platformDamage;

    void Start()
    {
        shootsound = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton15))
        {
            Select();
        }
    }

    public void Select()
    {
        shootsound.Play();
        RaycastHit hit;
        shootRay.origin = shootDirection.transform.position;
        shootRay.direction = shootDirection.transform.forward;

        if (Physics.Raycast(shootRay, out hit, 100f))
        {
            if (hit.transform.gameObject.name == "Play" && targetHealth != 0 && skybox != null)
            {
                PlayerPrefs.SetInt("platformDamage", platformDamage);
                PlayerPrefs.SetInt("targetHealth", targetHealth);
                PlayerPrefs.SetString("skybox", skybox);
                SceneManager.LoadScene("Game");
            }
            if (hit.transform.gameObject.name == "Easy")
            {
                easy.Select();
                targetHealth = 20;
                platformDamage = 25;
            }
            if (hit.transform.gameObject.name == "Medium")
            {
                medium.Select();
                targetHealth = 40;
                platformDamage = 50;
            }
            if (hit.transform.gameObject.name == "Hard")
            {
                hard.Select();
                targetHealth = 60;
                platformDamage = 100;
            }
            if (hit.transform.gameObject.name == "Sky")
            {
                sky.color = Color.green;
                sun.color = Color.white;
                forest.color = Color.white;
                skybox = "sky";
            }
            if (hit.transform.gameObject.name == "Sun")
            {
                sun.color = Color.green;
                sky.color = Color.white;
                forest.color = Color.white;
                skybox = "sun";
            }
            if (hit.transform.gameObject.name == "Forest")
            {
                forest.color = Color.green;
                sky.color = Color.white;
                sun.color = Color.white;
                skybox = "forest";
            }
            if (hit.transform.gameObject.name == "Quit")
            {
                Application.Quit();
            }
        }
    }
}
