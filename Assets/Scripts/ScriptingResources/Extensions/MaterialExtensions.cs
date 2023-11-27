using System;
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

        public static MaterialPropertyBlock SetPropertyBlock(this Renderer renderer, string property, object value, MaterialPropertyBlock? block = null)
        {
            var propertyBlock = block ?? new MaterialPropertyBlock();
            switch (value)
            {
                case Color color:
                    propertyBlock.SetColor(property, color);
                    break;
                case float f:
                    propertyBlock.SetFloat(property, f);
                    break;
                case int i:
                    propertyBlock.SetInt(property, i);
                    break;
                case ComputeBuffer buffer:
                    propertyBlock.SetBuffer(property, buffer);
                    break;
            }
            renderer.SetPropertyBlock(propertyBlock);
            return propertyBlock;
        }
    }
}