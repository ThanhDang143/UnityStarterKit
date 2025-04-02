#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class ConditionalHideAttribute : PropertyAttribute
{
    public string ConditionalSourceField = "";
    public bool HideInInspector = false;
    public object ConditionalSourceValue = null;

    public ConditionalHideAttribute(string conditionalSourceField, bool hideInInspector)
    {
        this.ConditionalSourceField = conditionalSourceField;
        this.HideInInspector = hideInInspector;
    }

    public ConditionalHideAttribute(string conditionalSourceField, object conditionalSourceValue)
    {
        this.ConditionalSourceField = conditionalSourceField;
        this.ConditionalSourceValue = conditionalSourceValue;
    }
}

[CustomPropertyDrawer(typeof(ConditionalHideAttribute))]
public class ConditionalHidePropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ConditionalHideAttribute condHAtt = (ConditionalHideAttribute)attribute;
        bool enabled = GetConditionalHideAttributeResult(condHAtt, property);

        if (enabled)
        {
            EditorGUI.PropertyField(position, property, label, true);
        }
        else if (!condHAtt.HideInInspector)
        {
            EditorGUI.LabelField(position, label.text, " ");
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        ConditionalHideAttribute condHAtt = (ConditionalHideAttribute)attribute;
        bool enabled = GetConditionalHideAttributeResult(condHAtt, property);

        if (enabled)
        {
            return EditorGUI.GetPropertyHeight(property, label);
        }
        else
        {
            // The property is not being drawn, so no height is needed
            return -EditorGUIUtility.standardVerticalSpacing;
        }
    }

    private bool GetConditionalHideAttributeResult(ConditionalHideAttribute condHAtt, SerializedProperty property)
    {
        bool enabled = true;
        string propertyPath = property.propertyPath; //returns the property path of the current element
        string conditionPath = propertyPath.Replace(property.name, condHAtt.ConditionalSourceField); //changes the path to the conditionalsourceproperty path
        SerializedProperty sourcePropertyValue = property.serializedObject.FindProperty(conditionPath);

        if (sourcePropertyValue != null)
        {
            if (condHAtt.ConditionalSourceValue != null)
            {
                enabled = sourcePropertyValue.propertyType switch
                {
                    SerializedPropertyType.Integer => sourcePropertyValue.intValue.Equals(condHAtt.ConditionalSourceValue),
                    SerializedPropertyType.Boolean => sourcePropertyValue.boolValue.Equals(condHAtt.ConditionalSourceValue),
                    SerializedPropertyType.Float => sourcePropertyValue.floatValue.Equals(condHAtt.ConditionalSourceValue),
                    SerializedPropertyType.String => sourcePropertyValue.stringValue.Equals(condHAtt.ConditionalSourceValue),
                    SerializedPropertyType.Enum => sourcePropertyValue.enumValueIndex.Equals((int)condHAtt.ConditionalSourceValue),
                    _ => true,
                };
            }
            else
            {
                enabled = sourcePropertyValue.boolValue;
            }
        }
        else
        {
            Debug.LogWarning("Attempting to use a conditional hide attribute but no matching SourcePropertyValue found in object: " + condHAtt.ConditionalSourceField);
        }

        return enabled;
    }
}
#endif