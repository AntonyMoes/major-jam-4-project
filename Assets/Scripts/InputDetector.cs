 using System;
 using UnityEngine;
 using System.Collections;
 
 public class InputDetector : MonoBehaviour {
     public enum InputState {
         MouseKeyboard,
         Controller
     };
     
     InputState _state = InputState.MouseKeyboard;
     public InputState State => _state;
     public const string ControllerPrefix = "Controller";

     void OnGUI() {
         switch(_state) {
             case InputState.MouseKeyboard:
                 if(IsControllerInput()) {
                     _state = InputState.Controller;
                 }
                 break;
             case InputState.Controller:
                 if (IsMouseKeyboard()) {
                     _state = InputState.MouseKeyboard;
                 }
                 break;
             default:
                 throw new ArgumentOutOfRangeException();
         }
     }

     static bool IsMouseKeyboard() {
         if (Event.current.isKey ||
             Event.current.isMouse) {
             return true;
         }
         
         if (Input.GetAxis("Mouse X") != 0.0f ||
             Input.GetAxis("Mouse Y") != 0.0f ) {
             return true;
         }
         
         return false;
     }

     static bool IsControllerInput() {
         if (Input.GetKey(KeyCode.Joystick1Button0)  || 
             Input.GetKey(KeyCode.Joystick1Button1)  ||
             Input.GetKey(KeyCode.Joystick1Button2)  ||
             Input.GetKey(KeyCode.Joystick1Button3)  ||
             Input.GetKey(KeyCode.Joystick1Button4)  ||
             Input.GetKey(KeyCode.Joystick1Button5)  ||
             Input.GetKey(KeyCode.Joystick1Button6)  ||
             Input.GetKey(KeyCode.Joystick1Button7)  ||
             Input.GetKey(KeyCode.Joystick1Button8)  ||
             Input.GetKey(KeyCode.Joystick1Button9)  ||
             Input.GetKey(KeyCode.Joystick1Button10) ||
             Input.GetKey(KeyCode.Joystick1Button11) ||
             Input.GetKey(KeyCode.Joystick1Button12) ||
             Input.GetKey(KeyCode.Joystick1Button13) ||
             Input.GetKey(KeyCode.Joystick1Button14) ||
             Input.GetKey(KeyCode.Joystick1Button15) ||
             Input.GetKey(KeyCode.Joystick1Button16) ||
             Input.GetKey(KeyCode.Joystick1Button17) ||
             Input.GetKey(KeyCode.Joystick1Button18) ||
             Input.GetKey(KeyCode.Joystick1Button19) ) {
             return true;
         }
         
         if (Input.GetAxis("ControllerMoveHorizontal") != 0.0f ||
             Input.GetAxis("ControllerMoveVertical") != 0.0f ||
             Input.GetAxis("ControllerLookHorizontal") != 0.0f ||
             Input.GetAxis("ControllerLookVertical") != 0.0f) {
             return true;
         }
         
         return false;
     }
 }