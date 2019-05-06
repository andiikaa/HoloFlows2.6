namespace HoloFlows.Manager
{
    /// <summary>
    /// A managed object is managed by the <see cref="HoloFlowSceneManager"/>
    /// </summary>
    public interface IManagedObject
    {
        /// <summary>
        /// Hide the object in the ui
        /// </summary>
        void Hide();

        /// <summary>
        /// Show the object in the ui
        /// </summary>
        void Show();

        /// <summary>
        /// enable placing for this selected item
        /// </summary>
        /// <param name="enable">true for enabling, false for disabling</param>
        void EnablePlacingBox(bool enable);
    }
}
