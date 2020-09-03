using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using UnityEditor.UIElements;

[CustomEditor(typeof(Starter))]
[CanEditMultipleObjects]
public class StarterEditor : Editor
{
	private ReorderableList _managersList;

	private void OnEnable()
	{
		_managersList = new ReorderableList(serializedObject, serializedObject.FindProperty("managers"), true, true, true, true);

		_managersList.drawElementCallback = DrawManagersItems;
		_managersList.drawHeaderCallback = DrawManagersHeader;
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		serializedObject.Update();
		_managersList.DoLayoutList();
		serializedObject.ApplyModifiedProperties();
	}

	private void DrawManagersItems(Rect rect, int index, bool isActive, bool isFocused)
	{
		var element = _managersList.serializedProperty.GetArrayElementAtIndex(index);
		rect.y += 2;

		Rect propertyRect = new Rect(rect.x, rect.y, EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight);

		EditorGUI.PropertyField(propertyRect, element, GUIContent.none);
	}

	private void DrawManagersHeader(Rect rect)
	{
		EditorGUI.LabelField(rect, "Managers");
	}
}
