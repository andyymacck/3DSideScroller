using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BasicInternetService : IInternetService
{
    private ServiceState m_serviceState;
    public ServiceState ServiceState => m_serviceState;
    public bool IsConnected => throw new System.NotImplementedException();
    
    public bool IsRunning => throw new System.NotImplementedException();

    public async Task Initialize()
    {
        
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
