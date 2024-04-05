using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using System;

namespace SpaceBaboon
{
    
    public class PlayerTool : EditorWindow
    {
        [SerializeField] private VisualTreeAsset m_tree;

        private GameObject m_player;
        private Toggle m_invincibilityToggle;

        [MenuItem("Tools/Player")]
        public static void OpenWindow()
        {
            GetWindow<PlayerTool>();
        }

        private void CreateGUI()
        {
            m_tree.CloneTree(rootVisualElement);


            m_invincibilityToggle = rootVisualElement.Q<Toggle>("InvincibilityToggle");
            m_invincibilityToggle.RegisterValueChangedCallback(OnInvincibilityToggled);

            var playerRef = rootVisualElement.Q<ObjectField>("PlayerReference");
            playerRef.RegisterValueChangedCallback(OnPlayerRefValueChanged);
        }

        private void OnInvincibilityToggled(ChangeEvent<bool> evt)
        {
            Debug.Log("event called");
            var script = m_player.GetComponent<TestPlayer>();
            
            if (script == null)
            {
                Debug.Log("script null");
                return;
            }
            script.ToggleInvincibility();
        }

        private void OnPlayerRefValueChanged(ChangeEvent<UnityEngine.Object> evt)
        {
            m_player = evt.newValue as GameObject;
        }
    }



    /*public class PlayerTool : EditorWindow
    {
        [SerializeField] private VisualTreeAsset m_tree;

        SerializedObject m_serializedObject;
        SerializedProperty m_serializedProperty;

        private GameObject m_player;
        private Toggle m_invincibilityToggle;
        private ObjectField m_playerField;

        [MenuItem("Tools/Player")]
        public static void OpenWindow()
        {
            GetWindow<PlayerTool>();
        }

        private void CreateGUI()
        {
            m_tree.CloneTree(rootVisualElement);


            m_invincibilityToggle = rootVisualElement.Q<Toggle>("InvincibilityToggle");
            m_invincibilityToggle.RegisterValueChangedCallback(OnInvincibilityToggled);

            //var playerRef = rootVisualElement.Q<ObjectField>("PlayerReference");
            //playerRef.RegisterValueChangedCallback(OnPlayerRefValueChanged);
            m_playerField = rootVisualElement.Q<ObjectField>("PlayerReference");
            m_playerField.objectType = typeof(GameObject);
            m_playerField.RegisterValueChangedCallback(OnPlayerRefValueChanged);

            if (m_player == null)
            {
                m_player = GameObject.Find("TestPlayer");
                Debug.Log("reset m_player : " + m_player.name);
                m_playerField.value = m_player;
            }

        }

        private void OnInvincibilityToggled(ChangeEvent<bool> evt)
        {
            Debug.Log("event called");
            //var script = m_player.GetComponent<TestPlayer>();
            //
            //if (script == null)
            //{
            //    Debug.Log("script null");
            //    return;
            //}
            //script.ToggleInvincibility();

            // Start watching for changes
            m_serializedObject.Update();

            // Set the property value
            m_serializedProperty.boolValue = evt.newValue;

            // Apply the changes to the serialized object
            m_serializedObject.ApplyModifiedProperties();

            // Optionally, mark the object as dirty to ensure the change is saved
            EditorUtility.SetDirty(m_serializedObject.targetObject);
        }

        private void OnGUI()
        {
            Debug.Log("onGui");
            if (m_player == null)
            {
                m_player = GameObject.Find("TestPlayer");
                Debug.Log("reset m_player : " + m_player.name);
                m_playerField.value = m_player;
            }
        }

        private void OnPlayerRefValueChanged(ChangeEvent<UnityEngine.Object> evt)
        {
            
            m_player = evt.newValue as GameObject;


            var script = m_player.GetComponent<TestPlayer>();

            m_serializedObject = new SerializedObject(script);
            m_serializedProperty = m_serializedObject.FindProperty("m_isInvincible");

            m_serializedObject.Update();
            m_invincibilityToggle.value = m_serializedProperty.boolValue;
            m_serializedObject.ApplyModifiedProperties();
        }
    }*/
}
