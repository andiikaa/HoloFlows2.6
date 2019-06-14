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
        private RectTransform rectTransform;

        private Style style;
        private bool isSelected;
        //scale if a color is not selected
        private float currentDefaultScale = 0.9f;
        //offset if a color is selected
        private float scaleOffset = 0.3f;

        public override void ButtonStart()
        {
            BtnSpriteRenderer = transform.Find("BorderSprite").GetComponent<SpriteRenderer>();
            rectTransform = gameObject.GetComponent<RectTransform>();
            currentDefaultScale = rectTransform.localScale.x;
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
            {
                BtnSpriteRenderer.color = style.highlightColor;
                var tmpScale = currentDefaultScale + scaleOffset;
                rectTransform.localScale = new Vector3(tmpScale, tmpScale);
            }

        }

        public void OnFocusExit()
        {
            rectTransform.localScale = new Vector3(currentDefaultScale, currentDefaultScale);

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
