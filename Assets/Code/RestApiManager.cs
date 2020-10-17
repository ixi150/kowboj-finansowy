using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

public class RestApiManager : MonoBehaviour
{
    [SerializeField] string pesel = "22020274645";

    private const string token = "Bearer aaf5721a-1a51-3e57-a9ec-0a0df352f4f9";
    private const string apiURL = "https://developer.banking.asseco.pl/api/";
    
    public async void SendTest1()
    {
        await SendRequest("cb/party-data-management/v1/customers/persons");
    }

    public async Task GetAllPersons()
    {
        await SendRequest("cb/party-data-management/v1/customers/persons");
    }

    public async Task GetPerson()
    {
        await SendRequest("cb/party-data-management/v1/customers/persons?pesel:"+pesel);
    }

    async Task SendRequest(string url)
    {
        var client = GetClient();
        HttpResponseMessage response = await client.GetAsync(apiURL + url);
        await Response(response.Content);
    }

    async Task Response(HttpContent responseContent)
    {
        var stream = responseContent.ReadAsStreamAsync();
        using (var reader = new StreamReader(await stream))
        {
            Debug.Log(await reader.ReadToEndAsync());
        }
    }

    static HttpClient GetClient()
    {
        var client = new HttpClient();
        client.DefaultRequestHeaders.Add("accept", "*/*");
        client.DefaultRequestHeaders.Add("Authorization", token);
        return client;
    }
}
