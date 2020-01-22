using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class GameStateTests : MonoBehaviour
    {
        [UnityTest]
        public IEnumerator EnsureObjectStateDisables()
        {
            GameController script = new GameController();
            GameObject gameObject = new GameObject("TestObj");
            gameObject.AddComponent<Rigidbody2D>();

            RigidbodyConstraints2D previous = gameObject.GetComponent<Rigidbody2D>().constraints;

            script.DisableObjects();
            yield return new WaitForSeconds(0.1f);

            RigidbodyConstraints2D result = gameObject.GetComponent<Rigidbody2D>().constraints; ;
            Destroy(gameObject);

            Assert.AreNotEqual(previous, result);
        }
    }
}
