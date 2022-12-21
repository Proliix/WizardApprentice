using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(RewardsHandler))]
[CanEditMultipleObjects]
public class RewardsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        RewardsHandler rewards = (RewardsHandler)target;
        EditorGUILayout.LabelField("Tier1 chance");
        EditorGUILayout.LabelField("Min:" + 0 + "                   " + "Max:" + 100);
        rewards.chanceTeir1 = EditorGUILayout.Slider(rewards.chanceTeir1, 0, 100);
        if (rewards.chanceTeir2 > 100 - rewards.chanceTeir1)
            rewards.chanceTeir2 = 100 - rewards.chanceTeir1;
        EditorGUILayout.LabelField("Tier2 chance");
        EditorGUILayout.LabelField("Min:" + 0 + "                   " + "Max:" + (100 - rewards.chanceTeir1));
        rewards.chanceTeir2 = EditorGUILayout.Slider(rewards.chanceTeir2, 0, 100 - rewards.chanceTeir1);
        if (rewards.chanceTeir3 > 100 - (rewards.chanceTeir1 + rewards.chanceTeir2))
            rewards.chanceTeir3 = 100 - (rewards.chanceTeir1 + rewards.chanceTeir2);
        EditorGUILayout.LabelField("Tier3 chance");
        rewards.chanceTeir3 = 100 - (rewards.chanceTeir1 + rewards.chanceTeir2);
        EditorGUILayout.LabelField("Chance: " + rewards.chanceTeir3);
        EditorGUILayout.Space();


        DrawDefaultInspector();
    }
}
