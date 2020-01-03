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
            if (spriteTransforms == null)
                spriteTransforms = GetComponentsInChildren<RectTransform>();
            if (parentTr == null)
                parentTr = GetComponent<RectTransform>();

            lastCount = -1;
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
                count = 5;

            float width = parentTr.rect.width;

            float segmentWidth = width / (count + 1);

            int j = 0;
            for(int i = spriteTransforms.Length - 1; i >= spriteTransforms.Length - count; i--)
            {
                Vector3 oldPos = spriteTransforms[i].anchoredPosition;

                if (pos == Position.L)
                {
                    oldPos.x = segmentWidth * (j + 1);
                }
                else
                {
                    oldPos.x = segmentWidth * (j + 1) * -1;
                }

                spriteTransforms[i].anchoredPosition = oldPos;
                j++;
            }
        }
    }
}