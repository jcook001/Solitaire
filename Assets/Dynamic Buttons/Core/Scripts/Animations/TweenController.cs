using System.Collections.Generic;
using UnityEngine;

namespace DynamicButtons {

    internal class TweenHolder {

        private ITween tween;
        private float timeElapsed = 0;

        public TweenHolder (ITween tween) {
            this.tween = tween;
        }

        public void Update (float deltaTime) {
            timeElapsed += deltaTime;
            float percentageValue = Mathf.Clamp01 (timeElapsed / tween.Duration);
            tween.TweenValue (percentageValue);
        }

        public bool IsFinished () {
            return tween == null || timeElapsed > tween.Duration;
        }
    }

    public class TweenController : MonoBehaviour {

        private List<TweenHolder> tweenHolders = new List<TweenHolder> ();

        public void startTween (ITween tween) {
            tweenHolders.Add (new TweenHolder (tween));
        }

        public void stopAllTweens () {
            tweenHolders.Clear ();
        }

        void Update () {
            if (tweenHolders.Count == 0)
                return;

            List<TweenHolder> finishedTweens = new List<TweenHolder> ();

            foreach (TweenHolder holder in tweenHolders) {
                if (holder.IsFinished ()) {
                    finishedTweens.Add (holder);
                } else {
                    holder.Update (Time.deltaTime);
                }
            }
            tweenHolders.RemoveAll (finishedTweens.Contains);
        }
    }
}