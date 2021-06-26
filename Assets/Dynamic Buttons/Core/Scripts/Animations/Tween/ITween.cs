namespace DynamicButtons {

    public interface ITween {

        void TweenValue (float percentage);
        float Duration { get; }
    }
}