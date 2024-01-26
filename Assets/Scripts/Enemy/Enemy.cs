using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] float m_speed;
    NavMeshAgent m_agent;
    Vector3 m_destination;
    float m_defaultSpeed;

    public Action<Enemy> reachDestination;

    public void Init(Transform start, Vector3 destination)
    {
        transform.position = start.position;
        transform.rotation = start.rotation;
        m_destination = destination;
    }

    private void Awake()
    {
        m_agent = GetComponent<NavMeshAgent>();
        m_defaultSpeed = m_agent.speed;
        m_agent.speed = m_speed * m_defaultSpeed;
    }

    private void OnEnable()
    {
        m_agent.SetDestination(m_destination);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            reachDestination?.Invoke(this);
        }
    }
}
