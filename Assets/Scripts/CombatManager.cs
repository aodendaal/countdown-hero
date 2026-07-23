using System.Collections;
using QFSW.QC;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [Header("Participants")]
    [SerializeField] private Transform heroTransform;
    [SerializeField] private Transform monsterTransform;

    [Header("Positions & Offsets")]
    [SerializeField] private float centerPointX = 0f;      // Center of screen
    [SerializeField] private float hitDistanceOffset = 0.5f; // Distance kept between them on hit
    [SerializeField] private float offscreenExitX = 8f;     // Where hero runs off after winning

    [Header("Combat Settings")]
    [SerializeField] private float moveSpeed = 12f;
    [SerializeField] private float knockbackDistance = 1.2f;
    [SerializeField] private float knockbackDuration = 0.08f;

    [Header("Stats (Temporary for Demo)")]
    public int heroHP = 100;
    public int heroDamage = 25;

    private MonsterData currentMonsterData;
    private int currentMonsterHP;

    public void SetupAndStartBattle(MonsterData data)
    {
        currentMonsterData = data;
        currentMonsterHP = data.maxHP;

        // Add monster prefab
        var monster = Instantiate(data.monsterObject, new Vector3(8f, -4.5f, 0f), Quaternion.identity);
        monster.transform.SetParent(this.transform);
        monsterTransform = monster.transform;

        // Make sure monster GameObject is visible/active in combat stage
        monsterTransform.gameObject.SetActive(true);

        // Set starting positions for hero and monster
        heroTransform.position = new Vector3(-8f, -4.5f, 0f);

        // Start the battle sequence!
        StartCoroutine(BattleRoutine());
    }

    private IEnumerator BattleRoutine()
    {
        bool battleOver = false;

        while (!battleOver)
        {
            // 1. APPROACH: Move toward each other until reaching the hit offsets
            Debug.Log("1. APPROACH: Move toward each other until reaching the hit offsets");
            Vector3 heroTarget = new Vector3(centerPointX - hitDistanceOffset, heroTransform.position.y, heroTransform.position.z);
            Vector3 monsterTarget = new Vector3(centerPointX + hitDistanceOffset, monsterTransform.position.y, monsterTransform.position.z);

            while (Vector3.Distance(heroTransform.position, heroTarget) > 0.01f ||
                   Vector3.Distance(monsterTransform.position, monsterTarget) > 0.01f)
            {
                heroTransform.position = Vector3.MoveTowards(heroTransform.position, heroTarget, moveSpeed * Time.deltaTime);
                monsterTransform.position = Vector3.MoveTowards(monsterTransform.position, monsterTarget, moveSpeed * Time.deltaTime);
                yield return null;
            }

            // 2. IMPACT & DAMAGE
            Debug.Log("2. IMPACT & DAMAGE");
            heroHP -= currentMonsterData.attackDamage;
            currentMonsterHP -= heroDamage;

            // TODO: Trigger sound effect, sprite flash, or camera shake here!
            Debug.Log($"BAM! Hero HP: {heroHP} | Monster HP: {currentMonsterHP}");

            // Check Win/Loss conditions
            if (currentMonsterHP <= 0 || heroHP <= 0)
            {
                battleOver = true;
                break;
            }

            // 3. KNOCKBACK: Slide both actors backward
            Debug.Log("3. KNOCKBACK: Slide both actors backward");
            yield return StartCoroutine(ApplyKnockback());
        }

        // 4. RESOLUTION
        Debug.Log("4. RESOLUTION");
        if (currentMonsterHP <= 0)
        {
            // Disable or fade out monster sprite
            Destroy(monsterTransform.gameObject);
            //monsterTransform.gameObject.SetActive(false);

            // Hero runs off the right side of the screen
            Vector3 exitTarget = new Vector3(offscreenExitX, heroTransform.position.y, heroTransform.position.z);
            while (Vector3.Distance(heroTransform.position, exitTarget) > 0.01f)
            {
                heroTransform.position = Vector3.MoveTowards(heroTransform.position, exitTarget, moveSpeed * Time.deltaTime);
                yield return null;
            }

            Debug.Log("Hero exited battle standard view!");
            GameStateManager.Instance.EndCombat(true);
        }
        else if (heroHP <= 0)
        {
            Debug.Log("Hero died!");
            // Handle Hero defeat (Respawn at Goddess / Town)
            GameStateManager.Instance.EndCombat(false);

        }
    }

    private IEnumerator ApplyKnockback()
    {
        Vector3 heroStart = heroTransform.position;
        Vector3 monsterStart = monsterTransform.position;

        Vector3 heroEnd = heroStart + Vector3.left * knockbackDistance;
        Vector3 monsterEnd = monsterStart + Vector3.right * knockbackDistance;

        float elapsed = 0f;
        while (elapsed < knockbackDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / knockbackDuration;

            // Simple ease-out step for snappy knockback feeling
            heroTransform.position = Vector3.Lerp(heroStart, heroEnd, t);
            monsterTransform.position = Vector3.Lerp(monsterStart, monsterEnd, t);
            yield return null;
        }
    }
}