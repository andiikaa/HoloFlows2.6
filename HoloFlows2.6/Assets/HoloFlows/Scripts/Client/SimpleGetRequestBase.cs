using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace HoloFlows.Client
{
    /// <summary>
    /// A simple wrapper for web requests
    /// </summary>
    public abstract class SimpleGetRequestBase
    {
        private const string HTTP = "http://";
        private readonly string requestURL;

        public SimpleGetRequestBase(string requestURL, string uriTarget)
        {
            this.requestURL = requestURL.StartsWith(HTTP) ? requestURL : HTTP + requestURL;
            if (!this.requestURL.EndsWith("/")) { this.requestURL = this.requestURL + "/"; }
            this.requestURL = this.requestURL + uriTarget;
        }

        public IEnumerator ExecuteRequest()
        {
            UnityWebRequest www = UnityWebRequest.Get(requestURL);
            yield return www.SendWebRequest();

            while (!www.isDone)
            {
                Debug.LogError(".");
                yield return null;
            }

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log("SimpleGetRequest: " + requestURL + " " + www.error);
            }
            else
            {
                HandleResponse(www);
            }
        }

        protected abstract void HandleResponse(UnityWebRequest www);
    }
}
