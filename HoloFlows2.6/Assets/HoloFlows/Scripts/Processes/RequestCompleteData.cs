namespace HoloFlows.Processes
{
    public class RequestCompleteData<T>
    {
        public T Data;
        public bool HasError = false;
        public long StatusCode = 0;
    }
}
