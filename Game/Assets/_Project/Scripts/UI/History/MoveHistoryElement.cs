using _Project.Scripts.UI.Elements.Lists;
using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.History
{
    public class MoveHistoryElement : MonoBehaviour
    {
        public Button Button { get; private set; }
        
        public NumberedElement NumberedElement { get; private set; }
        
        public Tween CreationTween { get; set; }

        public void Construct(Button button)
        {
            NumberedElement = GetComponent<NumberedElement>();
            NumberedElement.Construct();
            Button = button;
        }
    }
}