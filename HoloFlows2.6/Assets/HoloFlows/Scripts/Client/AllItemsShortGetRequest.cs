using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace HoloFlows.Client
{
    /// <summary>
    /// Gets (only) the item states.
    /// </summary>
    public class AllItemsShortGetRequest : SimpleGetRequestBase
    {
        private const string URI_TARGET = "rest/items?recursive=false&fields=name%2C%20state";
        private List<ItemDataShort> responses;

        private readonly Action<List<ItemDataShort>> responseReadyAction;

        public AllItemsShortGetRequest(string requestURL, Action<List<ItemDataShort>> responseReadyAction = null) : base(requestURL, URI_TARGET)
        {
            this.responseReadyAction = responseReadyAction;
            responses = new List<ItemDataShort>();
        }

        protected override void HandleResponse(UnityWebRequest www)
        {
            responses = new List<ItemDataShort>();
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
