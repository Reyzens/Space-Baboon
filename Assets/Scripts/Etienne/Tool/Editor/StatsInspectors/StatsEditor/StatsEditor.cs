using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Reflection;

namespace SpaceBaboon
{
    [CustomEditor(typeof(BaseStats<>), true)]
    [CanEditMultipleObjects]
    public class StatsEditor : Editor
    {
        [SerializeField] private VisualTreeAsset m_tree;



        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();
            root.Add(new IMGUIContainer(OnInspectorGUI));

            //var button = new Button(OnButtonClicked);
            //button.style.height = 20;
            //button.text = "test button";
            //root.Add(button);

            var feature = target as IStatsEditable;
            var so = feature.GetData();


            if (so is ScriptableObject scriptableObject)
            {
                var fields = GetFields(scriptableObject);

                foreach (var item in fields)
                {
                    if (item.FieldType == typeof(float))
                    {
                        //var button = new Button();
                        //button.style.height = 20;
                        //button.text = item.Name;
                        //root.Add(button);


                        //Instead of 
                        //m_tree.CloneTree(root);
                        VisualElement clone = m_tree.CloneTree();
                        root.Add(clone);

                        var variable = clone.Q<Label>("Name");
                        variable.text = item.Name;

                        Debug.Log(item.Name);
                    }
                }
            }




            return root;
        }

        private FieldInfo[] GetFields(ScriptableObject so)
        {
            return so.GetType().GetFields();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            //DrawDefaultInspector();   //un ou l'autre ?


            //var feature = target as IStatsEditable;
            //var so = feature.GetData();
            //
            //
            //if (so is ScriptableObject scriptableObject)
            //{
            //    var fields = GetFields(scriptableObject);
            //
            //    foreach (var item in fields)
            //    {
            //        if (item.FieldType == typeof(float))
            //        {
            //            GUILayout.Label(item.Name);
            //        }
            //    }
            //}
        }
    }
}
