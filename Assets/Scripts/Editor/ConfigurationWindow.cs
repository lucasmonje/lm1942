using System;
using System.Collections.Generic;
using System.Linq;
using Core.Configurations;
using Core.Domain.Entities;
using Core.Domain.Models;
using Core.Infrastructure.Factories;
using Core.Utils;
using Core.Utils.Extensions;
using UniRx;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Editor
{
    internal class Tab
    {
        public string TabName { get; private set; }
        public Action OnPress { get; private set; }
        public Vector2 ScrollPosition { get; set; }

        public Tab(string tabName, Action onPress)
        {
            TabName = tabName;
            OnPress = onPress;
            ScrollPosition = new Vector2();
        }
    }

    public sealed class ConfigurationWindow : EditorWindow
    {
        private Tab[] mainTabs;
        private int editorMode;
        private int levelsGrid;

        [MenuItem("Game/Configure")]
        private static void Init()
        {
            var window = (ConfigurationWindow) GetWindow(typeof(ConfigurationWindow), true, "Game Configuration");
            window.Initialize();
            window.Show();
        }

        private void Initialize()
        {
            ConfigurationLoader.Reload();
        }

        private Tab[] GetMainTabs()
        {
            return mainTabs ?? (mainTabs = new[]
            {
                new Tab("Player", DrawPlayerTab),
                new Tab("Enemies", DrawEnemiesTab),
                new Tab("Guns", DrawGunsTabs),
                new Tab("PowerUps", DrawPowerUpsTabs),
                new Tab("Paths", DrawPathsTabs),
                new Tab("Define Levels", DrawLevelsTabs)
            });
        }

        private void OnGUI()
        {
            GUILayout.Space(3);

            var oldValue = GUI.skin.window.padding.bottom;
            GUI.skin.window.padding.bottom = -20;
            var windowRect = GUILayoutUtility.GetRect(1, 17);
            windowRect.x += 4;
            windowRect.width -= 7;
            editorMode = GUI.SelectionGrid(windowRect, editorMode, GetMainTabs().Select(t => t.TabName).ToArray(),
                mainTabs.Length, "Window");
            GUI.skin.window.padding.bottom = oldValue;
            GUILayout.Space(5);

            var tab = GetMainTabs().ElementAt(editorMode);
            tab.ScrollPosition = GUILayout.BeginScrollView(tab.ScrollPosition);
            tab.OnPress();
            GUILayout.EndScrollView();

            DrawButtonApply();
        }

        private void DrawPlayerTab()
        {
            GUILayout.Space(10);
            var playerDefinition = GameProvider.ProvideConfiguration().Player;
            if (playerDefinition == null)
                playerDefinition = (GameProvider.ProvideConfiguration().Player = CreateInstance<PlayerDefinition>());
            var serializedObject = new SerializedObject(playerDefinition);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Speed"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Lives"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Health"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Prefab"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("FireRate"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("LimitLeft"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("LimitRight"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("LimitTop"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("LimitBottom"), true);
            serializedObject.ApplyModifiedProperties();
            DrawHorizontalSlider();
            GUILayout.Label("Gun", EditorStyles.boldLabel);
            if (playerDefinition.GunDefinition == null)
                playerDefinition.GunDefinition = CreateInstance<GunDefinition>();
            DrawGun(playerDefinition.GunDefinition);
        }

        private void DrawEnemiesTab()
        {
            GUILayout.Space(10);
            var levelsConfig = GameProvider.ProvideConfiguration();
            levelsConfig.AllEnemies.ForEach(dto =>
            {
                if (dto == null) return;
                DrawEnemy(dto);
                DrawHorizontalSlider();
            });
            SplitAction("Add new enemy", () => levelsConfig.AllEnemies.Add(CreateInstance<EnemyDefinition>()));
        }

        private void DrawGunsTabs()
        {
            GUILayout.Space(10);
            var levelsConfig = GameProvider.ProvideConfiguration();
            levelsConfig.AllGuns.ForEach(dto =>
            {
                if (dto == null) return;
                DrawGun(dto);
                DrawHorizontalSlider();
            });
            SplitAction("Add new Gun", () => levelsConfig.AllGuns.Add(CreateInstance<GunDefinition>()));
        }

        private void DrawPowerUpsTabs()
        {
            GUILayout.Space(10);
            var levelsConfig = GameProvider.ProvideConfiguration();
            levelsConfig.AllPowerUps.ForEach(dto =>
            {
                if (dto == null) return;
                DrawPowerUp(dto);
                DrawHorizontalSlider();
            });
            SplitAction("Add new PowerUp", () => levelsConfig.AllPowerUps.Add(CreateInstance<PowerUpDefinition>()));
        }

        private void DrawPathsTabs()
        {
            GUILayout.Space(10);
            var levelsConfig = GameProvider.ProvideConfiguration();
            levelsConfig.AllPaths.ForEach(dto =>
            {
                if (dto == null) return;
                DrawPath(dto);
                DrawHorizontalSlider();
            });
            SplitAction("Add new Path", () => levelsConfig.AllPaths.Add(CreateInstance<PathDefinition>()));
        }

        private void DrawLevelsTabs()
        {
            GUILayout.Space(10);
            var levelsConfig = GameProvider.ProvideConfiguration();
            
            var windowRect = GUILayoutUtility.GetRect(1, 17);
            windowRect.x += 4;
            windowRect.width -= 7;
            
            var levelTabs = new List<Tab>();
            var i = 0;
            while (i < levelsConfig.AllLevels.Count)
            {
                var dto = levelsConfig.AllLevels.ElementAt(i);
                levelTabs.Add(new Tab((i + 1).ToString(), () => DrawLevel(dto)));
                i++;
            }
            levelTabs.Add(new Tab("New Level", () => levelsConfig.AllLevels.Add(CreateInstance<LevelDefinition>())));

            levelsGrid = GUI.SelectionGrid(windowRect, levelsGrid, levelTabs.Select(t => t.TabName).ToArray(),
                levelTabs.Count);
            GUILayout.Space(5);
            levelTabs[levelsGrid].OnPress();
        }

        private static void DrawHorizontalSlider()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            EditorGUILayout.EndHorizontal();
        }

        private void DrawButtonApply()
        {
            GUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Reload File"))
            {
                ConfigurationLoader.Reload();
            }

            if (GUILayout.Button("Apply"))
            {
                Save();
            }

            EditorGUILayout.EndHorizontal();

            GUILayout.Space(10);
        }

        private static void SplitAction(string label, Action action)
        {
            GUILayout.Space(5);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider, GUILayout.MinWidth(10));

            if (GUILayout.Button(label, GUILayout.MaxWidth(150)))
                action.Invoke();

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider, GUILayout.MinWidth(10));
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(10);
        }

        private void DrawLevel(LevelDefinition levelDefinition)
        {
            var levelSerializedObject = new SerializedObject(levelDefinition);
            EditorGUILayout.PropertyField(levelSerializedObject.FindProperty("PowerUps"), true);
            EditorGUILayout.PropertyField(levelSerializedObject.FindProperty("Rounds"), true);
            levelSerializedObject.ApplyModifiedProperties();
            DrawHorizontalSlider();

            while (levelDefinition.EnemiesList.Count < levelDefinition.Rounds)
                levelDefinition.EnemiesList.Add(CreateInstance<EnemiesDefinition>());

            levelDefinition.EnemiesList.Resize(levelDefinition.Rounds, CreateInstance<EnemiesDefinition>);

            foreach (var enemiesDefinition in levelDefinition.EnemiesList)
            {
                var serializedObject = new SerializedObject(enemiesDefinition);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("EnemyName"), true);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("Count"), true);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("Delay"), true);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("InstantiationInterval"), true);
                serializedObject.ApplyModifiedProperties();
            }
        }

        private void DrawEnemy(EnemyDefinition enemyDefinition)
        {
            var serializedObject = new SerializedObject(enemyDefinition);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Name"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Health"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("GunName"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Speed"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Prefab"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("PathName"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("ScorePoints"), true);
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawGun(GunDefinition gunDefinition)
        {
            var serializedObject = new SerializedObject(gunDefinition);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Name"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Damage"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("AmountBullets"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Prefab"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("ToPosition"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Speed"), true);
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawPowerUp(PowerUpDefinition powerUpDefinition)
        {
            var serializedObject = new SerializedObject(powerUpDefinition);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Name"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Prefab"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("FromPosition"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("ToPosition"), true);
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawPath(PathDefinition pathDefinition)
        {
            var serializedObject = new SerializedObject(pathDefinition);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Name"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("SpawnPosition"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("WayPoints"), true);
            serializedObject.ApplyModifiedProperties();
        }

        private static void Save()
        {
            var levelsConfig = GameProvider.ProvideConfiguration();
            var levelsDto = levelsConfig.ToDto(AssetDatabase.GetAssetPath);
            var json = JsonUtility.ToJson(levelsDto, true);
            IOUtils.Save(ConfigurationLoader.Path, json);
            AssetDatabase.Refresh();
        }
    }
}