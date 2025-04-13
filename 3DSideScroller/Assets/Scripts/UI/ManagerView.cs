using SideScroller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerView : MonoBehaviour
{
 
    [ContextMenu("Do something")]
    public void DoSomething()
    {
        AnyView anyView = new AnyView();
        RandomView randomView = new RandomView();

        Debug.Log("anyView");
        anyView.Show();
        anyView.UpdateView();
        anyView.ToDo();
        anyView.Clean();

        ICleanable notView = anyView;
        notView.Clean();

        ICleanable lm = new LevelController();
        lm.Clean();

        List<ICleanable> list = new List<ICleanable>();
        list.Add(lm);
        list.Add(notView);
        list.Add(lm);

        foreach (ICleanable l in list)
        {
            l.Clean();
        }

        //randomView.Show();
        //randomView.Hide();
        //randomView.UpdateView();
        //randomView.ToDo();

        BaseView anyViewBase = new AnyView();
        BaseView randomViewBase = new RandomView();

        Debug.Log("anyViewBase");
        anyViewBase.Show();
        anyViewBase.UpdateView();
        randomViewBase.Show();
        randomViewBase.UpdateView();

    }
}
