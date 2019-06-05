using Processes.Sofia;
using System;
using System.Collections.Generic;

namespace HoloFlows.Processes.SofiaUtil
{
    /// <summary>
    /// This class helps to generate process models.
    /// </summary>
    public class ProcessCreator
    {
        //no instance needed
        private ProcessCreator() { }


        /// <summary>
        /// Connect two process steps via StartControlPorts.
        /// StartControlPorts are create on both, the source and target. Also transitions are created.
        /// Example scenario: RootProcess ---> Subprocess
        /// </summary>
        /// <param name="source">Source Process</param>
        /// <param name="sourcePortName">Port name for the created StartControlPort on the source</param>
        /// <param name="target">Target process</param>
        /// <param name="targetPortName">Port name for the created StartControlPort on the target</param>
        public static void ConnectViaStartPorts(ProcessStep source, string sourcePortName, ProcessStep target, string targetPortName)
        {
            var transition = CreateTransition();
            var sourcePort = CreateStartControlPort(source, sourcePortName);
            var targetPort = CreateStartControlPort(target, targetPortName);
            transition.sourcePort = sourcePort;
            transition.targetPort = targetPort;
            sourcePort.outTransitions.Add(transition);
            targetPort.inTransitions = transition;
        }

        /// <summary>
        /// Connect to process steps via endport --> startport
        /// </summary>
        /// <param name="source"></param>
        /// <param name="sourcePortName"></param>
        /// <param name="target"></param>
        /// <param name="targetPortName"></param>
        /// <param name="transitionType">Defines the <see cref="TransitionType"/>. E.g. <see cref="If"/> uses <see cref="FalseTransition"/> and <see cref="TrueTransition"/></param>
        public static void ConnectViaEndToStartPorts(ProcessStep source, string sourcePortName, ProcessStep target, string targetPortName, TransitionType transitionType = TransitionType.NORMAL)
        {
            Transition transition = CreateTransition(transitionType);
            var sourcePort = CreateEndControlPort(source, sourcePortName);
            var targetPort = CreateStartControlPort(target, targetPortName);
            transition.sourcePort = sourcePort;
            transition.targetPort = targetPort;
            sourcePort.outTransitions.Add(transition);
            targetPort.inTransitions = transition;
        }

        /// <summary>
        /// Connect to processes via endport ---> endport.
        /// Note the target is a composite step. 
        /// Scenario: from subprocess --> parentstep
        /// </summary>
        /// <param name="source"></param>
        /// <param name="sourcePortName"></param>
        /// <param name="target"></param>
        /// <param name="targetPortName"></param>
        public static void ConnectViaEndToEndPorts(ProcessStep source, string sourcePortName, CompositeStep target, string targetPortName)
        {
            Transition transition = CreateTransition();
            var sourcePort = CreateEndControlPort(source, sourcePortName);
            var targetPort = CreateEndControlPort(target, targetPortName);
            transition.sourcePort = sourcePort;
            transition.targetPort = targetPort;
            sourcePort.outTransitions.Add(transition);
            targetPort.inTransitions = transition;
        }

        /// <summary>
        /// source --> target
        /// Scenario: parentstep ---> subStep
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void ConnectViaStartDataPorts(StartDataPort source, StartDataPort target)
        {
            var t = CreateTransition();
            t.sourcePort = source;
            t.targetPort = target;
            source.outTransitions.Add(t);
            target.inTransitions = t;
        }

        /// <summary>
        /// source --> target
        /// Scenario: substep --> substep
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void ConnectViaEndDataToStartDataPorts(EndDataPort source, StartDataPort target)
        {
            var t = CreateTransition();
            t.sourcePort = source;
            t.targetPort = target;
            source.outTransitions.Add(t);
            target.inTransitions = t;
        }

        /// <summary>
        /// Set the <see cref="DataType"/> to a port.
        /// The given <see cref="DataPort"/> is also added to the <see cref="DataType.portMembers"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="port"></param>
        public static void SetDataTypeToPort(DataType type, DataPort port)
        {
            port.portDatatype = type;
            if (!type.portMembers.Contains(port))
                type.portMembers.Add(port);
        }

        /// <summary>
        /// Create a <see cref="DataType"/> and add to the root process.
        /// Note: All DataTypes within the process model, must be added to <see cref="Process.dataTypeDefinitions"/> on the ROOT process.
        /// This method does this.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="root"></param>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T CreateDataType<T>(Process root, T type, string name) where T : DataType
        {
            if (root == null || root.parentstep != null)
                throw new ArgumentException("DataType must be attached to the root process. Root process cant be null or cant have a parent.");
            if (root.dataTypeDefinitions == null)
                root.dataTypeDefinitions = new List<DataType>();
            if (!root.dataTypeDefinitions.Contains(type))
                root.dataTypeDefinitions.Add(type);

            InitDataType(type, name);
            return type;
        }

