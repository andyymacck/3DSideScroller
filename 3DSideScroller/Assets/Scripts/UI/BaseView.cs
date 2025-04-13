using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseView : MonoBehaviour
{
    public abstract void Show();
    public abstract void Hide();
    public virtual void UpdateView()
    {
        Debug.Log("BaseView.UpdateView");
    }
}
