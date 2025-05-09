using System;
using UnityEngine;

namespace Webxotic
{
    public class UITweener : MonoBehaviour
    {
        [SerializeField] private RectTransform objectToTweenRectTransform;

        [SerializeField] private SupportedTweenAnimationTypes typeOfTween;
        [SerializeField] private LeanTweenType easeType;
        [SerializeField] private bool performOnEnable = true;
        [SerializeField] private bool performOnDisable = false;
        [SerializeField] private float durationOfTween = 0.5f;
        [SerializeField] private float delayTime;

        [SerializeField] private Vector3 defaultFrom = Vector3.zero;
        [SerializeField] private Vector3 defaultTo = Vector3.one;

        private LTDescr leanTweenObject;

        private void Awake()
        {
            if (objectToTweenRectTransform == null)
            {
                objectToTweenRectTransform = gameObject.GetComponent<RectTransform>();
            }
        }

        private void OnEnable()
        {
            if (performOnEnable)
            {
                ForwardTween();
            }

        }

        private void OnDisable()
        {
            if (performOnDisable)
            {
                BackwardTween();
            }

        }

        public void ForwardTween()
        {
            LeanTween.cancel(objectToTweenRectTransform);
            Vector3 from = defaultFrom;
            Vector3 to = defaultTo;
            PerformTween(from, to, TweenDirection.FORWARD);
        }
        public void DeactivateWithDisableTweening()
        {
            BackwardTween();
        }

        public void BackwardTween()
        {
            LeanTween.cancel(objectToTweenRectTransform);
            Vector3 from = defaultFrom;
            Vector3 to = defaultTo;
            ReverseTweenFromAndTo(ref from, ref to);
            PerformTween(from, to, TweenDirection.BACKWARD);
        }

        private void PerformTween(Vector3 from, Vector3 to, TweenDirection tweenDirection)
        {
            switch (typeOfTween)
            {
                case SupportedTweenAnimationTypes.SCALE:
                    ScaleTween(from, to, tweenDirection);
                    break;
                case SupportedTweenAnimationTypes.FADE:
                    FadeTween(from, to, tweenDirection);
                    break;
                case SupportedTweenAnimationTypes.MOVE:
                    MoveTween(from, to, tweenDirection);
                    break;
            }

        }
        private void FadeTween(Vector3 from, Vector3 to, TweenDirection tweenDirection)
        {
            if (objectToTweenRectTransform.gameObject.GetComponent<CanvasGroup>() == null)
            {
                objectToTweenRectTransform.gameObject.AddComponent<CanvasGroup>();
            }
            CanvasGroup cg = objectToTweenRectTransform.gameObject.GetComponent<CanvasGroup>();
            float targetAlpha = to.x;
            cg.alpha = from.x;

            LTDescr lTDescr = LeanTween.alphaCanvas(cg, targetAlpha, durationOfTween)
                    .setDelay(delayTime).setEase(easeType);

            if (tweenDirection == TweenDirection.BACKWARD)
            {
                lTDescr.setOnComplete(OnTweenComplete);
            }
        }

        private void ScaleTween(Vector3 from, Vector3 to, TweenDirection tweenDirection)
        {

            objectToTweenRectTransform.localScale = from;
            LTDescr lTDescr = LeanTween.scale(objectToTweenRectTransform, to, durationOfTween)
                      .setEase(easeType).setDelay(delayTime);
            if (tweenDirection == TweenDirection.BACKWARD)
            {
                lTDescr.setOnComplete(OnTweenComplete);
            }

        }
        private void MoveTween(Vector3 from, Vector3 to, TweenDirection tweenDirection)
        {
            LeanTween.moveLocal(gameObject, to, durationOfTween);
        }
        private void OnTweenComplete()
        {
            gameObject.SetActive(false);
        }

        private void ReverseTweenFromAndTo(ref Vector3 from, ref Vector3 to)
        {
            Vector3 temp = from;
            from = to;
            to = temp;
        }

        private enum SupportedTweenAnimationTypes
        {
            SCALE,
            FADE,
            MOVE
        }
        private enum TweenDirection
        {
            FORWARD, BACKWARD
        }

    }

}