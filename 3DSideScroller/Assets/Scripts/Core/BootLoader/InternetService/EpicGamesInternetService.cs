using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EpicGamesInternetService : IInternetService
{
    public bool IsConnected => throw new System.NotImplementedException();

    public bool IsRunning => throw new System.NotImplementedException();

    public void Initialize()
    {
        Debug.Log("EpicGamesInternetService.Initialize");
        throw new System.NotImplementedException();
    }

    public void Shutdown()
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
