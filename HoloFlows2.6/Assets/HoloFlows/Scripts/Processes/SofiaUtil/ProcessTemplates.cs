using Processes.Sofia;

namespace HoloFlows.Processes.SofiaUtil
{
    /// <summary>
    /// This class contains some example processes, or process templates
    /// </summary>
    public class ProcessTemplates
    {
        /// <summary>
        /// Creates a SimpleOrProcess like the process in the java rest client package (SimpleOrTest.diagram)
        /// </summary>
        /// <returns></returns>
        public static Process CreateSimpleOr()
        {
            //root process
            var simpleOr = CreateProcess(null, "SimpleOrProcess");

            //middle processes and ors
            var processTop1 = CreateProcess(simpleOr, "ProcessTop1");
            var processTop2 = CreateProcess(simpleOr, "ProcessTop2");
            var processMiddle1 = CreateProcess(simpleOr, "ProcessMiddle1");
            var processMiddle2 = CreateProcess(simpleOr, "ProcessMiddle2");
            var processBottom1 = CreateProcess(simpleOr, "ProcessBottom1");
            var processBottom2 = CreateProcess(simpleOr, "ProcessBottom2");
            var orTop = CreateOr(simpleOr, "OrTop");
            var orMiddle = CreateOr(simpleOr, "OrMiddle");
            var orBottom = CreateOr(simpleOr, "OrBottom");

            //simple connection via root and the first 2 middle processes via control ports
            ConnectViaStartPorts(simpleOr, "start_0", processTop1, "start_0");
            ConnectViaStartPorts(simpleOr, "start_1", processTop2, "start_1");

            //create if
            var ifFirstCreaterThanSecond = CreateIf(simpleOr, "If");

            //create startdata ports for root and if 
            var ifDataLeft = CreateStartDataPort(ifFirstCreaterThanSecond, "start_0");
            var ifDataRight = CreateStartDataPort(ifFirstCreaterThanSecond, "start_1");
            var rootDataLeft = CreateStartDataPort(simpleOr, "start_2");
            var rootDataRight = CreateStartDataPort(simpleOr, "start_3");

            //Create some DataType
            var intLeft = CreateDataType(simpleOr, new IntegerType(), "intLeft");
            var intRight = CreateDataType(simpleOr, new IntegerType(), "intRight");

            //primitive values can be set on the ports (must match to the datatype attached to the port)
            rootDataLeft.value = "1";
            rootDataRight.value = "0";

            //assign the datatypes to the ports  
            SetDataTypeToPort(intLeft, ifDataLeft);
            SetDataTypeToPort(intLeft, rootDataLeft);
            SetDataTypeToPort(intRight, ifDataRight);
            SetDataTypeToPort(intRight, rootDataRight);

            //Set the condition to the if
            SetCondition(ifFirstCreaterThanSecond, ifDataLeft, ifDataRight, Comparator.GreaterThan);

            //connect the startdataports from root with the startdata ports from the if
            ConnectViaStartDataPorts(rootDataLeft, ifDataLeft);
            ConnectViaStartDataPorts(rootDataRight, ifDataRight);

            //connect the if step with the process steps
            ConnectViaEndToStartPorts(ifFirstCreaterThanSecond, "end_0", processMiddle1, "start_0", TransitionType.TRUE);
            ConnectViaEndToStartPorts(ifFirstCreaterThanSecond, "end_1", processMiddle2, "start_0", TransitionType.FALSE);
            ConnectViaEndToStartPorts(ifFirstCreaterThanSecond, "end_2", processBottom1, "start_0", TransitionType.FALSE);
            ConnectViaEndToStartPorts(ifFirstCreaterThanSecond, "end_3", processBottom2, "start_0", TransitionType.FALSE);

            //connect the middle processes with the ors
            ConnectViaEndToStartPorts(processTop1, "end_0", orTop, "start_0");
            ConnectViaEndToStartPorts(processTop2, "end_0", orTop, "start_1");
            ConnectViaEndToStartPorts(processMiddle1, "end_0", orMiddle, "start_0");
            ConnectViaEndToStartPorts(processMiddle2, "end_0", orMiddle, "start_1");
            ConnectViaEndToStartPorts(processBottom1, "end_0", orBottom, "start_0");
            ConnectViaEndToStartPorts(processBottom2, "end_0", orBottom, "start_1");

            //connect endports on the or with endports on the root process
            ConnectViaEndToEndPorts(orTop, "end_0", simpleOr, "end_0");
            ConnectViaEndToEndPorts(orMiddle, "end_0", simpleOr, "end_1");
            ConnectViaEndToEndPorts(orBottom, "end_0", simpleOr, "end_2");

            return simpleOr;
        }

        /// <summary>
        /// triggeredEvent ---> restInvoke
        /// 
        /// TriggeredEvent will wait, till the given event was received. 
        /// After that, the RESTInvoke is activated and will post the given postDataAsString to the given serverUri.
        /// </summary>
        public static Process CreateRestPostInvokeAfterTriggeredEvent(string name, string serverUri, string eplStmt, string postDataAsString)
        {
            //root process
            var root = CreateProcess(null, name);

            //both substeps
            var triggeredEvent = CreateTriggeredEvent(root, "WaitForTheEvent", eplStmt);
            var restInvoke = CreateRESTInvoke(root, "RestPostInvoke");
            restInvoke.HTTPMethod = HTTPverb.POST;
            restInvoke.ServerUri = serverUri;

            //connect root and triggeredEvent
            ConnectViaStartPorts(root, "start_0", triggeredEvent, "start_0");

            //create dataports and datatype
            var dataType = CreateDataType(root, new StringType(), "PostData");
            var restStartDataPort = CreateStartDataPort(restInvoke, "start_0");

            //assign datatype
            SetDataTypeToPort(dataType, restStartDataPort);

            //Optional ports dont necessarily need incoming transitions.
            //We model this port as optional and give it the POSTData.
            //The RESTInvoke will pick up the data and send it via post.
            restStartDataPort.optional = true;
            restStartDataPort.value = postDataAsString;

            //connect both substeps via control ports
            ConnectViaEndToStartPorts(triggeredEvent, "end_0", restInvoke, "start_1");

            return root;
        }
    }
}
