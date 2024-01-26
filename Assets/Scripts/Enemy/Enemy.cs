using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] float m_speedMin;
    [SerializeField] float m_speedMax;
    [SerializeField] float m_maxSpeedAllCharacters;
    [SerializeField] GameObject[] m_characters;
    NavMeshAgent m_agent;
    Vector3 m_destination;
    float m_defaultSpeed;
    GameObject m_currentCharacter;
    Animator m_animator;

    public Action<Enemy> reachDestination;

    public void Init(Vector3 startPos, Quaternion startRot)
    {
        transform.position = startPos;
        transform.rotation = startRot;
        Debug.Log(transform.forward);
        m_destination = startPos + transform.forward * 20;
    }

    private void Awake()
    {
        m_agent = GetComponent<NavMeshAgent>();
        m_defaultSpeed = m_agent.speed;
        m_maxSpeedAllCharacters *= m_defaultSpeed;
        float speed = UnityEngine.Random.Range(m_speedMin, m_speedMax);
        m_agent.speed = speed * m_defaultSpeed;
        foreach (var character in m_characters)
            character.SetActive(false);
    }

    private void OnEnable()
    {
        m_agent.SetDestination(m_destination);
        if (m_currentCharacter != null)
            m_currentCharacter.SetActive(false);

        m_currentCharacter = m_characters[UnityEngine.Random.Range(0, m_characters.Length)];
        m_currentCharacter.SetActive(true);
        m_animator = m_currentCharacter.GetComponent<Animator>();
    }

    private void Update()
    {
        float speed = m_agent.speed / m_maxSpeedAllCharacters;
        m_animator.SetFloat("Speed", speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            reachDestination?.Invoke(this);
        }
    }
}
