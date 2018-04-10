using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRTK
{

	public class LeftHand_Osc : MonoBehaviour
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
			message.address = "/LeftHand";

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
				rightController.TriggerClicked += LeftController_TriggerClicked;
				rightController.TriggerUnclicked += LeftController_TriggerUnclicked;
				rightController.TouchpadAxisChanged += LeftController_TouchpadAxisChanged;
				rightController.GripReleased += LeftController_GripReleased;
				rightController.GripPressed += LeftController_GripPressed;
				rightController.TouchpadPressed += LeftController_TouchpadPressed;
				rightController.TouchpadReleased += LeftController_TouchpadReleased;
				rightController.ButtonTwoPressed += LeftController_ButtonTwoPressed;
				rightController.ButtonTwoReleased += LeftController_ButtonTwoReleased;
			}
		}

		//__ Grip Status _//

		private void LeftController_GripPressed(object sender, ControllerInteractionEventArgs e)
		{
			gripStatus = 1;
			Debug.Log("grip pressed");
		}

		private void LeftController_GripReleased(object sender, ControllerInteractionEventArgs e)
		{
			gripStatus = 0;
			Debug.Log("grip released");
		}

		//_ Touchpad status _//

		private void LeftController_TouchpadAxisChanged(object sender, ControllerInteractionEventArgs e)
		{
			touchpadAngle = e.touchpadAngle;
		}

		private void LeftController_TouchpadPressed(object sender, ControllerInteractionEventArgs e)
		{
			touchpadStatus = 1;
		}

		private void LeftController_TouchpadReleased(object sender, ControllerInteractionEventArgs e)
		{
			touchpadStatus = 0;
		}

		//_ Trigger status _//

		private void LeftController_TriggerUnclicked(object sender, ControllerInteractionEventArgs e)
		{
			triggerStatus = 0;
		}

		private void LeftController_TriggerClicked(object sender, ControllerInteractionEventArgs e)
		{
			triggerStatus = 1;
		}

		//_ Button two _//

		private void LeftController_ButtonTwoPressed(object sender, ControllerInteractionEventArgs e)
		{
			buttonTwoStatus = 1;
		}

		private void LeftController_ButtonTwoReleased(object sender, ControllerInteractionEventArgs e)
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
