using UnityEngine;
using System.Collections.Generic;

namespace Data.Enemy
{
    /// <summary>敵のスキルデータを管理するクラス</summary>
    public class EnemySkillDatabase : MonoBehaviour
    {
        public static EnemySkillDatabase Instance { get; private set; }

        [Header("スキルデータのリスト"), SerializeField] private List<EnemySkillData> skills;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
            
            skills.Sort((a, b) => a.skillNumber.CompareTo(b.skillNumber));
        }

        /// <summary>スキル番号を指定してデータを取得する</summary>
        /// <param name="skillNumber">1から始まるスキル番号</param>
        public EnemySkillData GetSkillData(int? skillNumber)
        {
            if (skillNumber >= 1 && skillNumber <= skills.Count)
            {
                return skills[skillNumber.Value - 1];
            }

            return null;
        }
    }
}