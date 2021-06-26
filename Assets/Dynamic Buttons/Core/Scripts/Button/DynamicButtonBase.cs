 using System.Collections;
 using System;
 using UnityEditor;
 using UnityEngine.Events;
 using UnityEngine.EventSystems;
 using UnityEngine.UI;
 using UnityEngine;

 namespace DynamicButtons {

     [ExecuteInEditMode]
     public abstract class DynamicButtonBase : Selectable, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, ISubmitHandler {

         public enum DynamicButtonTransitionType { PROPERTY, ANIMATION }

         public enum DynamicButtonBackgroundType { IMAGE, PROCEDURAL }

         [Serializable]
         public class DynamicButtonClickEvent : UnityEvent { }

         [Serializable]
         public class DynamicButtonStateChangedEvent : UnityEvent<DynamicButtonState> { }

         public Image iconField;
         public Image imageBackgroundField;
         public RoundedRectangleGraphic proceduralBackgroundField;

         [SerializeField]
         private DynamicButtonTransitionType buttonTransitionType = DynamicButtonTransitionType.PROPERTY;
         [SerializeField]
         private DynamicButtonBackgroundType backgroundType = DynamicButtonBackgroundType.PROCEDURAL;
         [SerializeField]
         private TextProperties textProperties = new TextProperties ();
         [SerializeField]
         private ImageProperties iconProperties = new ImageProperties ();
         [SerializeField]
         private ImageProperties imageBackgroundProperties = new ImageProperties ();
         [SerializeField]
         private RoundedRectangleProperties proceduralBackgroundProperties = new RoundedRectangleProperties ();
         [SerializeField]
         private DynamicButtonClickEvent onClickEvent = new DynamicButtonClickEvent ();
         [SerializeField]
         private DynamicButtonStateChangedEvent onStateChangedEvent = new DynamicButtonStateChangedEvent ();

         protected TweenController tweenController;
         protected ViewCreator viewCreator = new ViewCreator ();
         protected bool isDirty = false;

         public DynamicButtonTransitionType ButtonTransitionType {
             get { return buttonTransitionType; }
             set {
                 buttonTransitionType = value;
                 isDirty = true;
             }
         }

         public DynamicButtonBackgroundType BackgroundType {
             get { return backgroundType; }
             set {
                 backgroundType = value;
                 isDirty = true;
             }
         }

         public TextProperties TextProperties {
             get { return textProperties; }
             set { textProperties = value; }
         }

         public ImageProperties IconImageProperties {
             get { return iconProperties; }
             set { iconProperties = value; }
         }

         public ImageProperties BackgroundImageProperties {
             get { return imageBackgroundProperties; }
             set { imageBackgroundProperties = value; }
         }

         public RoundedRectangleProperties BackgroundProperties {
             get { return proceduralBackgroundProperties; }
             set { proceduralBackgroundProperties = value; }
         }

         public DynamicButtonClickEvent OnClickEvent {
             get { return onClickEvent; }
             set { onClickEvent = value; }
         }

         public DynamicButtonStateChangedEvent OnStateChangedEvent {
             get { return onStateChangedEvent; }
             set { onStateChangedEvent = value; }
         }

         protected override void Awake () {
             base.Awake ();

             initProperties ();
             UpdateBackgroundObject ();
             SetupTweenController ();
         }

         private void SetupTweenController () {
             if (tweenController == null && Application.isPlaying) {
                 tweenController = gameObject.AddComponent<TweenController> ();
             }
         }

         private void initProperties () {
             if (imageBackgroundProperties.Sprite.DefaultValue == null) {
                //imageBackgroundProperties.Sprite.DefaultValue = AssetDatabase.GetBuiltinExtraResource<Sprite> ("UI/Skin/UISprite.psd");
                Debug.LogError("imageBackgroundProperties.Sprite.DefaultValue is null and the dynamic button code that was here stops the game building");
             }
         }

         private void UpdateBackgroundObject () {
             if (backgroundType == DynamicButtonBackgroundType.IMAGE) {
                 if (imageBackgroundField == null) {
                     imageBackgroundField = viewCreator.InstantiateButtonImageBackground (transform);
                     imageBackgroundField.transform.SetSiblingIndex (0);
                 }
                 if (proceduralBackgroundField != null) {
                     DestroyImmediate (proceduralBackgroundField.gameObject);
                     proceduralBackgroundField = null;
                 }
             }

             if (backgroundType == DynamicButtonBackgroundType.PROCEDURAL) {
                 if (proceduralBackgroundField == null) {
                     proceduralBackgroundField = viewCreator.InstantiateButtonProceduralBackground (transform);
                     proceduralBackgroundField.transform.SetSiblingIndex (0);
                 }
                 if (imageBackgroundField != null) {
                     DestroyImmediate (imageBackgroundField.gameObject);
                     imageBackgroundField = null;
                 }
             }
         }

         protected override void Start () {
             base.Start ();

             isDirty = true;
         }

         private SelectionState getCurrentSelectionStateFixed () {
             if (!IsInteractable ())
                 return SelectionState.Disabled;
             else
                 return currentSelectionState;
         }

         protected virtual void Update () {
             if (isDirty) {
                 UpdateBackgroundObject ();
                 UpdateSelectableTransisionType ();
                 UpdateAnimatorEnableState ();
                 TransitionButtonProperties (GetButtonState (getCurrentSelectionStateFixed()), !Application.isPlaying);
                 isDirty = false;
             }
         }

         private void UpdateSelectableTransisionType () {
             if (ButtonTransitionType == DynamicButtonTransitionType.ANIMATION) {
                 transition = Transition.Animation;
             } else {
                 transition = Transition.None;
             }
         }

         private void UpdateAnimatorEnableState () {
             if (animator == null)
                 return;

             animator.enabled = ButtonTransitionType == DynamicButtonTransitionType.ANIMATION;
         }

#if UNITY_EDITOR
         protected override void OnValidate () {
             base.OnValidate ();
             isDirty = true;
         }
#endif

         protected override void DoStateTransition (SelectionState state, bool instant) {
             if (!gameObject.activeInHierarchy)
                 return;

             DynamicButtonState buttonState = GetButtonState (state);
             if (ButtonTransitionType == DynamicButtonTransitionType.ANIMATION) {
                 base.DoStateTransition (state, instant);
             } else {
                 TransitionButtonProperties (buttonState, instant);
             }

             OnStateChangedEvent.Invoke (buttonState);
         }

         private DynamicButtonState GetButtonState (SelectionState state) {
#if UNITY_2019 || UNITY_2020
             if (state == SelectionState.Selected)
                 return DynamicButtonState.SELECTED;
#endif
             if (state == SelectionState.Disabled) {
                 return DynamicButtonState.DISABLED;
             } else if (state == SelectionState.Pressed) {
                 return DynamicButtonState.PRESSED;
             } else if (state == SelectionState.Highlighted) {
                 return DynamicButtonState.HIGHLIGHTED;
             } else {
                 return DynamicButtonState.NORMAL;
             }
         }

         protected virtual void TransitionButtonProperties (DynamicButtonState state, bool instant) {
             if (ButtonTransitionType == DynamicButtonTransitionType.PROPERTY) {
                 PropertiesExecutor.ApplyImageProperties (tweenController, instant, iconField, iconProperties, state);
                 PropertiesExecutor.ApplyImageProperties (tweenController, instant, imageBackgroundField, imageBackgroundProperties, state);
                 PropertiesExecutor.ApplyRoundedRectangleProperties (tweenController, instant, proceduralBackgroundField, proceduralBackgroundProperties, state);
             }
         }

         public void OnPointerClick (PointerEventData eventData) {
             if (!IsActive () || !IsInteractable () || eventData.button != PointerEventData.InputButton.Left) {
                 return;
             }

             Press ();
         }

         public void OnSubmit (BaseEventData eventData) {
             if (!IsActive () || !IsInteractable ()) {
                 return;
             }

             Press ();
             DoStateTransition (SelectionState.Pressed, false);
             StartCoroutine (OnFinishSubmit ());
         }

         private IEnumerator OnFinishSubmit () {
             var fadeTime = PropertiesExecutor.TweenDuration;
             var elapsedTime = 0f;

             while (elapsedTime < fadeTime) {
                 elapsedTime += Time.unscaledDeltaTime;
                 yield return null;
             }

             DoStateTransition (getCurrentSelectionStateFixed(), false);
         }

         private void Press () {
             OnClickEvent.Invoke ();
         }
     }
 }