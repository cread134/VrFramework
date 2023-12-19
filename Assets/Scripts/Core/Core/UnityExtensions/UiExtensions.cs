using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Core.UnityExtensions
{
    public static class UiExtensions
    {
        public static void AddHeader(this VisualElement element, string text)
        {
            var headerLabel = new Label(text);
            headerLabel.style.fontSize = 18;
            element.Add(headerLabel);
        }
    }
}
