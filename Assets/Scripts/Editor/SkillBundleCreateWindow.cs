using Common.Attacker;
using Particle;
using UnityEngine;
using UnityEditor;

namespace Editor
{
    /// <summary></summary>
    public class SkillBundleCreateWindow : EditorWindow
    {
        /// <summary>スキルのパーティクル</summary>
        [SerializeField] private GameObject[] particlePrefabs;
        
        /// <summary>使用者の種類</summary>
        private CasterType _casterType;

        /// <summary>コライダーの形状</summary>
        private ColliderShape _colliderShape;
        
        private SerializedObject _serializedObject;
        private SerializedProperty _particlesProperty;

        [MenuItem("Tools/Skill Bundle Creator")]
        public static void ShowWindow()
        {
            GetWindow<SkillBundleCreateWindow>("Skill Bundle Creator");
        }

        private void OnEnable()
        {
            _serializedObject = new SerializedObject(this);
            _particlesProperty = _serializedObject.FindProperty("particlePrefabs");
        }

        private void OnGUI()
        {
            // 見出し
            GUILayout.Space(10);
            GUILayout.Label("Skill Bundle Create Settings", EditorStyles.largeLabel);
            
            // 使用者の設定
            GUILayout.Space(10);
            _casterType = (CasterType)EditorGUILayout.EnumPopup("Caster Type", _casterType);
            
            // コライダーの設定
            GUILayout.Space(15);
            _colliderShape = (ColliderShape)EditorGUILayout.EnumPopup("Collider Shape", _colliderShape);
            
            // パーティクルの設定
            GUILayout.Space(15);
            EditorGUILayout.PropertyField(_particlesProperty, new GUIContent("Particle Prefabs"), true);
            
            // ボタン
            GUILayout.Space(15);
            if (GUILayout.Button("Create Skill Bundle"))
            {
                _serializedObject.ApplyModifiedProperties();
                CreateSkillBundle();
            }
        }

        /// <summary>スキルのバンドルを作成する</summary>
        private void CreateSkillBundle()
        {
            // パーティクルの確認
            if (particlePrefabs is not { Length: > 0 })
            {
                Debug.LogWarning("At least one prefab must be assigned.");
                return;
            }
            
            // 親オブジェクトを作成
            var parent = new GameObject("Skill Bundle");
            
            // 子オブジェクト（攻撃バンドル）を作成
            var attackBundle = new GameObject("Attack Bundle");
            attackBundle.transform.SetParent(parent.transform);
            
            // 子オブジェクト（パーティクル）を作成
            foreach (var particlePrefab in particlePrefabs)
            {
                _ = PrefabUtility.InstantiatePrefab(particlePrefab, parent.transform) as GameObject;
            }
            
            // 親オブジェクトにパーティクル制御クラスを追加
            AddParticleControllerComponent(parent);
            
            // 子オブジェクトに攻撃クラスを追加
            AddAttackerComponent(attackBundle);
            
            // 子オブジェクトにコライダーを追加
            AddColliderComponent(attackBundle);
        }

        /// <summary>パーティクルを制御するクラスをアタッチする</summary>
        private void AddParticleControllerComponent(GameObject obj)
        {
            switch (_casterType)
            {
                case CasterType.Player:
                    obj.AddComponent<PlayerParticleController>(); break;
                case CasterType.Enemy:
                    obj.AddComponent<EnemyParticleController>(); break;
            }
        }

        /// <summary>攻撃クラスをアタッチする</summary>
        private void AddAttackerComponent(GameObject obj)
        {
            switch (_casterType)
            {
                case CasterType.Player:
                    obj.AddComponent<PlayerAttacker>(); break;
                case CasterType.Enemy: 
                    obj.AddComponent<EnemyAttacker>(); break;
            }
        }

        /// <summary>コライダーをアタッチする</summary>
        private void AddColliderComponent(GameObject obj)
        {
            switch (_colliderShape)
            {
                case ColliderShape.Box:
                    obj.AddComponent<BoxCollider>(); break;
                case ColliderShape.Sphere:
                    obj.AddComponent<SphereCollider>(); break;
                case ColliderShape.Capsule:
                    obj.AddComponent<CapsuleCollider>(); break;
            }
        }
        
        /// <summary>使用者の種類</summary>
        private enum CasterType
        {
            Player,
            Enemy
        }

        /// <summary>コライダーの形状</summary>
        private enum ColliderShape
        {
            Box,
            Sphere,
            Capsule
        }
    }
}