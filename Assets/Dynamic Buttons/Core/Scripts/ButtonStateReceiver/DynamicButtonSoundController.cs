using UnityEngine;

namespace DynamicButtons {

    public class DynamicButtonSoundController : MonoBehaviour {

        public AudioClip pressedAudioClip;
        public AudioClip highlightedAudioClip;

        private AudioSource audioSource;
        private DynamicButtonState lastButtonState = DynamicButtonState.NORMAL;

        private DynamicButtonBase button {
            get { return GetComponent<DynamicButtonBase> (); }
        }

        void Start () {
            setupAudioSource ();
            addButtonListeners ();
        }

        private void setupAudioSource () {
            if (audioSource == null)
                audioSource = gameObject.AddComponent<AudioSource> ();

            audioSource.playOnAwake = false;
        }

        private void addButtonListeners () {
            if (button != null) {
                button.OnClickEvent.AddListener (this.onButtonClick);
                button.OnStateChangedEvent.AddListener (this.onButtonStateChanged);
            }
        }

        private void onButtonClick () {
            audioSource.clip = pressedAudioClip;
            audioSource.Play ();
        }

        private void onButtonStateChanged (DynamicButtonState state) {
            if (state == DynamicButtonState.HIGHLIGHTED && state != lastButtonState) {
                audioSource.clip = highlightedAudioClip;
                audioSource.Play ();
            }
            lastButtonState = state;
        }

        void OnDestroy () {
            if (button != null) {
                button.OnClickEvent.RemoveListener (this.onButtonClick);
                button.OnStateChangedEvent.RemoveListener (this.onButtonStateChanged);
            }
        }
    }
}