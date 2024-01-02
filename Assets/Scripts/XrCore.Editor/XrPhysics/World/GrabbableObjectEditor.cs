using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Linq;
using XrCore.XrPhysics.Interaction;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

namespace XrCore.XrPhysics.World.Editor
{
    [CustomEditor(typeof(GrabbableObject))]
    public class GrabbableObjectEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();
            var grabbableObject = target as GrabbableObject;
            root.Add(new PropertyField { label = "PhysicsConfig", bindingPath = "physicsSettings"});

            var grabPointButton = new Button { text = "Add Grab Point" };
            grabPointButton.clicked += AddGrabPoint;
            root.Add(grabPointButton);

            if(grabbableObject.grabPoints != null)
            {
                root.Add(new PropertyField(serializedObject.FindProperty("grabPoints")));
            }
            return root;
        }

        void AddGrabPoint()
        {
            var grabbableObject = target as GrabbableObject;
            if(grabbableObject.grabPoints == null)
            {
                grabbableObject.grabPoints = new GrabPoint[0];
            }
            var asList = grabbableObject.grabPoints.ToList<GrabPoint>();

            var newInstance = ObjectFactory.CreateGameObject($"GrabPoint_{target.name}_{grabbableObject.grabPoints.Length + 1}");
            newInstance.transform.SetParent(grabbableObject.transform, false);
            newInstance.transform.localPosition = Vector3.zero;
            var grabPoint = newInstance.AddComponent<GrabPoint>();

            asList.Add(grabPoint);
            
            grabbableObject.grabPoints = asList.ToArray();

            EditorUtility.SetDirty(grabbableObject);
            AssetDatabase.SaveAssets();
            EditorSceneManager.SaveOpenScenes();
        }
    }
}
