using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SideScroller
{
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField] private List<EnemyBase> m_enemyList = new List<EnemyBase>();

        public List<EnemyBase> EnemyList => m_enemyList;

        public void Initialize(PlayerController player)
        {
            for (int i = 0; i < m_enemyList.Count; i++)
            {
                m_enemyList[i].SetEnemyManager(this);
                m_enemyList[i].SetPlayer(player);
            }
        }

        public void AddEnemy(EnemyBase enemy)
        {
            m_enemyList.Add(enemy);
        }

        public void RemoveEnemy(EnemyBase enemy)
        {
            m_enemyList.Remove(enemy);
        }
    }
}