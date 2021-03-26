using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class GameSettingsEditor : EditorWindow
{
    public enum SettingsType
    {
        Player,
        Weapon
    }

    private SettingsType settingsType;
    private float playerSpeed;
    private float fireRate;
    private float bulletSpeed;
    private Impulse impulse;

    [MenuItem("Game Settings/Player")]
    public static void OpenPlayerSettings()
    {
        var window = (GameSettingsEditor)GetWindow(typeof(GameSettingsEditor));

        PlayerSettings settings = Resources.Load<PlayerSettings>("PlayerSettings");

        if (settings != null)
        {
            window.playerSpeed = settings.PlayerSpeed;
        }

        window.settingsType = SettingsType.Player;
        window.Show();
    }
    
    [MenuItem("Game Settings/Weapon")]
    public static void OpenWeaponSettings()
    {
        var window = (GameSettingsEditor)GetWindow(typeof(GameSettingsEditor));

        WeaponSettings settings = Resources.Load<WeaponSettings>("WeaponSettings");

        if (settings != null)
        {
            window.fireRate = settings.FireRate;
            window.bulletSpeed = settings.BulletSpeed;
            window.impulse = settings.ImpactImpulse;
        }

        window.settingsType = SettingsType.Weapon;
        window.Show();
    }

    private void OnGUI()
    {
        switch (settingsType)
        {
            case SettingsType.Player:
                DrawPlayerSettings();
                break;
            case SettingsType.Weapon:
                DrawWeaponSettings();
                break;
        }
    }

    private void DrawPlayerSettings()
    {
        GUILayout.Space(5);
        GUILayout.Label("Player Settings", EditorStyles.boldLabel);
        GUILayout.Space(5);
        EditorGUILayout.LabelField(new GUIContent("Speed:"));
        playerSpeed = EditorGUILayout.FloatField(playerSpeed);

        if (GUILayout.Button("Save"))
        {
            var settings = ScriptableObject.CreateInstance<PlayerSettings>();

            settings.PlayerSpeed = playerSpeed;
  
            SaveAsset(settings, "PlayerSettings");
        }
    }

    private void DrawWeaponSettings()
    {
        GUILayout.Space(5);
        GUILayout.Label("Weapon Settings", EditorStyles.boldLabel);
        GUILayout.Space(5);
        EditorGUILayout.LabelField(new GUIContent("Fire Rate:"));
        fireRate = EditorGUILayout.FloatField(fireRate);
        GUILayout.Space(5);
        EditorGUILayout.LabelField(new GUIContent("Bullet Speed:"));
        bulletSpeed = EditorGUILayout.FloatField(bulletSpeed);
        GUILayout.Space(5);
        EditorGUILayout.LabelField(new GUIContent("Impact Impulse:"));
        EditorGUI.indentLevel += 1;
        EditorGUILayout.LabelField(new GUIContent("Weak"));
        impulse.Weak = EditorGUILayout.FloatField(impulse.Weak);
        EditorGUILayout.LabelField(new GUIContent("Medium"));
        impulse.Medium = EditorGUILayout.FloatField(impulse.Medium);
        EditorGUILayout.LabelField(new GUIContent("Strong"));
        impulse.Strong = EditorGUILayout.FloatField(impulse.Strong);
        EditorGUI.indentLevel -= 1;

        if (GUILayout.Button("Save"))
        {
            var settings = ScriptableObject.CreateInstance<WeaponSettings>();

            settings.FireRate = fireRate;
            settings.BulletSpeed = bulletSpeed;
            settings.ImpactImpulse = impulse;

            SaveAsset(settings, "WeaponSettings");
        }
    }

    private void SaveAsset(ScriptableObject asset, string assetName)
    {
        string settingsDir = "Assets/Settings/Resources/";
        string folderDir = Application.dataPath + "/Settings/Resources/";

        if (!Directory.Exists(folderDir))
            Directory.CreateDirectory(folderDir);

        AssetDatabase.CreateAsset(asset, settingsDir + assetName + ".asset");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
