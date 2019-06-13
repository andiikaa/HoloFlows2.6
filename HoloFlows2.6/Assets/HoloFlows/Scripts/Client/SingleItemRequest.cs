using HoloFlows.Model;
using Newtonsoft.Json;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace HoloFlows.Client
{
    public class SingleItemRequest : SimpleGetRequestBase
    {
        private const string URI_TARGET = "rest/items/";
        //item, is 404
        private readonly Action<SimpleItem, bool> responseReadyAction;


        public SingleItemRequest(string requestURL, string itemName, Action<SimpleItem, bool> responseReadyAction = null) : base(requestURL, URI_TARGET + itemName, true)
        {
            this.responseReadyAction = responseReadyAction;
        }

        protected override void HandleResponse(UnityWebRequest www)
        {
            if (www.responseCode == 404)
            {
                responseReadyAction?.Invoke(null, true);
                return;
            }
            string temp = www.downloadHandler.text;
            SimpleItem response = null;
            try
            {
                response = JsonConvert.DeserializeObject<SimpleItem>(temp);
            }
            catch (JsonException ex)
            {
                Debug.LogErrorFormat("failed to deserialize SingleItemRequest response", ex.Message);
                responseReadyAction?.Invoke(null, false);
                return;
            }

            responseReadyAction?.Invoke(response, false);

        }
    }
}
