using UnityEngine;
using System.Collections;
using System.Diagnostics.Tracing;
using Base;
using TMPro;
using RuntimeInspectorNamespace;
using DanielLochner.Assets.SimpleSideMenu;

namespace TrilleonAutomation
{

    [AutomationClass]
    public class MainScreenTests : MonoBehaviour
    {
        GameObject sceneseBtn, projectsBtn, packagesBtn;
        GameObject newSceneBtn, saveSceneBtn, closeSceneBtn;
        GameObject inputDialog, confirmationDialog;
        GameObject inputDialogInput;
        GameObject inputDialogOKButton;
        GameObject confirmationDialogOKButton;
        SceneOptionMenu sceneOptionMenu;
        GameObject sceneRename, sceneRemove;
        [SetUpClass]
        public IEnumerator SetUpClass()
        {

            yield return null;

        }

        [SetUp]
        public IEnumerator SetUp()
        {
            GameObject buttonsLandscape = Q.driver.Find(By.Name, "ButtonsLandscape");
            GameObject scenesList = Q.driver.Find(By.Name, "ScenesList");
            GameObject mainScreen = Q.driver.Find(By.Name, "MainScreen");

            sceneseBtn = Q.driver.FindIn(buttonsLandscape, By.Name, "ScenesButton");
            projectsBtn = Q.driver.FindIn(buttonsLandscape, By.Name, "ProjectsButton");
            packagesBtn = Q.driver.FindIn(buttonsLandscape, By.Name, "PackagesButton");

            saveSceneBtn = Q.driver.Find(By.Name, "SaveScene");
            closeSceneBtn = Q.driver.Find(By.Name, "CloseScene");

            newSceneBtn = Q.driver.FindIn(scenesList, By.Name, "TileNew(Clone)");

            inputDialog = Q.driver.FindIn(mainScreen, By.Name, "InputDialog");

            inputDialogInput = Q.driver.FindIn(inputDialog, By.Name, "Input");
            inputDialogOKButton = Q.driver.FindIn(inputDialog, By.Name, "Got It");

            sceneOptionMenu = MainScreen.Instance.SceneOptionMenu;
            sceneRename = Q.driver.FindIn(sceneOptionMenu.gameObject, By.Name, "Rename");
            sceneRemove = Q.driver.FindIn(sceneOptionMenu.gameObject, By.Name, "Delete");

            confirmationDialog = sceneOptionMenu.confirmationDialog.gameObject;
            confirmationDialogOKButton = Q.driver.FindIn(confirmationDialog, By.Name, "Ok");

            yield return null;
        }
        [DependencyTest(1)]
        [Automation("Main screen tests")]
        public IEnumerator UserCanSwitchListsTest()
        {
            yield return StartCoroutine(Q.assert.IsTrue(MainScreen.Instance.ScenesList.gameObject.activeSelf, "Scenes should be active"));
            yield return StartCoroutine(Q.assert.IsTrue(!MainScreen.Instance.ProjectsList.gameObject.activeSelf, "Projects should be inactive"));
            yield return StartCoroutine(Q.assert.IsTrue(!MainScreen.Instance.PackageList.gameObject.activeSelf, "Packages should be inactive"));
            yield return StartCoroutine(Q.driver.Click(projectsBtn, "Click on projects."));
            yield return StartCoroutine(Q.assert.IsTrue(!MainScreen.Instance.ScenesList.gameObject.activeSelf, "Scenes should be inactive"));
            yield return StartCoroutine(Q.assert.IsTrue(MainScreen.Instance.ProjectsList.gameObject.activeSelf, "Projects should be active"));
            yield return StartCoroutine(Q.assert.IsTrue(!MainScreen.Instance.PackageList.gameObject.activeSelf, "Packages should be inactive"));
            yield return StartCoroutine(Q.driver.Click(packagesBtn, "Click on packages."));
            yield return StartCoroutine(Q.assert.IsTrue(!MainScreen.Instance.ScenesList.gameObject.activeSelf, "Scenes should be inactive"));
            yield return StartCoroutine(Q.assert.IsTrue(!MainScreen.Instance.ProjectsList.gameObject.activeSelf, "Projects should be inactive"));
            yield return StartCoroutine(Q.assert.IsTrue(MainScreen.Instance.PackageList.gameObject.activeSelf, "Packages should be active"));
            yield return StartCoroutine(Q.driver.Click(sceneseBtn, "Click on scenes."));
            yield return StartCoroutine(Q.assert.IsTrue(MainScreen.Instance.ScenesList.gameObject.activeSelf, "Scenes should be active"));
            yield return StartCoroutine(Q.assert.IsTrue(!MainScreen.Instance.ProjectsList.gameObject.activeSelf, "Projects should be inactive"));
            yield return StartCoroutine(Q.assert.IsTrue(!MainScreen.Instance.PackageList.gameObject.activeSelf, "Packages should be inactive"));

        }
        [DependencyTest(2)]
        [Automation("Main screen tests")]
        public IEnumerator UserCanCreateAndSaveSceneTest() {
            yield return StartCoroutine(Q.driver.Click(newSceneBtn, "Click on new scene button"));
            inputDialog.GetComponent<InputDialog>().SetInputValue("test scene");
            yield return StartCoroutine(Q.driver.Click(inputDialogOKButton, "Click on Got it button"));

            yield return StartCoroutine(Q.driver.Click(saveSceneBtn, "Click on save scene button"));
            yield return StartCoroutine(Q.driver.Click(closeSceneBtn, "Click on close scene button"));
            bool sceneExist = false;
            foreach (IO.Swagger.Model.ListScenesResponseData scene in Base.GameManager.Instance.Scenes) {
                if (scene.Name == "test scene")
                    sceneExist = true;
            }
            yield return StartCoroutine(Q.assert.IsTrue(sceneExist, "Test scene does not exist"));
            
        }
        [DependencyTest(3)]
        [Automation("Main screen tests")]
        public IEnumerator UserCanRenameSceneTest() {
            SceneTile sceneTile = MainScreen.Instance.GetSceneTile("test scene");
            yield return StartCoroutine(Q.assert.IsTrue(sceneTile != null, "Scene not found!"));
            yield return StartCoroutine(Q.driver.Click(sceneTile.GetOptionButton().gameObject, "Click on option menu."));
            yield return StartCoroutine(Q.assert.IsTrue(sceneOptionMenu.GetComponent<SimpleSideMenu>().CurrentState == SimpleSideMenu.State.Open, "Option menu not opened!"));
            yield return StartCoroutine(Q.driver.Click(sceneRename, "Click on scene rename."));
            inputDialog.GetComponent<InputDialog>().SetInputValue("test scene renamed");
            yield return StartCoroutine(Q.driver.Click(inputDialogOKButton, "Click on Got it button"));
            // wait for server to respond
            yield return new WaitForSeconds(2);
            sceneTile = MainScreen.Instance.GetSceneTile("test scene renamed");
            yield return StartCoroutine(Q.assert.IsTrue(sceneTile != null, "Scene not found!"));
            bool sceneExist = false;
            foreach (IO.Swagger.Model.ListScenesResponseData scene in Base.GameManager.Instance.Scenes) {
                if (scene.Name == "test scene renamed")
                    sceneExist = true;
            }
            yield return StartCoroutine(Q.assert.IsTrue(sceneExist, "Test scene does not exist"));
        }
        [DependencyTest(4)]
        [Automation("Main screen tests")]
        public IEnumerator UserCanRemoveSceneTest() {
            SceneTile sceneTile = MainScreen.Instance.GetSceneTile("test scene renamed");
            yield return StartCoroutine(Q.assert.IsTrue(sceneTile != null, "Scene not found!"));
            yield return StartCoroutine(Q.driver.Click(sceneTile.GetOptionButton().gameObject, "Click on option menu."));
            yield return StartCoroutine(Q.assert.IsTrue(sceneOptionMenu.GetComponent<SimpleSideMenu>().CurrentState == SimpleSideMenu.State.Open, "Option menu not opened!"));
            yield return StartCoroutine(Q.driver.Click(sceneRemove, "Click on scene remove."));
            yield return StartCoroutine(Q.driver.Click(confirmationDialogOKButton, "Click on ok button"));
            // wait for server to respond
            yield return new WaitForSeconds(2);
            try {
                sceneTile = MainScreen.Instance.GetSceneTile("test scene renamed");
            } catch (ItemNotFoundException) {
                sceneTile = null;
            }            
            yield return StartCoroutine(Q.assert.IsTrue(sceneTile == null, "Scene should be removed but it is not!"));
            bool sceneExist = false;
            foreach (IO.Swagger.Model.ListScenesResponseData scene in Base.GameManager.Instance.Scenes) {
                if (scene.Name == "test scene renamed")
                    sceneExist = true;
            }
            yield return StartCoroutine(Q.assert.IsTrue(!sceneExist, "Test scene should not exist"));
        }


        [TearDown]
        public IEnumerator TearDown()
        {

            yield return null;

        }

        [TearDownClass]
        public IEnumerator TearDownClass()
        {

            yield return null;

        }

    }

}