using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dialogue
{
    public class CharacterCanvas : MonoBehaviour
    {
        public static float speed = 1.5f;
        public static float catchUpSpeed = .5f;

        [SerializeField]
        private RectTransform charTr;
        [SerializeField]
        private Image charSprite;

        public RectTransform curAnchor;

        private IEnumerator curCoroutine;

        private bool posUpdated = false;
        public bool catchUpToAnchor = false;

        private float utime_t = 0;

        private void Awake()
        {
            if (charTr == null)
                charTr = GetComponent<RectTransform>();

            if (charSprite == null)
                charSprite = GetComponent<Image>();
        }

        private void Update()
        {
            if (curAnchor != null && charTr.position != curAnchor.position && !catchUpToAnchor)
            {
                utime_t = 0;
                catchUpToAnchor = true;
            } else if(charTr.position == curAnchor.position)
            {
                catchUpToAnchor = false;
            }

            if(catchUpToAnchor)
            {
                utime_t += Time.deltaTime;
                posUpdated = false;
            }
        }

        private void FixedUpdate()
        {
            if (!posUpdated && curCoroutine == null)
                charTr.position = Vector3.Lerp(charTr.position, curAnchor.position, utime_t * catchUpSpeed);
        }


        [Button]
        public bool MoveAndRotate(RectTransform start, RectTransform end, Sprite nextSprite, bool rotateEnabled = false)
        {
            if (curCoroutine != null)
                return false;

            curCoroutine = MoveAndRotateCoroutine(start, end, nextSprite, rotateEnabled);

            StartCoroutine(curCoroutine);

            return true;
        }

        private IEnumerator MoveAndRotateCoroutine(RectTransform startPos, RectTransform targetPos, Sprite nextSprite, bool rotateEnabled)
        {
            yield return null;
            float initTime = Time.unscaledTime;
            float time_t = 0;
            bool spriteSwitched = false;

            Quaternion initRot = Quaternion.identity;
            Quaternion virtualTargetRot = Quaternion.Euler(0, 180, 0);

            float slerpTime = 0;

            while (time_t < 1)
            {
                time_t = (Time.unscaledTime - initTime) * speed;
                if (!spriteSwitched)
                    slerpTime = time_t;
                else
                    slerpTime = 1 - time_t;

                if (!spriteSwitched && slerpTime > .5f)
                {
                    charSprite.sprite = nextSprite;
                    spriteSwitched = true;
                }

                if (rotateEnabled)
                {
                    charTr.localRotation = Quaternion.Slerp(initRot, virtualTargetRot, slerpTime);
                }

                charTr.position = Vector3.Lerp(startPos.position, targetPos.position, time_t);

                yield return null;
            }

            charTr.localRotation = initRot;
            charTr.position = targetPos.position;
            curAnchor = targetPos;

            curCoroutine = null;
        }
    }
}