using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SideScroller
{
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField] private List<Unit> m_enemyList = new List<Unit>();

        public List<Unit> EnemyList => m_enemyList;

        void Start()
        {

        }

  
        void Update()
        {

        }
    }
}
