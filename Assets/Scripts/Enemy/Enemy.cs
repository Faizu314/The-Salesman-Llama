using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] float m_speedMin;
    [SerializeField] float m_speedMax;
    [SerializeField] float m_maxSpeedAllCharacters;
    [SerializeField] int m_moneyToEarn;
    [SerializeField] GameObject[] m_characters;
    NavMeshAgent m_agent;
    Vector3 m_destination;
    float m_defaultSpeed;
    GameObject m_currentCharacter;
    Animator m_animator;
    FillTracker m_fillTracker;
    Collider m_collider;
    CancellationTokenSource m_tokenSource;

    private float m_EnemySpeed;

    public Transform BarberShopDestination { get; set; }
    public Action<Enemy> reachDestination;

    public void Init(Vector3 startPos, Quaternion startRot)
    {
        transform.position = startPos;
        transform.rotation = startRot;
        m_destination = startPos + transform.forward * 60;
    }

    public void AddDamage(float damage)
    {
        m_fillTracker.AddValue(damage);
    }

    private void Awake()
    {
        m_collider = GetComponent<Collider>();
        m_fillTracker = GetComponent<FillTracker>();
        m_fillTracker.reachedMax += OnReachedMax;
        m_agent = GetComponent<NavMeshAgent>();
        m_defaultSpeed = m_agent.speed;
        m_maxSpeedAllCharacters *= m_defaultSpeed;
        m_EnemySpeed = UnityEngine.Random.Range(m_speedMin, m_speedMax);
        foreach (var character in m_characters)
            character.SetActive(false);
    }

    private void OnEnable()
    {
        m_agent.speed = m_EnemySpeed * m_defaultSpeed;
        m_agent.SetDestination(m_destination);
        if (m_currentCharacter != null)
            m_currentCharacter.SetActive(false);

        m_currentCharacter = m_characters[UnityEngine.Random.Range(0, m_characters.Length)];
        m_currentCharacter.SetActive(true);
        m_collider.enabled = true;
        m_animator = m_currentCharacter.GetComponent<Animator>();
        m_fillTracker.ResetValue();
    }

    private void OnDisable()
    {
        if (m_tokenSource != null)
            m_tokenSource.Cancel();
    }

    private void Update()
    {
        float speed = m_agent.velocity.magnitude / m_maxSpeedAllCharacters;
        m_animator.SetFloat("Speed", speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Destination"))
        {
            reachDestination?.Invoke(this);
        }
    }

    private void OnReachedMax()
    {
        OverlayUI.Instance.AddMoney(m_moneyToEarn);
        m_tokenSource = new CancellationTokenSource();
        OnFillReachedMax(m_tokenSource).Forget();
    }

    private async UniTaskVoid OnFillReachedMax(CancellationTokenSource tokenSource)
    {
        m_collider.enabled = false;
        m_agent.isStopped = true;
        await UniTask.Delay(500);
        m_agent.isStopped = false;
        m_agent.speed = m_defaultSpeed;
        m_agent.SetDestination(BarberShopDestination.position);
        await UniTask.Delay(5000, cancellationToken: tokenSource.Token);
        if (!tokenSource.IsCancellationRequested)
            reachDestination?.Invoke(this);
    }
}
