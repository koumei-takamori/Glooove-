// ------------------------------------------------
// PlayerHP.cs
// プレイヤーのHP管理
// ダメージを受ける処理やUI変更を行う
// Playerへのアタッチを想定
// 足りないものがある場合はWarningとしてLogを表示
// 2026/01/07
// 池田桜輔
// ------------------------------------------------

using Nakashi.Player;
using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    // -----------------普遍値-----------------
    const string hpGaugeCanvas3DObjectName = "HPGaugeCanvas-3D"; // 3Dの相手が見るUIの名前(Layer切り替え用)
    const string hpSystemOwnerName = "HPGaugeGroup";// HPGaugeSystemの親オブジェクト名

    const string UICanvas2DObjectName = "UICanvas"; // 2Dの自分が見るUIの名前


    const string Player1LayrerTag = "Player1-UI"; // 1Pプレイヤーのタグ(自分の3D-HPが見えてしまうのを防ぐため)
    const string Player2LayrerTag = "Player2-UI"; // 2Pプレイヤーのタグ(自分の3D-HPが見えてしまうのを防ぐため)


    // -----------------変動値-----------------
    [SerializeField] private int currentHP;

    [SerializeField] private ArmPlayerController armPlayerController; // プレイヤー判別用
    [SerializeField] private ArmPlayerData armPlayerData; // 最大HP取得用
    [SerializeField] private GameObject hpGaugeCanvas3D; // 相手視点のHPゲージ表示用（Layerを変更する必要があるため）
    [SerializeField] private HPGaugeSystem[] hpGaugeSystem = new HPGaugeSystem[2]; // HPゲージシステム 0=2D 1=3D


    int playerNumber; // プレイヤー番号 1 or 2


    private void Start()
    {
        NullCheck(); // Nullチェック

        InitializedPlayerIndex(); // プレイヤー番号初期化
        SetToPlayer3DHPGaugeUILayer(); // 3DHPゲージのLayer設定
    }

    public void Damaged(int damage)
    {

    }


    // -----------------------Nullチェック-----------------------
    private void NullCheck()
    {
        // PlayerPrefabについている コンポーネントがアタッチされているか確認
        armPlayerController = GetComponent<ArmPlayerController>();

        if (armPlayerController == null)
            Debug.LogWarning("ArmPlayerControllerがアタッチされていません。PlayerHP.cs");


        armPlayerData = armPlayerController.GetPlayerData();
        if (armPlayerData == null)
            Debug.LogWarning("ArmPlayerDataの取得に失敗しました。PlayerHP.cs");


        // 3Dの相手が見るUIがアタッチされているか確認 ない場合はNameで取得
        if (hpGaugeCanvas3D == null)
        {
            hpGaugeCanvas3D = transform.Find(hpGaugeCanvas3DObjectName).gameObject;
            if (hpGaugeCanvas3D == null)
                Debug.LogWarning("HPGaugeCanvas-3Dが設定されていません。・Player子オブジェクトとして存在しません。PlayerHP.cs");
        }

        string player2DUIpath = "HPGaugeGroup-" + (playerNumber + 1) + "P";

        // 2DのHPGaugeSystemをNameで取得
        hpGaugeSystem[0] = GameObject.Find(UICanvas2DObjectName).transform.Find(player2DUIpath).GetComponent<HPGaugeSystem>();
        if (hpGaugeSystem[0] == null)
            Debug.LogWarning("2DのHPGaugeSystemがScene上に存在しません。PlayerHP.cs");

        // 3DのHPGaugeSystemをNameで取得
        hpGaugeSystem[1] = hpGaugeCanvas3D.transform.Find(hpSystemOwnerName).GetComponent<HPGaugeSystem>();
        if (hpGaugeSystem[1] == null)
            Debug.LogWarning("3DのHPGaugeSystemがHPGaugeCanvas-3Dの子オブジェクトとして存在しません。PlayerHP.cs");


        // レイヤーが存在するかを確認
        if (LayerMask.NameToLayer(Player1LayrerTag) == -1)
            Debug.LogWarning("Player1 -UIレイヤーが存在しません。PlayerHP.cs");

        if (LayerMask.NameToLayer(Player2LayrerTag) == -1)
            Debug.LogWarning("Player2 -UIレイヤーが存在しません。PlayerHP.cs");
    }



    // -----------------------取得用の処理-----------------------
    private void InitializedPlayerIndex()
    {
        // プレイヤー番号取得
        playerNumber = armPlayerController.PlayerId + 1;
    }


    // -----------------------レイヤー設定用の処理-----------------------
    private void SetToPlayer3DHPGaugeUILayer()
    {
        // プレイヤー番号に応じたレイヤー名を取得
        string layerName = (playerNumber == 1)
            ? Player1LayrerTag
            : Player2LayrerTag;

        int layer = LayerMask.NameToLayer(layerName);

        // hpGaugeCanvas3D 自身 + 子孫すべてを変更
        SetLayerRecursively(hpGaugeCanvas3D, layer);
    }


    private void SetLayerRecursively(GameObject obj, int layer)
    {
        // 自分自身のレイヤーを変更
        obj.layer = layer;

        // 子オブジェクトを再帰的に処理
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }
}
