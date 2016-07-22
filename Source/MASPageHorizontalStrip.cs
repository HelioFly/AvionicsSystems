﻿/*****************************************************************************
 * The MIT License (MIT)
 * 
 * Copyright (c) 2016 MOARdV
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to
 * deal in the Software without restriction, including without limitation the
 * rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
 * sell copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
 * DEALINGS IN THE SOFTWARE.
 * 
 ****************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AvionicsSystems
{
    internal class MASPageHorizontalStrip : IMASSubComponent
    {
        private string name = "(anonymous)";
        private GameObject imageObject;
        private Material imageMaterial;
        private readonly string variableName;
        private readonly string inputName;
        private readonly float textureOffset;
        private readonly float texelWidth;
        private MASFlightComputer.Variable range1, range2;
        private readonly MASFlightComputer.Variable inputRange1, inputRange2;
        private readonly MASFlightComputer.Variable displayRange1, displayRange2;
        private readonly bool rangeMode;
        private bool currentState;

        internal MASPageHorizontalStrip(ConfigNode config, InternalProp prop, MASFlightComputer comp, MASMonitor monitor, Transform pageRoot, float depth)
        {
            if (!config.TryGetValue("name", ref name))
            {
                name = "(anonymous)";
            }

            string textureName = string.Empty;
            if (!config.TryGetValue("texture", ref textureName))
            {
                throw new ArgumentException("Unable to find 'texture' in HORIZONTAL_STRIP " + name);
            }
            Texture2D mainTexture = GameDatabase.Instance.GetTexture(textureName, false);
            if (mainTexture == null)
            {
                throw new ArgumentException("Unable to find 'texture' " + textureName + " for HORIZONTAL_STRIP " + name);
            }
            mainTexture.wrapMode = TextureWrapMode.Repeat;

            Vector2 position = Vector2.zero;
            if (!config.TryGetValue("position", ref position))
            {
                throw new ArgumentException("Unable to find 'position' in HORIZONTAL_STRIP " + name);
            }

            Vector2 size = Vector2.zero;
            if (!config.TryGetValue("size", ref size))
            {
                throw new ArgumentException("Unable to find 'size' in HORIZONTAL_STRIP " + name);
            }

            if (!config.TryGetValue("input", ref inputName))
            {
                throw new ArgumentException("Unable to find 'input' in HORIZONTAL_STRIP " + name);
            }

            string inputRange = string.Empty;
            if (!config.TryGetValue("inputRange", ref inputRange))
            {
                throw new ArgumentException("Unable to find 'inputRange' in HORIZONTAL_STRIP " + name);
            }
            string[] ranges = inputRange.Split(',');
            if (ranges.Length != 2)
            {
                throw new ArgumentException("Incorrect number of values in 'inputRange' in HORIZONTAL_STRIP " + name);
            }
            inputRange1 = comp.GetVariable(ranges[0], prop);
            inputRange2 = comp.GetVariable(ranges[1], prop);

            string displayRange = string.Empty;
            if (!config.TryGetValue("displayRange", ref displayRange))
            {
                throw new ArgumentException("Unable to find 'displayRange' in HORIZONTAL_STRIP " + name);
            }
            ranges = displayRange.Split(',');
            if (ranges.Length != 2)
            {
                throw new ArgumentException("Incorrect number of values in 'displayRange' in HORIZONTAL_STRIP " + name);
            }
            displayRange1 = comp.GetVariable(ranges[0], prop);
            displayRange2 = comp.GetVariable(ranges[1], prop);

            float displayWidth = 0.0f;
            if (!config.TryGetValue("displayWidth", ref displayWidth))
            {
                throw new ArgumentException("Unable to find 'displayWidth' in HORIZONTAL_STRIP " + name);
            }
            texelWidth = mainTexture.texelSize.x;
            float textureSpan = displayWidth * texelWidth;
            textureOffset = textureSpan * -0.5f;

            if (config.TryGetValue("variable", ref variableName))
            {
                variableName = variableName.Trim();
            }

            string range = string.Empty;
            if (config.TryGetValue("range", ref range))
            {
                ranges = range.Split(',');
                if (ranges.Length != 2)
                {
                    throw new ArgumentException("Incorrect number of values in 'range' in HORIZONTAL_STRIP " + name);
                }
                range1 = comp.GetVariable(ranges[0], prop);
                range2 = comp.GetVariable(ranges[1], prop);

                rangeMode = true;
            }
            else
            {
                rangeMode = false;
            }

            // Set up our display surface.
            imageObject = new GameObject();
            imageObject.name = pageRoot.gameObject.name + "-MASPageHorizontalStrip-" + name + "-" + depth.ToString();
            imageObject.layer = pageRoot.gameObject.layer;
            imageObject.transform.parent = pageRoot;
            imageObject.transform.position = pageRoot.position;
            imageObject.transform.Translate(monitor.screenSize.x * -0.5f + position.x, monitor.screenSize.y * 0.5f - position.y, depth);
            // add renderer stuff
            MeshFilter meshFilter = imageObject.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = imageObject.AddComponent<MeshRenderer>();
            Mesh mesh = new Mesh();
            mesh.vertices = new[]
                {
                    new Vector3(0.0f, 0.0f, depth),
                    new Vector3(size.x, 0.0f, depth),
                    new Vector3(0.0f, -size.y, depth),
                    new Vector3(size.x, -size.y, depth),
                };
            mesh.uv = new[]
                {
                    new Vector2(0.0f, 1.0f),
                    Vector2.one,
                    Vector2.zero,
                    new Vector2(1.0f, 0.0f),
                };
            mesh.triangles = new[] 
                {
                    0, 1, 2,
                    1, 3, 2
                };
            mesh.RecalculateBounds();
            mesh.Optimize();
            mesh.UploadMeshData(true);
            meshFilter.mesh = mesh;
            imageMaterial = new Material(Shader.Find("KSP/Alpha/Unlit Transparent"));
            imageMaterial.mainTexture = mainTexture;
            imageMaterial.mainTextureScale = new Vector2(textureSpan, 1.0f);
            meshRenderer.material = imageMaterial;

            comp.RegisterNumericVariable(inputName, prop, InputCallback);
            if (!string.IsNullOrEmpty(variableName))
            {
                // Disable the mesh if we're in variable mode
                imageObject.SetActive(false);
                comp.RegisterNumericVariable(variableName, prop, VariableCallback);
            }
            else
            {
                imageObject.SetActive(true);
            }
        }

        /// <summary>
        /// Update the texture offset.  We do this be inverse-lerping the
        /// input variable and lerping it into the scaled output variable.
        /// </summary>
        /// <param name="newValue"></param>
        private void InputCallback(double newValue)
        {
            float iLerp = Mathf.InverseLerp((float)inputRange1.SafeValue(), (float)inputRange2.SafeValue(), (float)newValue);
            float newCenter = Mathf.Lerp((float)displayRange1.SafeValue() * texelWidth, (float)displayRange2.SafeValue() * texelWidth, iLerp);
            imageMaterial.mainTextureOffset = new Vector2(newCenter + textureOffset, 0.0f);
        }

        /// <summary>
        /// Handle a changed value
        /// </summary>
        /// <param name="newValue"></param>
        private void VariableCallback(double newValue)
        {
            if (rangeMode)
            {
                newValue = (newValue.Between(range1.SafeValue(), range2.SafeValue())) ? 1.0 : 0.0;
            }

            bool newState = (newValue > 0.0);

            if (newState != currentState)
            {
                currentState = newState;
                imageObject.SetActive(currentState);
            }
        }

        /// <summary>
        ///  Return the name of the action.
        /// </summary>
        /// <returns></returns>
        public string Name()
        {
            return name;
        }

        /// <summary>
        /// Release resources
        /// </summary>
        public void ReleaseResources(MASFlightComputer comp, InternalProp internalProp)
        {
            UnityEngine.GameObject.Destroy(imageObject);
            imageObject = null;
            UnityEngine.GameObject.Destroy(imageMaterial);
            imageMaterial = null;

            comp.UnregisterNumericVariable(inputName, internalProp, InputCallback);
            if (!string.IsNullOrEmpty(variableName))
            {
                comp.UnregisterNumericVariable(variableName, internalProp, VariableCallback);
            }
        }
    }
}