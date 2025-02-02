using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SideScroller
{
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField] private List<Unit> m_enemyList = new List<Unit>();

        public List<Unit> EnemyList => m_enemyList;

        private void Awake()
        {
            for (int i = 0; i < m_enemyList.Count; i++)
            {
                m_enemyList[i].SetEnemyManager(this);
            }
        }

        public void AddEnemy(Unit unit)
        {
            m_enemyList.Add(unit);
        }

        public void RemoveEnemy(Unit unit)
        {
            m_enemyList.Remove(unit);
        }
    }
}
