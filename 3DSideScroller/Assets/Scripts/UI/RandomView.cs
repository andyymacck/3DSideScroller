using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomView : BaseView
{
    public override void Show()
    {
        Debug.Log("RandomView.Show");
    }

    public override void Hide()
    {
        Debug.Log("RandomView.Hide");
    }

    public void ToDo()
    {
        Debug.Log("RandomView.ToDo");
    }
}
