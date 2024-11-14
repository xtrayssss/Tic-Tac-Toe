using UnityEngine;

namespace _Project.Scripts.UI.Elements.ScrollSnap
{
    public class ScrollSnapElement : MonoBehaviour
    {
        public RectTransform RectTransform { get; private set; }
        public float Position { get; set; }

        public void Construct(float position)
        {
            Position = position;
            RectTransform = GetComponent<RectTransform>();
        }
    }
}