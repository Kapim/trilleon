﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TrilleonAutomation {

   /// <summary>
   ///   These Demo tests will always skip if the current test run involves a single device. 
   ///   Required:
   ///   1) Communication tool for multiple devices/servers to communicate on.
   ///   2) Another client to communicate with this one.
   ///   3) Explicit or implicit avenue for these two devices to determine that they should form a "Buddy" relationship.
   /// </summary>
   [AutomationClass]
   [DebugClass]
   public class BuddySystemTests : MonoBehaviour {

      [Automation("Debug/Demo")]
      [BuddySystem(Buddy.Action)]
      public IEnumerator BuddySystemTest_Action() {
         string requiredValue = "I AM A BUDDY PRIMARY";
         BuddyHandler.SendInfoForBuddyTests("REQUIRED_VALUE", requiredValue);
         yield return StartCoroutine(Q.driver.WaitRealTime(1));
      }
         
      //You can have any number of Reactions or CounterReactions that point to the single, above Action test.
      [Automation("Debug/Demo")]
      [BuddySystem(Buddy.Reaction, "BuddySystemTest_Action")]
      public IEnumerator BuddySystemTest_Reaction() {
         //Verify that the required test data was passed from the Primary buddy to the Secondary Buddy.
         string expectedValue = BuddyHandler.GetValueFromBuddyPrimaryTestForSecondaryTest("REQUIRED_VALUE");
         Q.assert.IsTrue(expectedValue == "I AM A BUDDY PRIMARY", "Buddy primary failed to provide required information for secondary reaction test.");
         yield return StartCoroutine(Q.driver.WaitRealTime(1));
      }

      [Automation("Debug/Demo")]
      [BuddySystem(Buddy.CounterReaction, "BuddySystemTest_Action")]
      public IEnumerator BuddySystemTest_CounterReaction() {
         //Some logic to verify that Reaction test had expected effect.
         yield return StartCoroutine(Q.driver.WaitRealTime(1));
      }

   }

}