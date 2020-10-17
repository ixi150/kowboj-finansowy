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
    [SerializeField] string user_id = "0";
    [SerializeField] string document_id = "0";

    private const string token = "Bearer aaf5721a-1a51-3e57-a9ec-0a0df352f4f9";
    private const string apiURL = "https://developer.banking.asseco.pl/api/";
    
    public async void SendTest1()
    {
        await SendRequest("cb/payment-execution/v1/documents");
    }

    public async void GetAllPaymentExecutions()
    {
        await SendRequest("cb/payment-execution/v1/documents");
    }

    public async void GetPaymentDocument()
    {
        await SendRequest("cb/payment-execution/v1/documents/"+ document_id);
    }

    public async void GetAllPersons()
    {
        await SendRequest("cb/party-data-management/v1/customers/persons");
    }

    public async void GetPerson()
    {
        await SendRequest("cb/party-data-management/v1/customers/persons?pesel="+pesel);
    }

    async Task SendRequest(string url, params string[] headers)
    {
        var client = GetClient();
        if (headers != null)
        {
            foreach (var header in headers)
            {
                var split = header.Split(':');
                client.DefaultRequestHeaders.Add(split[0], split[1]);
            }
        }

        HttpResponseMessage response = await client.GetAsync(apiURL + url);
        Response(await response.Content.ReadAsStreamAsync());
    }

    void Response(Stream stream)
    {
        using (var reader = new StreamReader(stream))
        {
            var json = reader.ReadToEnd();
            JsonFormatter formatter = new JsonFormatter(json);
            Debug.Log(formatter.Format());
        }
    }

    static HttpClient GetClient()
    {
        var client = new HttpClient();
        client.DefaultRequestHeaders.Add("accept", "*/*");
        client.DefaultRequestHeaders.Add("Authorization", token);
        //client.DefaultRequestHeaders.Add("Content-Type", "application/json");
        //client.DefaultRequestHeaders.Add("X-Request-ID", "f182351c-87b8-4118-9360-24d28aaf3212");
        return client;
    }
}
