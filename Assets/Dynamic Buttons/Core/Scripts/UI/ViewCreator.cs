using UnityEngine;
using UnityEngine.UI;

namespace DynamicButtons {

    public class ViewCreator : Object {

        public T InstantiateWithPrefab<T> (string name, T prefab, Transform parent) where T : Object {
            T result = Instantiate (prefab, parent, false);
            result.name = name;

            return result;
        }

        public Image InstantiateButtonImageBackground (Transform parent) {
            var image = InstantiateWithPrefab ("ImageBackground", Resources.Load<Image> ("Prefabs/ButtonImageBackground"), parent);
            image.type = Image.Type.Sliced;

            return image;
        }

        public RoundedRectangleGraphic InstantiateButtonProceduralBackground (Transform parent) {
            return InstantiateWithPrefab ("ProceduralBackground", Resources.Load<RoundedRectangleGraphic> ("Prefabs/ButtonProceduralBackground"), parent);
        }
    }
}