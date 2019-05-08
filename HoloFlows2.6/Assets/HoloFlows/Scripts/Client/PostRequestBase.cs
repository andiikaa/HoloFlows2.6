using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace HoloFlows.Client
{
    public abstract class PostRequestBase
    {
        private const string HTTP = "http://";
        private readonly string requestURL;
        private readonly string contentType;

        public PostRequestBase(string requestURL, string uriTarget, string contentType)
        {
            this.requestURL = requestURL.StartsWith(HTTP) ? requestURL : HTTP + requestURL;
            if (!this.requestURL.EndsWith("/")) { this.requestURL = this.requestURL + "/"; }
            this.requestURL = this.requestURL + uriTarget;
            this.contentType = contentType;
        }

        public IEnumerator ExecuteRequest(string postData)
        {
            return InternalExecute(postData, true);
        }

        /// <summary>
        /// Without callback
        /// </summary>
        public IEnumerator ExecuteRequestOnly(string postData)
        {
            return InternalExecute(postData);
        }

        private IEnumerator InternalExecute(string postData, bool handleResponse = false)
        {
            UnityWebRequest www = UnityWebRequest.Post(requestURL, postData);
            www.SetRequestHeader("Content-Type", contentType);
            yield return www.SendWebRequest();

            while (!www.isDone)
            {
                Debug.LogError(".");
                yield return null;
            }

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log("PostRequest: " + requestURL + " " + www.error);
            }
            else
            {
                if (handleResponse) { HandleResponse(www); }
            }
        }

        protected abstract void HandleResponse(UnityWebRequest www);
    }
}
