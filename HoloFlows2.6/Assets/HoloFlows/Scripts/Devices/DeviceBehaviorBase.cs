using HoloFlows.ButtonScripts;
using HoloFlows.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace HoloFlows.Devices
{
    public abstract class DeviceBehaviorBase : MonoBehaviour, ManagedObject
    {
        protected enum DeviceType { BASIC, TWO_WAY, THREE_WAY, MULTI }

        public bool IsBasicDevice { get { return GetDeviceType() == DeviceType.BASIC; } }
        public bool IsTwoWayDevice { get { return GetDeviceType() == DeviceType.TWO_WAY; } }
        public bool IsThreeWayDevice { get { return GetDeviceType() == DeviceType.THREE_WAY; } }
        public bool IsMultiDevice { get { return GetDeviceType() == DeviceType.MULTI; } }

        protected abstract DeviceType GetDeviceType();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupSource">A group which contains the buttons</param>
        /// <param name="groupTarget">A group which should contain the copied buttons</param>
        private void CopyButtonsWithBehaviors(Transform groupSource, Transform groupTarget)
        {
            if (groupSource.childCount > 1)
            {
                for (int i = 1; i < groupSource.childCount; i++)
                {
                    Transform child = groupSource.GetChild(i);

                    //TODO a better check would be nice
                    if (child.gameObject.name.Contains("button") || child.gameObject.name.Contains("Button"))
                    {
                        CreateNewButtons(groupTarget, child);
                    }
                }
            }
        }

        /// <summary>
        /// Copies the billboard groups with the whole contents and behaviors
        /// </summary>
        /// <param name="groupSource"></param>
        /// <param name="groupTarget"></param>
        protected void CopyBillboardGroup(Transform groupSource, Transform groupTarget)
        {
            CopyDeviceHeaderData(groupSource.Find("Canvas"), groupTarget.Find("Canvas"));
            CopyButtonsWithBehaviors(groupSource, groupTarget);
        }


        private void CopyDeviceHeaderData(Transform sourceCanvas, Transform targetCanvas)
        {
            //TODO image source
            SpriteRenderer sourceImage = sourceCanvas.Find("Image").GetComponent<SpriteRenderer>();
            var sDesc = sourceCanvas.Find("Description").GetComponent<Text>();
            var sValue = sourceCanvas.Find("Value").GetComponent<Text>();

            SpriteRenderer targetImage = targetCanvas.Find("Image").GetComponent<SpriteRenderer>();
            var tDesc = targetCanvas.Find("Description").GetComponent<Text>();
            var tValue = targetCanvas.Find("Value").GetComponent<Text>();

            targetImage.sprite = sourceImage.sprite;
            tDesc.text = sDesc.text;
            tValue.text = sValue.text;
        }

        private void CreateNewButtons(Transform parent, Transform sourceChild)
        {
            GameObject newChild = Instantiate(sourceChild.gameObject);

            DefaultDeviceButtonBehavior[] sourceBehaviors = sourceChild.GetComponentsInChildren<DefaultDeviceButtonBehavior>();
            DefaultDeviceButtonBehavior[] targetBehaviors = newChild.GetComponentsInChildren<DefaultDeviceButtonBehavior>();

            if (sourceBehaviors.Length != targetBehaviors.Length)
            {
                Debug.LogError("defaultbuttonbehaviors count does not match for copied instance");
                return;
            }

            for (int i = 0; i < sourceBehaviors.Length; i++)
            {
                targetBehaviors[i].DeviceId = sourceBehaviors[i].DeviceId;
                targetBehaviors[i].RealCommandName = sourceBehaviors[i].RealCommandName;
                targetBehaviors[i].CommandDisplayName = sourceBehaviors[i].CommandDisplayName;
            }

            newChild.transform.SetParent(parent, false);
        }

        public abstract void Hide();
        public abstract void Show();
    }
}
