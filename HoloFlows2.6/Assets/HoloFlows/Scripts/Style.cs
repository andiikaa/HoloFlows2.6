using HoloToolkit.Unity;
using UnityEngine;

namespace HoloFlows
{
    public class Style : Singleton<Style>
    {

        public Material highlightMat;
        public Color highlightColor;
        public Color defaultColor;
        public Material ClickedOnMat;
        public Color ClickedOnColor;

        public Material LineMaterialWFMode;
        public Material LineMaterialNotWFMode;
        public Material ArrowMaterialWFMode;
        public Material ArrowMaterialNotWFMode;

        public Material AndWFMode;
        public Material OrWFMode;

        public Material AndChosen;
        public Material OrChosen;

        public Material AndNotWFMode;
        public Material OrNotWfMode;

        public Material SimpleBlue;
        public Material Invisible;


        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}