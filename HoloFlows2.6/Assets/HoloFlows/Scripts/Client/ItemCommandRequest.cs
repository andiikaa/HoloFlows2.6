using System;
using UnityEngine.Networking;

namespace HoloFlows.Client
{
    class ItemCommandRequest : PostRequestBase
    {
        private readonly Action<bool> requestDone;
        private const string PLAIN_CONTENT_TYPE = "text/plain";
        private const string URI_TARGET = "rest/items/{0}";


        public ItemCommandRequest(string requestURL, string itemId, Action<bool> requestDone = null)
            : base(requestURL, string.Format(URI_TARGET, itemId), PLAIN_CONTENT_TYPE)
        {
            this.requestDone = requestDone;
        }

        protected override void HandleResponse(UnityWebRequest www)
        {
            requestDone?.Invoke(true);
        }
    }
}
