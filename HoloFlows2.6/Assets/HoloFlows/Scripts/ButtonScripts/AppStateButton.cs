using HoloFlows.Manager;
using HoloToolkit.Unity.InputModule;
using UnityEngine;
using UnityEngine.UI;

namespace HoloFlows.ButtonScripts
{
    public class AppStateButton : TapSoundButton, IFocusable
    {
        private Text BtnText;
        private TextMesh BtnTextMesh;
        private SpriteRenderer BtnSpriteRenderer;
        private Style style;
        private bool isVisualyDisabled = false;
        private bool isCurrentState = false;

        public AppStateButtonType ButtonType { get; set; }

        public override void ButtonStart()
        {
            BtnText = transform.GetComponentInChildren<Text>();
            BtnTextMesh = transform.GetComponentInChildren<TextMesh>();
            BtnSpriteRenderer = transform.GetComponentInChildren<SpriteRenderer>();
            style = GameObject.Find("GlobalStyle").GetComponent<Style>();
        }

        public override void HandleClickEvent(InputClickedEventData eventData)
        {
            if (isVisualyDisabled) return;

            switch (ButtonType)
            {
                case AppStateButtonType.CONTROL:
                    HoloFlowSceneManager.Instance.SwitchToControl();
                    break;
                case AppStateButtonType.EDIT:
                    HoloFlowSceneManager.Instance.SwitchToEdit();
                    break;
                case AppStateButtonType.SCAN:
                    HoloFlowSceneManager.Instance.SwitchToQRScan();
                    break;
                default:
                    Debug.LogErrorFormat("cant handle app state button of type '{}'", ButtonType);
                    break;
            }
        }

        public void VisualDisable()
        {
            isVisualyDisabled = true;
            //dirty fixes np
            if (style == null) Start();
            ChangeButtonColor(style.InactiveButtonColor);
        }

        public void VisualEnable()
        {
            isVisualyDisabled = false;
            //dirty fixes np
            if (style == null) Start();
            ChangeButtonColor(style.defaultColor);
        }

        public void MarkAsCurrentState()
        {
            isCurrentState = true;
            ChangeButtonColor(style.ActiveStateColor);
        }

        public void UnmarkAsCurrentState()
        {
            isCurrentState = false;
            if (isVisualyDisabled) { ChangeButtonColor(style.InactiveButtonColor); }
            else { ChangeButtonColor(style.defaultColor); }
        }

        #region IFocusable
        public void OnFocusEnter()
        {
            if (isVisualyDisabled)
            {
                return;
            }
            ChangeButtonColor(style.highlightColor);
        }

        public void OnFocusExit()
        {
            if (isVisualyDisabled)
            {
                if (isCurrentState) { ChangeButtonColor(style.ActiveStateColor); }
                else { ChangeButtonColor(style.InactiveButtonColor); }
            }
            else
            {
                if (isCurrentState) { ChangeButtonColor(style.ActiveStateColor); }
                else { ChangeButtonColor(style.defaultColor); }
            }
        }
        #endregion

        private void ChangeButtonColor(Color color)
        {
            if (BtnText != null)
                BtnText.color = color;

            if (BtnSpriteRenderer != null)
                BtnSpriteRenderer.color = color;

            if (BtnTextMesh != null)
                BtnTextMesh.color = color;
        }
    }
}
