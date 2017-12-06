using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRTK
{

    public class LeftHand_Osc : MonoBehaviour
    {

        public OSC osc;
        public VRTK_ControllerEvents rightController;

        private bool triggerPressed = false;
        private int triggerBoolValue = 0;   //triggerPressed expressed as an int because the OSC plugin does not support sending booleans
        private float attack = 10;          //angle of touchpad
        private bool isGripClicked = false;  //if grip is clicked or not

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (triggerPressed)
            {
                triggerBoolValue = 1;
            }
            else
            {
                triggerBoolValue = 0;
            }

            OscMessage message = new OscMessage();


            //messages are only sent trough OSC when the hand is gripped
            if(isGripClicked == true)
            {
                message = new OscMessage();
                message.address = "/LeftHand";
                message.values.Add(transform.position.x);
                message.values.Add(transform.position.y);
                message.values.Add(transform.position.z);
                message.values.Add(triggerBoolValue);           ///int
                message.values.Add(attack);
                message.values.Add(transform.rotation.x);
                osc.Send(message);
            }
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
            }
        }

        private void RightController_GripPressed(object sender, ControllerInteractionEventArgs e)
        {
            isGripClicked = true;
            Debug.Log("gripClicked");
        }

        private void RightController_GripReleased(object sender, ControllerInteractionEventArgs e)
        {
            isGripClicked = false;
            Debug.Log("gripReleased");
        }


        private void RightController_TouchpadAxisChanged(object sender, ControllerInteractionEventArgs e)
        {
            attack = map(0, 360, 0, 30, e.touchpadAngle);
        }

        private void RightController_TriggerUnclicked(object sender, ControllerInteractionEventArgs e)
        {
            triggerPressed = false;
            Debug.Log("Relased");
        }

        private void RightController_TriggerClicked(object sender, ControllerInteractionEventArgs e)
        {
            triggerPressed = true;
            Debug.Log("Clicked");
            if(isGripClicked == false)
            {
                OscMessage grippedMessage = new OscMessage();
                grippedMessage.address = "/LeftHand";
                grippedMessage.values.Add(transform.position.x);
                grippedMessage.values.Add(transform.position.y);
                grippedMessage.values.Add(transform.position.z);
                grippedMessage.values.Add(triggerBoolValue);           ///int
                grippedMessage.values.Add(attack);
                grippedMessage.values.Add(transform.rotation.x);
                osc.Send(grippedMessage);
            }
        }

        //map a number from a spread to another
        float map(float a, float b, float c, float d, float input)
        {
            float output = (input - a) / (b - a) * (d - c) + c;
            return output;
        }


        //tranforms boolean value into int
        int btoi(bool boolean)
        {
            int output = 0;
            if (boolean)
            {
                output = 1;
            } else
            {
                output = 0;
            }
            return output;
        }


    }
}
