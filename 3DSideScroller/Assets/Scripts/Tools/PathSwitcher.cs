using SideScroller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathSwitcher : MonoBehaviour
{
    [SerializeField] private CatmullRomGenPoints m_targetPath;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.PLAYER_TAG_ID))
        {
            PathAgent pathAgent = other.GetComponent<PathAgent>();

            if (pathAgent != null )
            {
                pathAgent.Initialize(m_targetPath);
            }
        }
    }
}
