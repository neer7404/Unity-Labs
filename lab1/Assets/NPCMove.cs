using UnityEngine;
using UnityEngine.AI;

public class NPCMove : MonoBehaviour
{
    public Transform[] waypoints;
    private int index = 0;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        SetDestination();
    }

    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            index = (index + 1) % waypoints.Length;
            SetDestination();
        }
    }

    void SetDestination()
    {
        if (waypoints.Length > 0)
            agent.destination = waypoints[index].position;
    }

    

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the sensing circle is the Player
        if (other.CompareTag("Player"))
        {
            Debug.Log("NPC: I sensed the Player!");
            // Turn Red when player is near
            GetComponent<Renderer>().material.color = Color.red;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("NPC: Player left my range.");
            // Turn back to White when player leaves
            GetComponent<Renderer>().material.color = Color.white;
        }
    }
} 