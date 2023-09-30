using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace XrCore.XrPhysics.Hands.Posing
{
    public class XrPoseHelper
    {
        public static void LoadPosesAsync(Action<List<PoseObject>> actionCallback)
        {
            LoadHandPoses(actionCallback);
        }

        static IEnumerator LoadHandPoses(Action<List<PoseObject>> actionCallback)
        {
            var poses = new List<PoseObject>();
            var loadHandle = Addressables.LoadAssetsAsync<PoseObject>(
                    "HandPoses",
                    addressable =>
                    {
                        //Gets called for every loaded asset
                        poses.Add(addressable);
                    },
                    false); // Whether to fail and release if any asset fails to load

            yield return loadHandle;

            actionCallback.Invoke(poses);
        }
    }
}
