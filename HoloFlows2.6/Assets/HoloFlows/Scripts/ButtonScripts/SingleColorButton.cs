using HoloFlows.Client;
using HoloToolkit.Unity.InputModule;
using UnityEngine;

namespace HoloFlows.ButtonScripts
{

    public class SingleColorButton : TapSoundButton, IFocusable
    {
        /// <summary>
        /// DeviceId e.g. the OpenHab ItemName
        /// </summary>
        public string DeviceId { get; set; }

        /// <summary>
        /// The real command name, which should be send to the middleware. 
        /// E.g. "1" for on and "0" for off.
        /// </summary>
        public string RealCommandName { get; set; }

        private SpriteRenderer BtnSpriteRenderer;

        private Style style;
        private bool isSelected;

        // Use this for initialization
        public override void Start()
        {
            base.Start();
            BtnSpriteRenderer = transform.Find("BorderSprite").GetComponent<SpriteRenderer>();
            style = GameObject.Find("GlobalStyle").GetComponent<Style>();
        }


        /// <summary>
        /// Checks if the given values for this button are valid.
        /// If not, the 
        /// </summary>
        public bool HasMissingValues
        {
            get
            {
                return string.IsNullOrEmpty(DeviceId)
                    || string.IsNullOrEmpty(RealCommandName);
            }
        }

        public override void HandleClickEvent(InputClickedEventData eventData)
        {
            if (HasMissingValues)
            {
                Debug.LogError("Command Data is missing!!");
            }
            else
            {
                Debug.LogFormat("Sending Command '{0}' to device {1}", RealCommandName, DeviceId);
                var request = new ItemCommandRequest(Settings.OPENHAB_URI, DeviceId);
                StartCoroutine(request.ExecuteRequestOnly(RealCommandName));
            }
        }

        #region IFocusable

        public void OnFocusEnter()
        {
            if (BtnSpriteRenderer != null)
                BtnSpriteRenderer.color = style.highlightColor;

        }

        public void OnFocusExit()
        {
            if (!isSelected)
            {
                if (BtnSpriteRenderer != null)
                    BtnSpriteRenderer.color = style.defaultColor;
            }
        }

        public void Select()
        {
            if (BtnSpriteRenderer != null)
                BtnSpriteRenderer.color = style.ClickedOnColor;

            isSelected = true;
        }

        public void Deselect()
        {

            if (BtnSpriteRenderer != null)
                BtnSpriteRenderer.color = style.defaultColor;

            isSelected = false;
        }

        #endregion

    }
}
