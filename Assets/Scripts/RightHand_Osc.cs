﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRTK
{

    public class RightHand_Osc : MonoBehaviour
    {

        public OSC osc;
        public VRTK_ControllerEvents rightController;

        private bool triggerPressed = false;
        private int triggerBoolValue = 0;
        private float attack = 10;

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
            }else
            {
                triggerBoolValue = 0;
            }

            OscMessage message = new OscMessage();

            message = new OscMessage();
            message.address = "/RightHand";
            message.values.Add(transform.position.x);
            message.values.Add(transform.position.y);
            message.values.Add(transform.position.z);
            message.values.Add(triggerBoolValue);
            message.values.Add(attack);
            message.values.Add(transform.rotation.x);
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
            }
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
        }


        float map(float a, float b, float c, float d, float input)
        {
            float output = (input - a) / (b - a) * (d - c) + c;
            return output;
        }


    }
}
