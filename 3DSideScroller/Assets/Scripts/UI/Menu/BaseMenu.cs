using UnityEngine;
using UnityEngine.UI;

namespace SideScroller
{
    public abstract class BaseMenu : MonoBehaviour
    {
        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        public virtual void Close()
        {
            gameObject.SetActive(false);
        }
    }
}
