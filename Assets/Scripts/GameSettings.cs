using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/Game Settings", fileName = "Game Settings")]
public class GameSettings : ScriptableObject
{

    [SerializeField] float m_gameTime;
    [SerializeField] float m_gameOverCountdownTime;
    [SerializeField] float m_moneyGoal;

    public float GameTime => m_gameTime;
    public float GameOverCountdownTime => m_gameOverCountdownTime;
    public float MoneyGoal => m_moneyGoal;
}
