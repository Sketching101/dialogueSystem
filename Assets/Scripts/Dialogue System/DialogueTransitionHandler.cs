using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    public static class DialogueTransitionHandler
    {
        public static void HandleTransition(DTransition transition)
        {
            string charID = transition.charID;
            int posTo = transition.posTo;
            int posFrom = transition.posFrom;
            DTransitionEnum tType = transition.transitionType;
            Sprite nextSprite = transition.nextSprite;

            switch (tType)
            {
                case DTransitionEnum.Move:
                    RectTransform from = CanvasAnchorLookup.Instance.canvasArr[posFrom];
                    RectTransform to = CanvasAnchorLookup.Instance.canvasArr[posTo];
                    CharacterCanvasLookup.Instance.charCanvasLookup[charID].MoveAndRotate(from, to, nextSprite);
                    break;
                default:
                    break;

            }
        }
    }
}