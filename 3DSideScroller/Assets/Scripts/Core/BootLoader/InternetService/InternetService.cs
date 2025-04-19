using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

namespace Core.Services
{
    public class InternetService : IInternetService
    {
        private bool m_isConnected;
        private HttpClient m_client;
        private int m_currentIndex = 0;
        private int m_timeOut = 4;
        private const int c_timeOutFast = 1;
        private const int c_timeOutSlow = 4;
        private readonly Dictionary<string, bool> m_sites = new();
        private readonly string[] m_sitesList = new string[]
        {
            "https://www.google.com",
            "https://www.cloudflare.com",
            "https://www.microsoft.com",
            "https://www.apple.com",
        };
        
        public event Action<ServiceState> OnConnectionStateChange;

        public ServiceState ServiceState { get; private set; }
       
        public object InitResult { get; private set; }

        bool IInternetService.IsConnected => throw new NotImplementedException();

        public bool IsRunning => throw new NotImplementedException();

        public InternetService()
        {
            m_isConnected = false;
            ServiceState = ServiceState.Created;
        }

        public async Task InitializeAsync()
        {
            
            m_client = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(m_timeOut)
            };
            
            foreach (string site in m_sitesList)
            {
                m_sites.Add(site, false);
            }
            
            bool isConnected = await CheckAllSitesAsync();
            SwitchState(isConnected);
            
            _ = StartCheckingConnectionAsync();

            ServiceState = ServiceState.Started;
        }

        public bool IsConnected()
        {
            return m_isConnected;
        }

        public bool HasInternetReachability()
        {
            return Application.internetReachability != NetworkReachability.NotReachable;
        }

        private async Task StartCheckingConnectionAsync()
        {
            while (true)
            {
                await CheckSingleSiteAsync(m_currentIndex);
                IncrementSiteIndex();

                await Task.Delay(m_timeOut * 1000);
            }
        }

        private void IncrementSiteIndex()
        {
            m_currentIndex = (m_currentIndex + 1) % m_sitesList.Length;
        }

        private async Task<bool> CheckAllSitesAsync()
        {
            if (!HasInternetReachability())
            {
                return false;
            }

            try
            {
                List<Task> tasks = new List<Task>();

                foreach (string site in m_sites.Keys)
                {
                    tasks.Add(UpdateSiteStatusAsync(site));
                }

                await Task.WhenAll(tasks);

                bool isConnected = m_sites.ContainsValue(true);
                ServiceState = isConnected ? ServiceState.Running : ServiceState.Down;


                return isConnected;
            }
            catch (Exception e)
            {
                Debug.LogError($"CheckAllSitesAsync encountered an error: {e.Message}");
                return false;
            }
        }

        private async Task CheckSingleSiteAsync(int index)
        {
            if (!HasInternetReachability())
            {
                SwitchState(false);
                return;
            }

            try
            {
                string webAddress = m_sitesList[index];
                bool isConnected = await CheckConnectionAsync(m_client, webAddress);
                
                m_sites[webAddress] = isConnected;

                if (isConnected)
                {
                    SwitchState(true);
                    m_timeOut = c_timeOutSlow;
                    ServiceState = ServiceState.Running;
                }
                else
                {
                    m_timeOut = c_timeOutFast;
                    
                    if (!m_sites.ContainsValue(true))
                    {
                        SwitchState(false);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"CheckSingleSiteAsync encountered an error: {e.Message}");
            }
        }

        private async Task UpdateSiteStatusAsync(string url)
        {
            m_sites[url] = await CheckConnectionAsync(m_client, url);
        }

        private async Task<bool> CheckConnectionAsync(HttpClient client, string url)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                //Debug.Log($"Connection check: {url}-{response.IsSuccessStatusCode}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error connecting to {url}: {e.Message}");
                return false;
            }
        }

        private void SwitchState(bool hasConnection)
        {
            if (hasConnection != m_isConnected)
            {
                OnConnectionStateChange?.Invoke(hasConnection ? ServiceState.Running : ServiceState.Down);
                m_isConnected = hasConnection;
                Debug.Log($"Internet Connected = {hasConnection}");
            }
        }

        public Task Initialize()
        {
            throw new NotImplementedException();
        }

        public void Shutdown()
        {
            throw new NotImplementedException();
        }
    }
}
