using UnityEngine.Events;

namespace DynamicButtons {

    public abstract class Tween<T> : ITween {

        private float duration;
        private T valueFrom;
        private T valueTo;
        private UnityAction<T> callback;

        public float Duration {
            get { return duration; }
            set { duration = value; }
        }

        public T ValueFrom {
            get { return valueFrom; }
            set { valueFrom = value; }
        }

        public T ValueTo {
            get { return valueTo; }
            set { valueTo = value; }
        }

        public UnityAction<T> Callback {
            get { return callback; }
            set { callback = value; }
        }

        protected abstract T CalculateValue (float percentage);

        public void TweenValue (float percentage) {
            callback.Invoke (CalculateValue (percentage));
        }
    }
}