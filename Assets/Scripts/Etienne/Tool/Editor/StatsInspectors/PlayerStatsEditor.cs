using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using System.Reflection;
using System.Collections.Generic;
using System;

namespace SpaceBaboon
{
    [CustomEditor(typeof(TestPlayer))]
    public class PlayerStatsEditor : Editor
    {


        //public FieldInfo[] GetAllFields(Type type, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
        //{
        //    List<FieldInfo> fieldsList = new List<FieldInfo>();
        //
        //    while (type != null && type != typeof(ScriptableObject))
        //    {
        //        fieldsList.AddRange(type.GetFields(bindingFlags));
        //        type = type.BaseType;
        //    }
        //
        //    return fieldsList.ToArray();
        //}


        private float m_modifier = 0.0f;



        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUIStyle nameStyle = new GUIStyle();
            nameStyle.richText = true;
            nameStyle.normal.textColor = Color.white;
            nameStyle.fontSize = 14;
            nameStyle.fontStyle = FontStyle.Bold;



            TestPlayer player = (TestPlayer)target;

            var playerData = player.GetData();
            var fields = typeof(PlayerData).GetFields();
            //var fields = GetAllFields(playerData.GetType());

            float inspectorWidth = EditorGUIUtility.currentViewWidth;
            float halfWidth = inspectorWidth * 0.5f;

            foreach (var item in fields)
            {
                if (item.FieldType != typeof(float))
                {
                    continue;
                }
                

                var floatValue = (float)item.GetValue(playerData);

                GUILayout.Label(item.Name, nameStyle);
                GUILayout.Space(5f);

                GUILayout.BeginHorizontal();

                GUILayout.Label("Value: " + floatValue, GUILayout.MaxWidth(halfWidth));
                m_modifier = EditorGUILayout.FloatField("Modifier", m_modifier, GUILayout.MaxWidth(halfWidth));

                GUILayout.EndHorizontal();
                //GUILayout.Space(10f);

                GUILayout.BeginHorizontal();

                var buttonUP = GUILayout.Button("UP", GUILayout.MaxWidth(halfWidth));
                if (buttonUP)
                {
                    item.SetValue(playerData, floatValue + m_modifier);
                }

                var buttonDOWN = GUILayout.Button("DOWN", GUILayout.MaxWidth(halfWidth));
                if (buttonDOWN)
                {
                    item.SetValue(playerData, floatValue - m_modifier);
                }

                GUILayout.EndHorizontal();

                GUILayout.Space(10f);





            }




            /*
            GUILayout.BeginHorizontal();

            GUILayout.Label("min: ", GUILayout.MaxWidth(textWidth));
            var min = EditorGUILayout.FloatField(min, GUILayout.MaxWidth(valueWidth));
            GUILayout.Label("max: ", GUILayout.MaxWidth(textWidth));
            var max = EditorGUILayout.FloatField(max, GUILayout.MaxWidth(valueWidth));

            GUILayout.Label("min: " + min, );

            GUILayout.EndHorizontal();
            GUILayout.Space(10f);


            m_slider = GUILayout.HorizontalSlider(m_slider, min, max);
            GUILayout.Space(10f);

            Debug.Log(m_slider);
            */



            /*foreach (var item in fields)
            {
                if (item.FieldType != typeof(float))
                {
                    continue;
                }

                GUILayout.BeginHorizontal();

                float inspectorWidth = EditorGUIUtility.currentViewWidth;

                var name = item.Name + " : ";
                var value = item.GetValue(playerData);
                float floatValue = (float)value;


                GUILayout.Label(name, GUILayout.MaxWidth(inspectorWidth * 0.3f));


                var slider = GUILayout.HorizontalSlider(floatValue, floatValue - 10, floatValue + 10);
                if (slider != floatValue)
                {
                    item.SetValue(playerData, slider);

                }

                GUILayout.EndHorizontal();

                Debug.Log("Name: " + name + "   Value: " + slider);


                GUILayout.Space(10f);

                //if (GUILayout.Button(name))
                //{
                //    var data = item.GetValue(playerData);
                //
                //    float newValue = (float)data + 10;
                //
                //    item.SetValue(playerData, newValue);
                //
                //    Debug.Log("Name: " + name + "  Value: " + item.GetValue(playerData));
                //
                //}

            }*/


        }

    }
}
