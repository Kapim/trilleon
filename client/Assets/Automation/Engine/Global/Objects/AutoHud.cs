﻿using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

namespace TrilleonAutomation {
   
   public class AutoHud : MonoBehaviour {

      public const string GAME_OBJECT_NAME = "AutoHud";
      const int DEFAULT_MESSAGE_DURATION = 5;

      public static AutoHud StaticSelfComponent {
         get { 
            if(_staticSelfComponent == null) {
               _staticSelfComponent = AutomationMaster.StaticSelf.GetComponent<AutoHud>();
            }
            return _staticSelfComponent;
         }
         private set { 
            _staticSelfComponent = value;
         }
      }
      private static AutoHud _staticSelfComponent;

      public bool IsSet { get; set; }
      public AutoHudAnchor PositionOnScreen {
         get { 
            return _positionOnScreen;
         }
         set { 
            
            _positionOnScreen = value;

            switch(value) {
               case AutoHudAnchor.MiddleLeft:

                  break;
               case AutoHudAnchor.MiddleRight:

                  break;
               case AutoHudAnchor.TopLeft:

                  break;
               case AutoHudAnchor.TopMiddle:

                  break;
               case AutoHudAnchor.TopRight:

                  break;
               case AutoHudAnchor.BottomLeft:

                  break;
               case AutoHudAnchor.BottomMiddle:
                  positionHud.anchorMax = new Vector2(0.5f, 0);
                  positionHud.anchorMin = new Vector2(0.5f, 0);
                  positionHud.pivot = new Vector2(0.5f, 0.5f);
                  break;
               case AutoHudAnchor.BottomRight:

                  break;
            }

         }
      }
      public AutoHudAnchor _positionOnScreen;

      Text Text {
         get { 
            if(_text == null) {
               GameObject textObj = TextObject;
               if(textObj != null) {
                  _text = textObj.GetComponent<Text>();
                  return _text;
               }
               return null;
            }
            return _text;
         }
         set { 
            _text = value;
         }
      }
      Text _text = null;

      CanvasGroup TextCanvasGroup {
         get { 
            if(_textCanvasGroup == null) {
               _textCanvasGroup = this.GetComponent<CanvasGroup>();
               return _textCanvasGroup;
            }
            return _textCanvasGroup;
         }
         set { 
            _textCanvasGroup = value;
         }
      }
      CanvasGroup _textCanvasGroup = null;

      public GameObject AutoHudBackground {
         get {
            if(_autoHudBackground == null) {
               _autoHudBackground = Q.driver.FindIn(this.GetChildren(), By.Name, GAME_OBJECT_NAME);
               return _autoHudBackground;
            }
            return _autoHudBackground;
         }
         private set{
            _autoHudBackground = value;
         }
      }
      public GameObject _autoHudBackground;

      public GameObject TextObject {
         get {
            if(_textObject == null) {
               _textObject = Q.driver.FindIn(gameObject, By.Name, "Text");
               return _textObject;
            }
            return _textObject;
         }
         private set{
            _textObject = value;
         }
      }
      public GameObject _textObject;

      RectTransform positionHud = new RectTransform();

      void Start() {

         //Create AutoHud object.
         GetComponent<RectTransform>().position = new Vector3(0, 40, 0);

         //Put RenderMode in Screen Space.
         Canvas autoHudCanvas = gameObject.AddComponent<Canvas>();
         autoHudCanvas.renderMode = RenderMode.ScreenSpaceOverlay;

         //Do not block raycasts.
         CanvasGroup autoHudCanvasGroup = gameObject.AddComponent<CanvasGroup>();
         autoHudCanvasGroup.blocksRaycasts = false;

         //Position the HUD in the requested spot on screen.
         RectTransform autoHudRect = this.GetComponent<RectTransform>();
         autoHudRect.position = new Vector3(0, 0, 0);

         AutoHudBackground = new GameObject("Bg", typeof(RectTransform));
         //Set this object as a direct child of the AutoHud.
         AutoHudBackground.transform.SetParent(this.transform);

         positionHud = AutoHudBackground.GetComponent<RectTransform>();
         positionHud.sizeDelta = new Vector2(300, 75);
         positionHud.position = new Vector3(0, 40, 0);

         //Add region for text to appear.
         TextObject = new GameObject("Text", typeof(RectTransform));
         TextObject.AddComponent<Text>();
         //Set this object as a direct child of the Background object.
         TextObject.transform.SetParent(AutoHudBackground.transform);
         TextObject.GetComponent<RectTransform>().position = new Vector3(0, 0, 0);

         //Middle Bottom TODO: GET PREFERENCE
         PositionOnScreen = AutoHudAnchor.BottomMiddle;

         //Give background to HUD text area.
         Image bg = AutoHudBackground.AddComponent<Image>();
         bg.color = new Color32(120, 120, 120, 110);
         bg.raycastTarget = false;

         //Set Initial Text.
         Text.raycastTarget = false;
         Text.color = Color.white;
         Text.fontSize = 25;
         Text.alignment = TextAnchor.MiddleCenter;
         Text.text = "Initialized";
         Text.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 75);
         Text.transform.localPosition = new Vector3(0, 0, 0);
         Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
         Text.font = ArialFont;
         TextCanvasGroup = Text.gameObject.AddComponent<CanvasGroup>();
         TextCanvasGroup.alpha = 1f;

         IsSet = true;

      }

      public static void UpdateMessage(string message, int duration = DEFAULT_MESSAGE_DURATION) {

         #if UNITY_EDITOR
         if(StaticSelfComponent != null) {
            
            StaticSelfComponent.SetMessage(message, duration);

         }
         #endif

      }

      void SetMessage(string message, int duration = DEFAULT_MESSAGE_DURATION) {

         StopCoroutine("FadeOut"); //Stop existing fade out.
         if(Text != null) {

            Text.text = message;
            TextCanvasGroup.alpha = 1f;
            StartCoroutine(FadeOut(duration));

         }

      }

      IEnumerator FadeOut(float duration = 2.5f) {

         while(TextCanvasGroup.alpha > 0f) {

            float increment = Time.deltaTime / duration * 100;
            TextCanvasGroup.alpha -= increment * 1;
            yield return new WaitForEndOfFrame();

         }

         yield return null;

      }

   }

   public enum AutoHudAnchor {
      MiddleLeft,
      MiddleRight,
      TopLeft,
      TopMiddle,
      TopRight,
      BottomLeft,
      BottomMiddle,
      BottomRight
   };

}