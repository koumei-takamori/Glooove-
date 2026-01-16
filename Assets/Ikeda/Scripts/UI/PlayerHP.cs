// ------------------------------------------------
// PlayerHP.cs
// プレイヤーのHP管理
// ダメージ処理 / HPゲージUI連携
// Player へのアタッチを想定
// 2026/01/07
// 池田桜輔
// ------------------------------------------------

using Nakashi.Player;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class PlayerHP : MonoBehaviour
{
    // ================= 定数 =================
    private const string HpGaugeCanvas3DName = "HPGaugeCanvas-3D";
    private const string HpGaugeSystemOwnerName = "HPGaugeGroup";
    private const string UiCanvas2DName = "UICanvas";

    private const string Player1UILayer = "Player1-UI";
    private const string Player2UILayer = "Player2-UI";

    // ================= 変数 =================
    [SerializeField] private int currentHP;
    [SerializeField] private UIController uiController;

    [SerializeField] private ArmPlayerController armPlayerController;
    [SerializeField] private ArmPlayerData armPlayerData;

    [SerializeField] private GameObject hpGaugeCanvas3D;
    [SerializeField] private HPGaugeSystem[] hpGaugeSystems = new HPGaugeSystem[2]; // 0=2D / 1=3D

    private int playerNumber; // 0 or 1


    public int GetCurrentHP() { return currentHP; }

    // ================= Unity =================
    private void Start()
    {
        StartCoroutine(InitializeCoroutine());
    }

    // ================= 初期化 =================
    private IEnumerator InitializeCoroutine()
    {
        // PlayerRegistry 登録待ち
        yield return WaitUntilPlayerRegistered();

        // 参照取得
        if (!TrySetupReferences())
        {
            Debug.LogError("PlayerHP 初期化失敗");
            yield break;
        }

        InitializeHP(playerNumber);
        ApplyPlayer3DHPGaugeLayer();

        Debug.Log($"PlayerHP 初期化完了 : Player {playerNumber}");
    }

    private IEnumerator WaitUntilPlayerRegistered()
    {
        yield return new WaitUntil(() =>
            PlayerRegistry.Instance != null &&
            PlayerRegistry.Instance.GetAllPlayers().Contains(gameObject)
        );
    }

    // ================= 参照取得 =================
    private bool TrySetupReferences()
    {
        // ---- PlayerController ----
        armPlayerController = GetComponent<ArmPlayerController>();
        if (armPlayerController == null)
        {
            Debug.LogError("ArmPlayerController が存在しません");
            return false;
        }

        armPlayerData = armPlayerController.GetPlayerData();
        if (armPlayerData == null)
        {
            Debug.LogError("ArmPlayerData の取得に失敗しました");
            return false;
        }

        // ---- Player番号 ----
        playerNumber = armPlayerController.PlayerId;

        // ---- 3D HP Canvas ----
        if (!TryGetChildObject(transform, HpGaugeCanvas3DName, out hpGaugeCanvas3D))
            return false;

        // ---- 2D HP Gauge ----
        string hp2DPath = $"HPGaugeGroup-{playerNumber + 1}P";

        if (!TryGetHPGauge2D(hp2DPath, out hpGaugeSystems[0]))
            return false;

        // ---- 3D HP Gauge ----
        if (!TryGetHPGauge3D(out hpGaugeSystems[1]))
            return false;

        // --- UIController ---
        if (uiController == null)
        {
            GameObject uiControllerObj = GameObject.Find("UIController");
            if (uiControllerObj == null)
            {
                Debug.LogError("UIController オブジェクトが存在しません");
                return false;
            }

            uiController = uiControllerObj.GetComponent<UIController>();
            if (uiController == null)
            {
                Debug.LogError("UIController コンポーネントの取得に失敗しました");
                return false;
            }
        }

        return true;
    }

    // ================= HP =================
    private void InitializeHP(int playerNumber)
    {
        currentHP = (int)armPlayerData.GetMaxHP();

        hpGaugeSystems[0].InitializeHP(currentHP,playerNumber + 1);
        hpGaugeSystems[1].InitializeHP(currentHP,playerNumber + 1);
    }

    // ---------------ダメージ処理---------------
    public void Damaged(int damage)
    {

        armPlayerController.GetAnimator().SetTrigger("IsHit");
        SoundManager.Instance.PlaySE("Damage");
        currentHP -= damage;

        hpGaugeSystems[0].Damage(damage);
        hpGaugeSystems[1].Damage(damage);
        // 確認：もし操作デバイスがコントローラーなら、振動

        if (currentHP <= 0)
        {
            armPlayerController.GetAnimator().SetTrigger("Down");
            SoundManager.Instance.PlaySE("KO");
            // 確認：もし操作デバイスがコントローラーなら、振動
            VibrateGamepad(0.75f, 1.0f);
            Debug.Log("HP が 0 になりました");
            uiController.ChangeState(PlayUIType.KO);

            //追加　マネージャーにプレイヤ番号を送る
            PlaySceneManager.Instance.SendCameraContorllerLosePlayerNumber(playerNumber);

            PlaySceneWinnerDataSender.Instance.SaveWinnerPlayerData(playerNumber);
        }
        else VibrateGamepad(0.25f, 0.5f);


    }

    public void Update()
    {
    }

    // ================= レイヤー =================
    private void ApplyPlayer3DHPGaugeLayer()
    {
        string layerName = (playerNumber == 0) ? Player2UILayer : Player1UILayer;
        int layer = LayerMask.NameToLayer(layerName);

        if (layer < 0)
        {
            Debug.LogError($"Layer が存在しません : {layerName}");
            return;
        }

        SetLayerRecursively(hpGaugeCanvas3D, layer);
    }

    private void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }

    // ================= Utility =================
    private bool TryGetChildObject(Transform parent, string name, out GameObject result)
    {
        Transform t = parent.Find(name);
        if (t == null)
        {
            Debug.LogError($"{name} が {parent.name} の子に存在しません");
            result = null;
            return false;
        }

        result = t.gameObject;
        return true;
    }

    private bool TryGetHPGauge2D(string path, out HPGaugeSystem system)
    {
        GameObject canvas = GameObject.Find(UiCanvas2DName);
        if (canvas == null)
        {
            Debug.LogError("UICanvas が存在しません");
            system = null;
            return false;
        }

        Transform t = canvas.transform.Find(path);
        if (t == null)
        {
            Debug.LogError($"{path} が UICanvas 配下に存在しません");
            system = null;
            return false;
        }

        system = t.GetComponent<HPGaugeSystem>();
        if (system == null)
        {
            Debug.LogError($"{path} に HPGaugeSystem が付いていません");
            return false;
        }

        return true;
    }

    private bool TryGetHPGauge3D(out HPGaugeSystem system)
    {
        Transform t = hpGaugeCanvas3D.transform.Find(HpGaugeSystemOwnerName);
        if (t == null)
        {
            Debug.LogError("3D HPGaugeGroup が存在しません");
            system = null;
            return false;
        }

        system = t.GetComponent<HPGaugeSystem>();
        if (system == null)
        {
            Debug.LogError("3D HPGaugeSystem が付いていません");
            return false;
        }

        return true;
    }

    // 振動させるメソッド
    private void VibrateGamepad(float duration = 0.4f, float power = 1.0f)
    {
        if (armPlayerController.PlayerInputDevice is Gamepad gamepad)
        {
            // 振動開始
            gamepad.SetMotorSpeeds(power, power);

            // 一定時間後に停止
            StartCoroutine(StopVibration(gamepad, duration));
        }
    }
    private IEnumerator StopVibration(Gamepad gamepad, float duration)
    {
        yield return new WaitForSeconds(duration);
        gamepad.SetMotorSpeeds(0f, 0f);
    }
}

