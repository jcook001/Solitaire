using UnityEngine;

namespace DynamicButtons {

    [System.Serializable]
    public class ImageProperties {
        [SerializeField]
        private ButtonSpriteProperty sprite = new ButtonSpriteProperty ();
        [SerializeField]
        private ButtonColorProperty color = ButtonColorProperty.defaultColorProperty;
        [SerializeField]
        private ButtonMaterialProperty material = new ButtonMaterialProperty ();

        public ImageProperties () { }

        public ImageProperties (Sprite defaultSprite) {
            sprite.DefaultValue = defaultSprite;
        }

        public ButtonSpriteProperty Sprite {
            get { return sprite; }
            set { sprite = value; }
        }

        public ButtonColorProperty Color {
            get { return color; }
            set { color = value; }
        }

        public ButtonMaterialProperty Material {
            get { return material; }
            set { material = value; }
        }
    }
}