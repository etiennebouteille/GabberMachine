﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRTK
{

    public class RightHand_Osc : MonoBehaviour
    {

        public OSC osc;
        public VRTK_ControllerEvents rightController;

        private int triggerStatus = 0;
        private float touchpadAngle = 10.0f;
        private int gripStatus = 0;
		private int touchpadStatus = 0;
		private int buttonTwoStatus = 0;

        // Use this for initialization
        void Start()
        {

        }

        void Update()
        {
            OscMessage message = new OscMessage();

            message = new OscMessage();
            message.address = "/RightHand";

			///Output 1 2 3 : position
            message.values.Add(transform.position.x);
            message.values.Add(transform.position.y);
            message.values.Add(transform.position.z);

			///Output 4 5 6 : rotation
			message.values.Add(transform.rotation.x);
			message.values.Add(transform.rotation.y);
			message.values.Add(transform.rotation.z);

			///Output 7 : trigger
            message.values.Add(triggerStatus);

			///Output 8 : grip
			message.values.Add(gripStatus); 

			///Output 9 : touchpad
			message.values.Add(touchpadStatus);

			//Output 10 : touchpad angle
			message.values.Add(touchpadAngle);

			//Output 11 : button two (top)
			message.values.Add(buttonTwoStatus);
            
            osc.Send(message);

		}



        protected virtual void Initialize()
        {
            //if (rightController == null)
            if (GetComponent<VRTK_ControllerEvents>() == null)
            {
                rightController = GetComponent<VRTK_ControllerEvents>();
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
                rightController.TouchpadAxisChanged += RightController_TouchpadAxisChanged;
                rightController.GripReleased += RightController_GripReleased;
                rightController.GripPressed += RightController_GripPressed;
				rightController.TouchpadPressed += RightController_TouchpadPressed;
				rightController.TouchpadReleased += RightController_TouchpadReleased;
				rightController.ButtonTwoPressed += RightController_ButtonTwoPressed;
				rightController.ButtonTwoReleased += RightController_ButtonTwoReleased;
            }
        }

		//__ Grip Status _//

        private void RightController_GripPressed(object sender, ControllerInteractionEventArgs e)
        {
            gripStatus = 1;
            Debug.Log("grip pressed");
        }

        private void RightController_GripReleased(object sender, ControllerInteractionEventArgs e)
        {
            gripStatus = 0;
            Debug.Log("grip released");
        }

		//_ Touchpad status _//

        private void RightController_TouchpadAxisChanged(object sender, ControllerInteractionEventArgs e)
        {
			touchpadAngle = e.touchpadAngle;
        }

		private void RightController_TouchpadPressed(object sender, ControllerInteractionEventArgs e)
		{
			touchpadStatus = 1;
		}

		private void RightController_TouchpadReleased(object sender, ControllerInteractionEventArgs e)
		{
			touchpadStatus = 0;
		}

		//_ Trigger status _//

        private void RightController_TriggerUnclicked(object sender, ControllerInteractionEventArgs e)
        {
            triggerStatus = 0;
        }

        private void RightController_TriggerClicked(object sender, ControllerInteractionEventArgs e)
        {
            triggerStatus = 1;
        }

		//_ Button two _//

		private void RightController_ButtonTwoPressed(object sender, ControllerInteractionEventArgs e)
		{
			buttonTwoStatus = 1;
		}

		private void RightController_ButtonTwoReleased(object sender, ControllerInteractionEventArgs e)
		{
			buttonTwoStatus = 0;
		}


		//_ Usefull function _//

        float map(float a, float b, float c, float d, float input)
        {
            float output = (input - a) / (b - a) * (d - c) + c;
            return output;
        }


    }
}
