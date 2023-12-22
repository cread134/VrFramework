using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Core.Extensions
{
    public static class UiExtensions
    {
        public static void AddHeader(this VisualElement element, string text)
        {
            var headerLabel = new Label(text);
            headerLabel.style.fontSize = 18;
            element.Add(headerLabel);
        }

        public static void AddButton(this VisualElement element, string text, Action action)
        {
            var button = new Button();
            button.text = text;
            button.clicked += action;
            element.Add(button);
        }
    }
}
