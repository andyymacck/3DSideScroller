using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerPrefsSavesService : ISavesService
{
    private ServiceState m_serviceState;
    public ServiceState ServiceState => m_serviceState;
    public bool IsAllSaved => throw new System.NotImplementedException();

    public bool IsRunning => throw new System.NotImplementedException();

    public object GetValue(string name)
    {
        throw new System.NotImplementedException();
    }

    public Task Initialize()
    {
        throw new System.NotImplementedException();
    }

    public void SaveValue(string name, object value)
    {
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
