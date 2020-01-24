using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

namespace Tests
{
    public class TouchControlTest : MonoBehaviour
    {
        [UnityTest]
        public IEnumerator TestButtonSpawn()
        {
            GameObject gameController = Instantiate(Resources.Load<GameObject>("Prefabs/GameControllerTouch"));
            yield return null;
            bool buttonsFound = false;
            foreach(Transform child in gameController.transform)
            {
                if(child.CompareTag("TouchInterface"))
                {
                    buttonsFound = true;
                }
            }
            Assert.IsTrue(buttonsFound);
        }

        [UnityTest]
        public IEnumerator CancelButtonFunctionalityTest()
        {
            GameObject component = Instantiate(Resources.Load<GameObject>("Prefabs/InteractiveComponents/Cannon Variant"));
            component.GetComponent<ComponentInteraction>().SetSelected(true);
            Assert.IsTrue(component.GetComponent<ComponentInteraction>().GetSelected());
            component.GetComponent<ComponentInteraction>().UpdateButtonPressed("CancelButton");
            yield return new WaitForSeconds(1.0f);
            Assert.IsTrue(component.GetComponent<ComponentInteraction>().GetSelected());

        }

        [UnityTest]
        public IEnumerator DeleteButtonFunctionalityTest()
        {
            GameObject component = Instantiate(Resources.Load<GameObject>("Prefabs/InteractiveComponents/Cannon Variant"));
            component.GetComponent<ComponentInteraction>().SetSelected(true);
            Assert.IsTrue(component.GetComponent<ComponentInteraction>().GetSelected());
            component.GetComponent<ComponentInteraction>().UpdateButtonPressed("DeleteButton");
            yield return new WaitForSeconds(0.1f);
            //Assert.IsNull(component);
        }

        [UnityTest]
        public IEnumerator MoveButtonFunctionalityTest()
        {
            GameObject gameController = Instantiate(Resources.Load<GameObject>("Prefabs/GameControllerTouch"));
            GameObject moveButton = null;
            foreach (Transform child in gameController.transform)
            {
                if (child.CompareTag("TouchInterface"))
                {
                    foreach (Transform button in child)
                    {
                        if (button.name == "MoveButton")
                        {
                            moveButton = button.gameObject;
                        }
                    }
                }
            }
            Assert.IsNotNull(moveButton);

            GameObject component = Instantiate(Resources.Load<GameObject>("Prefabs/InteractiveComponents/Cannon Variant"));
            component.GetComponent<ComponentInteraction>().SetSelected(true);
            Assert.IsTrue(component.GetComponent<ComponentInteraction>().GetSelected());
            Assert.IsTrue(component.GetComponent<ComponentInteraction>().GetMoveNotRotate());
            component.GetComponent<ComponentInteraction>().UpdateButtonPressed("MoveButton");
            

            yield return new WaitForSeconds(0.1f);


            Assert.IsFalse(component.GetComponent<ComponentInteraction>().GetMoveNotRotate());
            component.GetComponent<ComponentInteraction>().UpdateButtonPressed("MoveButton");
            Assert.IsTrue(component.GetComponent<ComponentInteraction>().GetMoveNotRotate());

        }
    }
}
