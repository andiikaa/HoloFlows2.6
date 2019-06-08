using Processes.Proteus.Rest.Model;

namespace HoloFlows.Processes
{
    public class HumanTaskResponseUtil
    {

        private HumanTaskResponseUtil() { }

        /// <summary>
        /// Creates a basic response according to request. E.g. fills the ports with the 
        /// with the values from the request, sets the humantask instance id, ...
        /// </summary>
        public static IHumanTaskResponse CreateResponseFromRequest(IHumanTaskRequest request)
        {
            IHumanTaskResponse response = new IHumanTaskResponse();
            response.HumanTaskInstanceId = request.HumanTaskInstanceId;
            response.EndDataPorts = request.EndDataPorts;
            response.StartDataPorts = request.StartDataPorts;
            response.Description = request.Description;
            response.HumanTaskType = request.HumanTaskType;
            response.HumanTaskUseCase = request.HumanTaskUseCase;
            response.EndControlPorts = request.EndControlPorts;
            response.Name = (request.Name);
            return response;
        }
    }
}
