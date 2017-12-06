using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace VRTK
{
    public class RightHandEventHandler : MonoBehaviour
    {

        public OSC osc;
        public VRTK_ControllerEvents rightController;

        private bool triggerPressed = false;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        protected virtual void Awake()
        {
            Initialize();
        }

        protected virtual void Initialize()
        {
            //if (rightController == null)
            if (GetComponent<VRTK_ControllerEvents>() == null)
            {
                rightController = GetComponentInParent<VRTK_ControllerEvents>();
            }
        }

        protected virtual void OnEnable()
        {
            if (rightController == null)
            {
                VRTK_Logger.Error(VRTK_Logger.GetCommonMessage(VRTK_Logger.CommonMessageKeys.REQUIRED_COMPONENT_MISSING_NOT_INJECTED, "RadialMenuController", "VRTK_ControllerEvents", "events", "the parent"));
                return;
            }
            else
            {
                rightController.TriggerClicked += RightController_TriggerClicked;
                rightController.TriggerUnclicked += RightController_TriggerUnclicked;
            }
        }

        private void RightController_TriggerUnclicked(object sender, ControllerInteractionEventArgs e)
        {
            triggerPressed = false;
        }

        private void RightController_TriggerClicked(object sender, ControllerInteractionEventArgs e)
        {
            triggerPressed = true;
        }
    }
}
