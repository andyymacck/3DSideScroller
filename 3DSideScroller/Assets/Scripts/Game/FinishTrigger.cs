using UnityEngine;

namespace SideScroller
{
    public class FinishTrigger : MonoBehaviour
    {
        private bool m_isFinished = false;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(Constants.PLAYER_TAG_ID))
            {
                if (!m_isFinished)
                {
                    m_isFinished = true;
                    int score = 0;
                    EventHub.Instance.Publish(new LevelFinishedEvent(score));
                    Debug.Log("finish");
                }
            }
        }
    }
}