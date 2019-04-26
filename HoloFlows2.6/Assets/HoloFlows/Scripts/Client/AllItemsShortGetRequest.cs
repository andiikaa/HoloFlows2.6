using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace HoloFlows.Client
{

    public class AllItemsShortGetRequest
    {
        private const string URI_TARGET = "rest/items?recursive=false&fields=name%2C%20state";
        private const string HTTP = "http://";
        private readonly string requestURL;
        private List<ItemDataShort> responses;

        private readonly Action<List<ItemDataShort>> responseReadyAction;

        public AllItemsShortGetRequest(string requestURL, Action<List<ItemDataShort>> responseReadyAction = null)
        {
            this.requestURL = requestURL.StartsWith(HTTP) ? requestURL : HTTP + requestURL;
            if (!this.requestURL.EndsWith("/")) { this.requestURL = this.requestURL + "/"; }
            this.requestURL = this.requestURL + URI_TARGET;
            this.responseReadyAction = responseReadyAction;
        }

        public IEnumerator ExecuteRequest()
        {
            responses = new List<ItemDataShort>();
            UnityWebRequest www = UnityWebRequest.Get(requestURL);
            yield return www.SendWebRequest();

            while (!www.isDone)
            {
                Debug.LogError(".");
                yield return null;
            }

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log("AllItemsGetRequest: " + requestURL + " " + www.error);
            }
            else
            {
                HandleResponse(www);
            }
        }

        private void HandleResponse(UnityWebRequest www)
        {
            string temp = www.downloadHandler.text;
            try
            {
                responses = JsonConvert.DeserializeObject<List<ItemDataShort>>(temp);
            }
            catch (JsonException ex)
            {
                Debug.LogErrorFormat("failed to deserialize allitems response", ex.Message);
                return;
            }

            responseReadyAction?.Invoke(responses);

        }
    }
}
