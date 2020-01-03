using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    public class CanvasAnchorLookup : SerializedMonoBehaviour
    {
        public static CanvasAnchorLookup Instance { get; private set; }

        public RectTransform[] canvasArr = new RectTransform[10];
        public RectTransform outOfScene;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

    }
}