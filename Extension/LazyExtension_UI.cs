using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace Threeyes.Core
{
    public static class LazyExtension_UI
    {
        static Slider.SliderEvent emptySliderEvent = new Slider.SliderEvent();
        public static void SetValueWithoutNotify(this Slider instance, float value)
        {
            var originalEvent = instance.onValueChanged;
            instance.onValueChanged = emptySliderEvent;
            instance.value = value;
            instance.onValueChanged = originalEvent;
        }

        static Button.ButtonClickedEvent emptyButtonEvent = new Button.ButtonClickedEvent();

        static Toggle.ToggleEvent emptyToggleEvent = new Toggle.ToggleEvent();

        public static void SelectWithoutNotify(this Button instance)
        {
            if (!instance)
                return;
            var originalEvent = instance.onClick;
            instance.onClick = emptyButtonEvent;
            instance.Select();
            instance.onClick = originalEvent;
        }

        /// <summary>
        /// 适用于只更改UI，不调用事件
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="value"></param>
        public static void SetValueWithoutNotify(this Toggle instance, bool value)
        {
            if (!instance)
                return;
            var originalEvent = instance.onValueChanged;
            instance.onValueChanged = emptyToggleEvent;
            instance.isOn = value;
            instance.onValueChanged = originalEvent;
        }

        static InputField.OnChangeEvent emptyInputFieldEvent = new InputField.OnChangeEvent();
        public static void SetValueWithoutNotify(this InputField instance, string value)
        {
            if (!instance)
                return;
            var originalEvent = instance.onValueChanged;
            instance.onValueChanged = emptyInputFieldEvent;
            instance.text = value;
            instance.onValueChanged = originalEvent;
        }

        static Dropdown.DropdownEvent emptyDropdownFieldEvent = new Dropdown.DropdownEvent();
        public static void SetValueWithoutNotify(this Dropdown instance, int value)
        {
            if (!instance)
                return;
            var originalEvent = instance.onValueChanged;
            instance.onValueChanged = emptyDropdownFieldEvent;
            instance.value = value;
            instance.onValueChanged = originalEvent;
        }

        #region RectTransform (Ref: https://orbcreation.com/cgi-bin/orbcreation/page.pl?1099)

        /// <summary>
        /// 获取RectTransform的尺寸，不管其是否为strength
        /// </summary>
        /// <param name="trans"></param>
        /// <returns></returns>
        public static Vector2 GetSize(this RectTransform trans)
        {
            return new Vector2(trans.rect.width, trans.rect.height);
        }
        public static float GetWidth(this RectTransform trans)
        {
            return trans.rect.width;
        }
        public static float GetHeight(this RectTransform trans)
        {
            return trans.rect.height;
        }

        /// <summary>
        /// 获取RectTransform中的某个点的位置
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="screenPoint">像素坐标，如mousePosition</param>
        /// <param name="camera">Canvas对应的相机。针对Overlay的Cavnas，camera值必须为null，否则会报错。（Warning：If you are using a canvas in 'Screen Space - Overlay' (e.g. a 2D canvas), you have to pass null as the argument for the camera to the RectTransformUtility. If you try to pass the main camera, or any other camera in the scene, in as an argument, you will get bizarre results that seem to have no bearing on reality.）</param>
        /// <returns></returns>
        public static Vector2 GetPos(this RectTransform trans, Vector2 screenPoint, Camera camera = null)
        {
            Vector2 originalLocalPointerPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(trans, screenPoint, camera, out originalLocalPointerPosition);
            return originalLocalPointerPosition;
        }


        public static void SetDefaultScale(this RectTransform trans)
        {
            trans.localScale = new Vector3(1, 1, 1);
        }
        public static void SetPivotAndAnchors(this RectTransform trans, Vector2 aVec)
        {
            trans.pivot = aVec;
            trans.anchorMin = aVec;
            trans.anchorMax = aVec;
        }
        public static void SetPositionOfPivot(this RectTransform trans, Vector2 newPos)
        {
            trans.localPosition = new Vector3(newPos.x, newPos.y, trans.localPosition.z);
        }

        public static void SetLeftBottomPosition(this RectTransform trans, Vector2 newPos)
        {
            trans.localPosition = new Vector3(newPos.x + trans.pivot.x * trans.rect.width, newPos.y + trans.pivot.y * trans.rect.height, trans.localPosition.z);
        }
        public static void SetLeftTopPosition(this RectTransform trans, Vector2 newPos)
        {
            trans.localPosition = new Vector3(newPos.x + trans.pivot.x * trans.rect.width, newPos.y - (1f - trans.pivot.y) * trans.rect.height, trans.localPosition.z);
        }
        public static void SetRightBottomPosition(this RectTransform trans, Vector2 newPos)
        {
            trans.localPosition = new Vector3(newPos.x - (1f - trans.pivot.x) * trans.rect.width, newPos.y + trans.pivot.y * trans.rect.height, trans.localPosition.z);
        }
        public static void SetRightTopPosition(this RectTransform trans, Vector2 newPos)
        {
            trans.localPosition = new Vector3(newPos.x - (1f - trans.pivot.x) * trans.rect.width, newPos.y - (1f - trans.pivot.y) * trans.rect.height, trans.localPosition.z);
        }

        public static void SetWidth(this RectTransform trans, float newSize)
        {
            trans.SetSize(new Vector2(newSize, trans.rect.size.y));
        }
        public static void SetHeight(this RectTransform trans, float newSize)
        {
            trans.SetSize(new Vector2(trans.rect.size.x, newSize));
        }
        public static void SetSize(this RectTransform trans, Vector2 newSize)
        {
            Vector2 oldSize = trans.rect.size;
            Vector2 deltaSize = newSize - oldSize;
            trans.offsetMin = trans.offsetMin - new Vector2(deltaSize.x * trans.pivot.x, deltaSize.y * trans.pivot.y);
            trans.offsetMax = trans.offsetMax + new Vector2(deltaSize.x * (1f - trans.pivot.x), deltaSize.y * (1f - trans.pivot.y));
        }
        public static void SetTop(this RectTransform trans, float value)
        {
            trans.offsetMax = new Vector2(trans.offsetMax.x, value);
        }
        public static void SetBottom(this RectTransform trans, float value)
        {
            trans.offsetMin = new Vector2(trans.offsetMin.x, value);
        }

        #endregion
    }
}