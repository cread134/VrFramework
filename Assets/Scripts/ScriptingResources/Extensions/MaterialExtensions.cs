using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptingResources.Extensions
{
    public static class MaterialExtensions
    {
        public static void SetColor(this Renderer renderer, Color color)
        {
            var propertyBlock = new MaterialPropertyBlock();
            propertyBlock.SetColor("_Color", color);
            renderer.SetPropertyBlock(propertyBlock);
        }
    }
}