﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

namespace TrilleonAutomation {

	[AutomationClass]
	[DependencyClass(0)]
	[DebugClass]
	public class GeneralFrameworkTests : MonoBehaviour {

		[SetUpClass]
		public IEnumerator SetUpClass() {

			//Prepare for all of the tests in this class. What does the account need to run these tests? Experience? Currency?
			yield return null;

		}

		[SetUp]
		public IEnumerator SetUp() {

			//Prepare for your test. What does the test need to properly execute?
			yield return null;

		}

		/// <summary>
		/// This empty test can be used to insert code/debug/play around with logic. No other Debug tests should be altered in any way.
		/// </summary>
		[Automation("Debug")]
		public IEnumerator Test() {

			Func<bool> condition = () => 5 + 5 == 10;
			string test = condition.Method.ToString();
			Debug.Log(test);
			yield return null;

		}

		[Automation("Debug")]
		[Automation("Trilleon/Validation")]
		[Validate(Expect.Failure)]
		public IEnumerator Demo_AutoFail() {

			Q.assert.IsTrue(false, "Auto fail test.");
			yield return null;

		}

		[Automation("Debug")]
		[Automation("Trilleon/Validation")]
		[DependencyWeb("Demo_AutoFail")]
		[Validate(Expect.Skipped)]
		public IEnumerator Demo_AutoSkip() {

			Q.assert.IsTrue(true, "This assertion should not appear as this test is expected to be skipped.");
			yield return null;

		}

		[Automation("Debug")]
		[Automation("Trilleon/Validation")]
		[Validate(Expect.Success)]
		public IEnumerator Demo_AutoPass() {

			Q.assert.IsTrue(string.IsNullOrEmpty(RandomHelper()), "Pass this assertion!");
			yield return null;

		}

		[Ignore("Demo Ignore Test")]
		[Automation("Debug")]
		[Automation("Trilleon/Validation")]
		[Validate(Expect.Ignored)]
		public IEnumerator Demo_AutoIgnore() {

			Q.assert.IsTrue(true, "This assertion should not appear as this test is set to be ignored.");
			yield return null;

		}

		[Automation("Debug")]
		[Automation("Trilleon/Validation")]
		[DependencyTest(3)]
		[Validate(Expect.Success)]
		[Validate(Expect.OrderRan, "3")]
		public IEnumerator DependencyMasterTest_03() {

			Q.assert.IsTrue(true, "Don't worry, I might look out of place in the ExampleDemoTests class, but this is exactly where I should be.");
			Q.assert.IsTrue(true, "Note that this class has a DependencyClass attribute with an order of one. If you came looking for me from the DependencyMasterDemoTests class, you will also know that it too shares the same ID");
			Q.assert.IsTrue(true, "That's okay. Multiple classes can have a DependencyClass attribute with the same ID. It links them together, should the need ever arise.");
			Q.assert.IsTrue(true, "The result is that, when all tests are found for a given test run, I will be ordered in relation to the tests contained in that class.");
			Q.assert.IsTrue(true, "Additionally, I was given a DependencyTest attribute with the order of 3.");
			Q.assert.IsTrue(true, "In other words, I will be the third test run by the test runner. Right after \"DependencyMasterTest_02\", and right before \"DependencyMasterTest_05\".");
			yield return null;

		}

		#region HardCrash

		//Compiles, but causes fatal SIGTERM(bad access) exception when called.
		private bool HardCrashWhenYouReferenceThis {
			get { 
				return HardCrashWhenYouReferenceThis;
			}
		}

		[Automation("Debug")]
		[TestRunnerFlag(TestFlag.OnlyLaunchWhenExplicitlyCalled)]
		public IEnumerator CauseHardCrashDuringTestRun() {

			yield return StartCoroutine(Q.driver.WaitRealTime(1));
			if(HardCrashWhenYouReferenceThis) {
				yield return null;
			}
			yield return null;

		}

		#endregion

		[TearDown]
		public IEnumerator TearDown() {

			//Maybe add some important variable to storage for use in tests outside of this class.
			Q.storage.AddOrModify("SOME_IMPORTANT_VARIABLE", RandomHelper());
			yield return null;

		}

		[TearDownClass]
		public IEnumerator TearDownClass() {

			yield return null;

		}

		#region Helper Methods

		private string RandomHelper() {

			//Ostensibly some logic here.
			return string.Empty;;

		}

		#endregion

	}

}