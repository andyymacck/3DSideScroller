using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnyView : BaseView, ICleanable
{
    public override void Show()
    {
        Debug.Log("AnyView.Show");
    }

    public override void Hide()
    {
        Debug.Log("AnyView.Hide");
    }

    public override void UpdateView()
    {
        Debug.Log("AnyView.UpdateView");
        base.UpdateView();
    }

    public void ToDo()
    {
        Debug.Log("AnyView.ToDo");
    }

    public void Clean()
    {
        Debug.Log("AnyView.NotTodo");
    }
}
