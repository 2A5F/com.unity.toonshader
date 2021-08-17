using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.Rendering.Toon;
namespace UnityEditor.Rendering.Toon
{
    [CustomEditor(typeof(ToonEvAdjustmentCurve))]

    public class ToonEvAdjustmentCurveCurveInspector : Editor
    {
        SerializedObject m_SerializedObject;
#if ADJUSTMENT_CURVE_DEBUG_UI
        string numberString = "1";
#endif //
        public override void OnInspectorGUI()
        {
            const string labelLightAdjustment = "Toon EV Adjustment";
            const string labelLightAdjustmentCurve = "Curve";
            const string labelLightHighCutFilter = "Light High-Cult Filter";
#if ADJUSTMENT_CURVE_DEBUG_UI
            const string labelExposureMin = "Min:";
            const string labelExposureMax = "Max:";
#endif
            bool isChanged = false;

            var obj = target as ToonEvAdjustmentCurve;
            // hi cut filter
            EditorGUI.BeginChangeCheck();

            bool lightFilterr = EditorGUILayout.Toggle(labelLightHighCutFilter, obj.m_ToonLightHiCutFilter);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Changed Light Hi Cut Filter");
                obj.m_ToonLightHiCutFilter = lightFilterr;
                isChanged = true;
            }


            if (isChanged)
            {
                // at leaset 2020.3.12f1, not neccessary. but, from which version??
                EditorApplication.QueuePlayerLoopUpdate();
            }


            // curve
            EditorGUI.BeginChangeCheck();
            bool exposureAdjustment = EditorGUILayout.Toggle(labelLightAdjustment, obj.m_ExposureAdjustmnt);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Changed Expsure Adjustment");
                obj.m_ExposureAdjustmnt = exposureAdjustment;
                isChanged = true;
            }

            EditorGUI.BeginDisabledGroup(!obj.m_ExposureAdjustmnt);
            {
                EditorGUI.indentLevel++;
                EditorGUI.BeginChangeCheck();
                //               var ranges = new Rect(-10, -10, 20, 20);
                //               var curve = EditorGUILayout.CurveField(labelExposureCurave, obj.m_AnimationCurve, Color.green,ranges);
                var curve = EditorGUILayout.CurveField(labelLightAdjustmentCurve, obj.m_AnimationCurve);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(target, "Changed Curve");
                    obj.m_AnimationCurve = curve;
                    isChanged = true;
                }
                var rangeMinLux = ConvertFromEV100(obj.m_Min);
                var rangeMaxLux = ConvertFromEV100(obj.m_Max);
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("rangeMin:" + rangeMinLux.ToString());
                EditorGUILayout.LabelField("rangeMax:" + rangeMaxLux.ToString());
                EditorGUILayout.EndHorizontal();


                EditorGUI.indentLevel--;
            }
            EditorGUI.EndDisabledGroup();





        }


        float ConvertFromEV100(float EV100)
        {

            float val = Mathf.Pow(2, EV100) * 2.5f;
            return val;

        }

        float ConvertToEV100(float val)
        {

            return Mathf.Log(val * 0.4f, 2.0f);

        }
        [MenuItem("GameObject/Toon Shader/Create Toon Ev Adjustment Curve", false, 9999)]
        static void CreateToonEvAdjustmentCurveGameObject()
        {
            var go = new GameObject();
            go.name = "Toon Ev Adjustment Curve";
            go.AddComponent<ToonEvAdjustmentCurve>();
            Undo.RegisterCreatedObjectUndo(go, "Create Toon Ev Adjustment Curve");
        }
    }
}