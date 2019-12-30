using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue {
    public class SpreadRectTransforms : MonoBehaviour
    {
        public Position pos;
        public DialogueMap dMap;
        public int spriteCount
        {
            get
            {
                if (pos == Position.L)
                    return dMap.lCount;
                else
                    return dMap.rCount;
            }
        }
        private int lastCount;

        public RectTransform parentTr;
        public RectTransform[] spriteTransforms;

        private void Awake()
        {
            lastCount = 0;
            spriteTransforms = GetComponentsInChildren<RectTransform>();
            if (parentTr == null)
                parentTr = GetComponent<RectTransform>();
        }

        private void Update()
        {
            if (lastCount == spriteCount)
                return;

            lastCount = spriteCount;

            ArrangeSprites(lastCount);               
        }

        private void ArrangeSprites(int count)
        {
            if (count == 0)
                return;

            float width = parentTr.rect.width;

            float segmentWidth = width / (count * 2);

            width += segmentWidth;
            segmentWidth *= 2;

            for(int i = spriteTransforms.Length - 1; i >= count; i--)
            {
                Vector3 oldPos = spriteTransforms[i].localPosition;
                oldPos.x = width - segmentWidth - width/2;
                spriteTransforms[i].localPosition = oldPos;
            }
        }
    }
}