using Newtonsoft.Json;
using Processes.Proteus.Rest.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


namespace HoloFlows.Processes
{
    //TODO some validation of the input data would be nice, to prevent uncaught exceptions
    /*
     * https://github.com/IoTUDresden/proteus
     * http://localhost:8082/api/#/
     */
    /// <summary>
    /// <para>REST client for communication with PROtEUS</para>
    /// <para>Standard ctor uses <see cref="Settings.PROTEUS_BASE_URI"/> as base uri.</para>
    /// </summary>
    public class ProteusRestClient
    {
        private const string JSON_CONTENT = "application/json";
        private const string CONTENT_TYPE = "Content-Type";

        // base uri should contain the / as last char
        private readonly string baseUri;


        public ProteusRestClient() : this(Settings.PROTEUS_BASE_URI) { }

        public ProteusRestClient(string baseUri)
        {
            this.baseUri = baseUri;
            if (!this.baseUri.EndsWith("/")) this.baseUri += "/";
            this.baseUri += "rest/";
        }



        #region process instance methods

        /// <summary>
        /// List all available process instances
        /// </summary>
        public IEnumerator GetProcessInstances(Action<RequestCompleteData<List<IProcessInstanceInfo>>> onComplete)
        {
            var request = UnityWebRequest.Get(baseUri + "processinstances/");
            var result = new RequestCompleteData<List<IProcessInstanceInfo>>();
            yield return request.SendWebRequest();

            HandleResponse(onComplete, request, result);
        }

        /// <summary>
        /// Get the Process Step Instance. Only available for the root processes.
        /// </summary>
        /// <param name="instanceId"></param>
        public IEnumerator GetProcessInstance(string instanceId, Action<RequestCompleteData<IJSONProcessStepInstance>> onComplete)
        {
            var request = UnityWebRequest.Get(baseUri + "processinstances/" + instanceId);
            var result = new RequestCompleteData<IJSONProcessStepInstance>();
            yield return request.SendWebRequest();

            HandleResponse(onComplete, request, result);
        }

        /// <summary>
        /// Get the latest state message for the given process instance.
        /// </summary>
        /// <param name="instanceId"></param>
        public IEnumerator GetRecentStateForProcessInstance(string instanceId, Action<RequestCompleteData<IStateChangeMessage>> onComplete)
        {
            var request = UnityWebRequest.Get(baseUri + "processinstances/recentstate/" + instanceId);
            var result = new RequestCompleteData<IStateChangeMessage>();
            yield return request.SendWebRequest();

            HandleResponse(onComplete, request, result);
        }

        /// <summary>
        /// Start the given process instance. Nothing is returned
        /// </summary>
        /// <param name="instanceId"></param>
        public IEnumerator StartProcessInstance(string instanceId, Action<RequestCompleteData<string>> onComplete, Dictionary<string, IJSONTypeInstance> inputParameter = null)
        {
            string postData = inputParameter == null ? String.Empty : JsonConvert.SerializeObject(inputParameter, Formatting.Indented);
            var request = UnityWebRequest.Post(baseUri + "processinstances/" + instanceId, postData);
            request.SetRequestHeader(CONTENT_TYPE, JSON_CONTENT);
            var result = new RequestCompleteData<string>();
            yield return request.SendWebRequest();

            HandleResponse(onComplete, request, result);
        }

        #endregion


        #region process methods

        /// <summary>
        /// Deploys a process instance for the given process id. This instance can then be used for execution.
        /// ProcessInstanceId for the deployed instance is returned.
        /// </summary>
        /// <param name="processId"></param>
        public IEnumerator DeployProcessInstance(string processId, Action<RequestCompleteData<string>> onComplete)
        {
            var request = UnityWebRequest.Post(baseUri + "processes/" + processId, String.Empty);
            var result = new RequestCompleteData<string>();
            yield return request.SendWebRequest();

            HandleResponse(onComplete, request, result);
        }

        /// <summary>
        /// Lists all deployed process instances, which available for deploying a process instance.
        /// </summary>
        public IEnumerator GetProcesses(Action<RequestCompleteData<List<IProcessInfo>>> onComplete)
        {
            var request = UnityWebRequest.Get(baseUri + "processes/");
            var result = new RequestCompleteData<List<IProcessInfo>>();
            yield return request.SendWebRequest();

            HandleResponse(onComplete, request, result);
        }

        /// <summary>
        /// Uploads a process definition, which is saved to file on server. The process is then deployed to the engine and available for deploying as process instance.
        /// Returns the process id of the uploaded process as string
        /// </summary>
        public IEnumerator UploadAndDeployProcess(UploadAndDeployRequest request, Action<RequestCompleteData<string>> onComplete)
        {
            string data = request.ToJson();
            var internalRequest = UnityWebRequest.Put(baseUri + "processes/", data);
            internalRequest.SetRequestHeader(CONTENT_TYPE, JSON_CONTENT);
            var result = new RequestCompleteData<string>();
            yield return internalRequest.SendWebRequest();

            HandleResponse(onComplete, internalRequest, result);
        }


        #endregion

        private static void LogError(UnityWebRequest request)
        {
            Debug.LogErrorFormat("request failed '{0}'\n code: {1}\n cause: {2}", request.url, request.responseCode, request.error);
        }

        /// <summary>
        /// Handles the response and tries to convert from json string to the specified type.
        /// </summary>
        /// <typeparam name="T">The type you want to retrieve</typeparam>
        /// <param name="onComplete"></param>
        /// <param name="request"></param>
        /// <param name="result"></param>
        /// <param name="hasData">if true (default), we expect, there is some data received</param>
        private static void HandleResponse<T>(Action<RequestCompleteData<T>> onComplete, UnityWebRequest request, RequestCompleteData<T> result)
        {
            if (request.isNetworkError || request.isHttpError)
            {
                LogError(request);
                result.HasError = true;
            }
            else
            {
                try
                {

                    if (typeof(T) == typeof(string))
                    {
                        result.Data = (T)Convert.ChangeType(request.downloadHandler.text, typeof(T));
                    }
                    else
                    {
                        T obj = JsonConvert.DeserializeObject<T>(request.downloadHandler.text);
                        result.Data = obj;
                    }


                    result.StatusCode = request.responseCode;
                }
                catch (Exception e)
                {
                    Debug.LogErrorFormat("Failed to deserialize request: {0}\n\n {1}", e.Message, e.InnerException);
                    result.HasError = true;
                }
            }

            onComplete(result);

        }

    }
}
