using System;
using UnityEngine.Networking;

namespace HoloFlows.Client
{
    public class UpdateWorldPositionRequest : PostRequestBase
    {
        private const string URI_TARGET = "rest/semantic/{0}/worldposition";
        private const string JSON_CONTENT_TYPE = "application/json";

        private readonly Action<bool> requestFinished;

        public UpdateWorldPositionRequest(string requestURL, string thingUid, Action<bool> requestFinished = null)
            : base(requestURL, string.Format(URI_TARGET, thingUid), JSON_CONTENT_TYPE)
        {
            this.requestFinished = requestFinished;
        }

        protected override void HandleResponse(UnityWebRequest www)
        {
            requestFinished?.Invoke(true);
        }
    }
}
