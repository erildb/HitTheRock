using UnityEngine;
using UnityEngine.AI;

public class MovementAI : MonoBehaviour
{
    [SerializeField]
    public ParticleSystem smoke;
    [SerializeField]
    public GameObject hand;
    [SerializeField]
    public GameObject player;
    [SerializeField]
    public GameObject[] stones;

    private NavMeshAgent agent;
    private GameObject[] islands;
    private GameObject[] enemies;
    private GameObject stone;
    private Animator anim;
    private Vector3 dir;
    private int randEnemy;
    private int index = 0;
    private int index2 = 0;
    private int index3 = 1;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        islands = GameObject.FindGameObjectsWithTag("Islands");
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Invoke("SpawnNext", 2f);
    }

    private void SpawnNext()
    {
        GetComponent<Collider>().enabled = true;
        randEnemy = Random.Range(0, 1);
        Vector3 spawnPosition = new Vector3(islands[index % islands.Length].transform.position.x, islands[index % islands.Length].transform.position.y + 5, islands[index % islands.Length].transform.position.z - 1);
        GameObject stoneinst = Instantiate(stones[randEnemy], spawnPosition, Quaternion.identity);
        Destroy(stoneinst, 15f);
        ParticleSystem smokes = Instantiate(smoke, spawnPosition, Quaternion.identity);
        smokes.transform.parent = stoneinst.transform;
        smokes.Play();
        Invoke("GoToDestination", 1f);
    }

    private void GoToDestination()
    {
        agent.transform.eulerAngles = new Vector3(0, 90, 0);
        anim.SetBool("walk", true);
        agent.SetDestination(islands[index % islands.Length].transform.position + new Vector3(0, 0, -0.5f));
        index = index2 + index3;
        index2 = index3;
        index3 = index;
    }

    private void Update()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            GetComponent<Collider>().enabled = false;

            foreach (var x in enemies)
            {
                if (x.transform == col.transform)
                {
                    stone = x;
                    this.GetComponent<Rigidbody>().transform.LookAt(player.transform.position);
                }
            }
            Pick();
        }

        if (col.gameObject.tag == "Link")
        {
            anim.SetBool("walk", false);
            anim.SetBool("fly", true);
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Link")
        {
            anim.SetBool("fly", false);
            anim.SetBool("walk", true);
        }
    }

    private void Pick()
    {
        anim.SetBool("pick", true);
        stone.transform.parent = hand.transform;
        stone.GetComponent<Rigidbody>().useGravity = false;
    }

    private void StartThrow()
    {
        anim.SetBool("pick", false);
        anim.SetBool("throw", true);
    }

    private void Throw()
    {
        try
        {
            stone.transform.parent = null;
            stone.GetComponent<Rigidbody>().useGravity = true;
            stone.transform.rotation = player.transform.rotation;
            dir = player.transform.position - transform.position + new Vector3(0, 15, -2);
            Vector3 newDir = Vector3.RotateTowards(transform.forward, player.transform.position, 1f, 0.0f);
            stone.GetComponent<Rigidbody>().AddForce(dir * 70, ForceMode.Acceleration);
        }
        catch
        {
        }
        anim.SetBool("throw", false);
        Invoke("SpawnNext", 1f);
    }
}