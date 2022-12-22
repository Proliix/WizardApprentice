using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(RewardsHandler))]
[CanEditMultipleObjects]
public class RewardsEditor : Editor
{
    int floor = 0;
    public override void OnInspectorGUI()
    {
        RewardsHandler rewards = (RewardsHandler)target;
        EditorGUILayout.LabelField("The floor to change, It starts at 0");
        floor = EditorGUILayout.IntField(floor);

        if ((rewards.chanceTeir1.Length - 1) >= floor && (rewards.chanceTeir2.Length - 1) >= floor && (rewards.chanceTeir3.Length - 1) >= floor)
        {

            EditorGUILayout.LabelField("Floor: " + (floor + 1) + " chance");
            EditorGUILayout.LabelField("Tier1 chance");
            EditorGUILayout.LabelField("Min:" + 0 + "                   " + "Max:" + 100);
            rewards.chanceTeir1[floor] = EditorGUILayout.Slider(rewards.chanceTeir1[floor], 0, 100);

            if (rewards.chanceTeir2[floor] > 100 - rewards.chanceTeir1[floor])
                rewards.chanceTeir2[floor] = 100 - rewards.chanceTeir1[floor];
            EditorGUILayout.LabelField("Tier2 chance");
            EditorGUILayout.LabelField("Min:" + 0 + "                   " + "Max:" + (100 - rewards.chanceTeir1[floor]));
            rewards.chanceTeir2[floor] = EditorGUILayout.Slider(rewards.chanceTeir2[floor], 0, 100 - rewards.chanceTeir1[floor]);
            if (rewards.chanceTeir3[floor] > 100 - (rewards.chanceTeir1[floor] + rewards.chanceTeir2[floor]))
                rewards.chanceTeir3[floor] = 100 - (rewards.chanceTeir1[floor] + rewards.chanceTeir2[floor]);
            EditorGUILayout.LabelField("Tier3 chance");
            rewards.chanceTeir3[floor] = 100 - (rewards.chanceTeir1[floor] + rewards.chanceTeir2[floor]);
            EditorGUILayout.LabelField("Chance: " + rewards.chanceTeir3[floor]);
            EditorGUILayout.Space();
        }
        else
        {
            EditorGUILayout.LabelField("Size is wrong");
            EditorGUILayout.LabelField("ChanceTier 1 size is: " + rewards.chanceTeir1.Length);
            EditorGUILayout.LabelField("ChanceTier 2 size is: " + rewards.chanceTeir2.Length);
            EditorGUILayout.LabelField("ChanceTier 3 size is: " + rewards.chanceTeir3.Length);

        }



        DrawDefaultInspector();
    }
}
