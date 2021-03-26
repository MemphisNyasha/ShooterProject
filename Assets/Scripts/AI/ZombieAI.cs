using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class ZombieAI : MonoBehaviour, IDamageable
{
    public float WalkRadius = 15f;
    public float MinSpeed = 0.5f;
    public float MaxSpeed = 5f;

    private NavMeshAgent agent;
    private Animator animator;
    private Vector3 destination;
    private float velocity;
    private float animationSpeed;
    private Coroutine walkCoroutine;

    public float AgentSpeed => agent.velocity.magnitude;

    private void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        animator = this.GetComponent<Animator>();

        walkCoroutine = StartCoroutine(StartWalking());
    }

    private void Update()
    {
        animationSpeed = Mathf.SmoothDamp(animationSpeed, AgentSpeed, ref velocity, 0.5f);
        animator.SetFloat("Speed", animationSpeed);
    }

    private IEnumerator StartWalking()
    {
        Vector3 point = Random.insideUnitSphere * WalkRadius;
        point += transform.position;

        if (NavMesh.SamplePosition(point, out var hit, WalkRadius, 1))
        {
            destination = hit.position;
            agent.SetDestination(destination);
        }

        agent.speed = Random.Range(MinSpeed, MaxSpeed);

        while (this.transform.position != destination)
        {
            yield return null;
        }

        walkCoroutine = StartCoroutine(StartWalking());
    }

    public void OnDamage()
    {
        animator.enabled = false;
        agent.speed = 0f;
        if (walkCoroutine != null) StopCoroutine(walkCoroutine);
    }
}
