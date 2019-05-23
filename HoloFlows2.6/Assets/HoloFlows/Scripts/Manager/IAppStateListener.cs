namespace HoloFlows.Manager
{
    public interface IAppStateListener
    {
        /// <summary>
        /// Called if the app state has changed.
        /// </summary>
        /// <param name="appState">the new app state</param>
        void AppStateChanged(ApplicationState appState);
    }
}
