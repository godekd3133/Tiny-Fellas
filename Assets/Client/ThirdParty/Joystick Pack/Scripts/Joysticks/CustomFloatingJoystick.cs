using UnityEngine;
using UnityEngine.EventSystems;

namespace ThirdParty.Joystick_Pack.Scripts.Joysticks
{
    public class CustomFloatingJoystick : Joystick
    {
        [SerializeField] private Transform _initializePosition;

        protected override void Start()
        {
            base.Start();
            background.anchoredPosition = _initializePosition.localPosition;
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
            base.OnPointerDown(eventData);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            background.anchoredPosition = _initializePosition.localPosition;
        }
    }
}