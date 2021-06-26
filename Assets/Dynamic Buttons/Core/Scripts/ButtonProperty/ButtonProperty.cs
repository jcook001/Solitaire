using UnityEngine;

namespace DynamicButtons {

	[System.Serializable]
	public class ButtonProperty<T> {

		[SerializeField]
		private T defaultValue;
		[SerializeField]
		private T highlightedValue;
		[SerializeField]
		private T selectedValue;
		[SerializeField]
		private T pressedValue;
		[SerializeField]
		private T disabledValue;

		[SerializeField]
		private bool highlightedEnabled;
		[SerializeField]
		private bool selectedEnabled;
		[SerializeField]
		private bool pressedEnabled;
		[SerializeField]
		private bool disabledEnabled;

		public T DefaultValue {
			get { return defaultValue; }
			set { defaultValue = value; }
		}

		public T PressedValue {
			get { return pressedValue; }
			set { pressedValue = value; }
		}

		public T HighlightedValue {
			get { return highlightedValue; }
			set { highlightedValue = value; }
		}

		public T SelectedValue {
			get { return selectedValue; }
			set { selectedValue = value; }
		}

		public T DisabledValue {
			get { return disabledValue; }
			set { disabledValue = value; }
		}

		public bool PressedEnabled {
			get { return pressedEnabled; }
			set { pressedEnabled = value; }
		}

		public bool HighlightedEnabled {
			get { return highlightedEnabled; }
			set { highlightedEnabled = value; }
		}

		public bool SelectedEnabled {
			get { return selectedEnabled; }
			set { selectedEnabled = value; }
		}

		public bool DisabledEnabled {
			get { return disabledEnabled; }
			set { disabledEnabled = value; }
		}

		public T getValue (DynamicButtonState state) {
			switch (state) {
				case DynamicButtonState.PRESSED:
					return PressedEnabled? PressedValue : DefaultValue;
				case DynamicButtonState.HIGHLIGHTED:
					return HighlightedEnabled? HighlightedValue : DefaultValue;
				case DynamicButtonState.SELECTED:
					return SelectedEnabled? SelectedValue : DefaultValue;
				case DynamicButtonState.DISABLED:
					return DisabledEnabled? DisabledValue : DefaultValue;
				default:
					return DefaultValue;
			}
		}
	}
}