        private static void InitDataType(DataType type, string name)
        {
            type.id = Guid.NewGuid().ToString();
            type.portMembers = new List<DataPort>();
            type.name = name;
        }

        /// <summary>
        /// Dont forget to add a <see cref="DataType"/>
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="portName"></param>
        /// <returns></returns>
        public static StartDataPort CreateStartDataPort(ProcessStep parent, string portName)
        {
            var p = new StartDataPort();
            InitPortBasic(p, parent, portName);
            return p;
        }

        public static EndDataPort CreateEndDataPort(ProcessStep parent, string portName)
        {
            var p = new EndDataPort();
            InitPortBasic(p, parent, portName);
            return p;
        }

        private static StartControlPort CreateStartControlPort(ProcessStep parent, string portName)
        {
            var p = new StartControlPort();
            InitPortBasic(p, parent, portName);
            return p;
        }

        private static EndControlPort CreateEndControlPort(ProcessStep parent, string portName)
        {
            var p = new EndControlPort();
            InitPortBasic(p, parent, portName);
            return p;
        }

        private static Transition CreateTransition(TransitionType transitionType = TransitionType.NORMAL)
        {
            Transition t = null;
            switch (transitionType)
            {
                case TransitionType.FALSE:
                    t = new FalseTransition();
                    break;
                case TransitionType.TRUE:
                    t = new TrueTransition();
                    break;
                default:
                    t = new Transition();
                    break;
            }

            t.id = Guid.NewGuid().ToString();
            t.name = "Transition";
            return t;
        }

        /// <summary>
        /// Creates a <see cref="Process"/> and add basic info (e.g. id)
        /// </summary>
        /// <param name="parent">can be null, in case the process is the root</param>
        /// <param name="processName"></param>
        /// <returns></returns>
        public static Process CreateProcess(CompositeStep parent, string processName)
        {
            var p = new Process();
            InitCompositeStep(p);
            InitProcessStep(p, processName, parent);
            return p;
        }

        /// <summary>
        /// Create <see cref="Or"/>
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="orName"></param>
        /// <returns></returns>
        public static Or CreateOr(CompositeStep parent, string orName)
        {
            var o = new Or();
            InitProcessStep(o, orName, parent);
            return o;
        }

        /// <summary>
        /// Create <see cref="If"/>
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="ifName"></param>
        /// <returns></returns>
        public static If CreateIf(CompositeStep parent, string ifName)
        {
            var i = new If();
            InitProcessStep(i, ifName, parent);
            return i;
        }

        /// <summary>
        /// Triggered Event needs the <see cref="TriggeredEvent.EPLStatement"/>
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="triggeredEventName"></param>
        /// <returns></returns>
        public static TriggeredEvent CreateTriggeredEvent(CompositeStep parent, string triggeredEventName, string eplStmt)
        {
            var triggeredEvent = new TriggeredEvent();
            InitProcessStep(triggeredEvent, triggeredEventName, parent);
            triggeredEvent.EPLStatement = eplStmt;
            return triggeredEvent;
        }

        /// <summary>
        /// In case this is a post request, request body must be on a startdata port.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static RESTInvoke CreateRESTInvoke(CompositeStep parent, string name)
        {
            var invoke = new RESTInvoke();
            InitProcessStep(invoke, name, parent);
            return invoke;
        }

        /// <summary>
        /// Set a <see cref="Condition"/> to the if step.
        /// </summary>
        /// <param name="step"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="ifComparator"></param>
        public static void SetCondition(If step, StartDataPort left, StartDataPort right, Comparator ifComparator)
        {
            var c = new Condition
            {
                comparator = ifComparator,
                leftSide = left,
                rightSide = right
            };
            step.ifCondition = c;

        }

        private static void InitPortBasic(Port port, ProcessStep parent, string portName)
        {
            port.name = portName;
            port.processStep = parent;
            port.outTransitions = new List<Transition>();
            port.id = Guid.NewGuid().ToString();

            if (parent != null && !parent.ports.Contains(port))
            {
                parent.ports.Add(port);
            }
        }

        private static void InitProcessStep(ProcessStep step, string name, CompositeStep parent = null)
        {
            step.parentstep = parent;
            step.name = name;
            step.ports = new List<Port>();
            step.id = Guid.NewGuid().ToString();
            if (parent != null && !parent.subSteps.Contains(step))
                parent.subSteps.Add(step);
        }

        private static void InitCompositeStep(CompositeStep step)
        {
            step.subSteps = new List<ProcessStep>();
        }

        public enum TransitionType
        {
            NORMAL, TRUE, FALSE
        }
    }
}
