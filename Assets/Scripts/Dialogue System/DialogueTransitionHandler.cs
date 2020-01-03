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
            RectTransform from, to;

            switch (tType)
            {
                case DTransitionEnum.Move:
                    from = CanvasAnchorLookup.Instance.canvasArr[posFrom];
                    to = CanvasAnchorLookup.Instance.canvasArr[posTo];
                    CharacterCanvasLookup.Instance.charCanvasLookup[charID].MoveAndRotate(from, to, nextSprite);
                    break;
                case DTransitionEnum.Add:
                    from = CanvasAnchorLookup.Instance.outOfScene;
                    to = CanvasAnchorLookup.Instance.canvasArr[posTo];
                    CharacterCanvasLookup.Instance.charCanvasLookup[charID].MoveAndRotate(from, to, nextSprite, true);
                    break;
                case DTransitionEnum.Remove:
                    from = CanvasAnchorLookup.Instance.canvasArr[posFrom];
                    to = CanvasAnchorLookup.Instance.outOfScene;
                    CharacterCanvasLookup.Instance.charCanvasLookup[charID].MoveAndRotate(from, to, nextSprite, true);
                    break;
                default:
                    break;

            }
        }
    }
}