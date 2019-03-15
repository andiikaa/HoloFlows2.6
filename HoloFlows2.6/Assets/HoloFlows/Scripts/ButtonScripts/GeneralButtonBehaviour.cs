using UnityEngine;
using UnityEngine.UI;

namespace HoloToolkit.Unity.InputModule
{
    public class GeneralButtonBehaviour : MonoBehaviour, IFocusable
    {
        private Text BtnText;
        private TextMesh BtnTextMesh;
        private SpriteRenderer BtnSpriteRenderer;

        private Style style;
        private bool isFocused, isSelected;

        // Use this for initialization
        void Start()
        {
            BtnText = transform.GetComponentInChildren<Text>();
            BtnTextMesh = transform.GetComponentInChildren<TextMesh>();
            BtnSpriteRenderer = transform.GetComponentInChildren<SpriteRenderer>();
            style = GameObject.Find("GlobalStyle").GetComponent<Style>();
        }

        public void OnFocusEnter()
        {
            if (BtnText != null)
                BtnText.color = style.highlightColor;

            if (BtnSpriteRenderer != null)
                BtnSpriteRenderer.color = style.highlightColor;

            if (BtnTextMesh != null)
                BtnTextMesh.color = style.highlightColor;

            isFocused = true;
        }

        public void OnFocusExit()
        {
            if (!isSelected)
            {

                if (BtnText != null)
                    BtnText.color = style.defaultColor;

                if (BtnSpriteRenderer != null)
                    BtnSpriteRenderer.color = style.defaultColor;

                if (BtnTextMesh != null)
                    BtnTextMesh.color = style.defaultColor;

            }

            isFocused = false;
        }

        public void Select()
        {
            if (BtnText != null)
                BtnText.color = style.ClickedOnColor;

            if (BtnSpriteRenderer != null)
                BtnSpriteRenderer.color = style.ClickedOnColor;

            if (BtnTextMesh != null)
                BtnTextMesh.color = style.ClickedOnColor;

            isSelected = true;
        }

        public void Deselect()
        {
            if (BtnText != null)
                BtnText.color = style.defaultColor;

            if (BtnSpriteRenderer != null)
                BtnSpriteRenderer.color = style.defaultColor;

            if (BtnTextMesh != null)
                BtnTextMesh.color = style.defaultColor;

            isSelected = false;
        }
    }
}
