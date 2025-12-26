/**********************************************
 * 
 *  NInputManager.cs 
 *  Joy-ConのNpad入力＋バイブレーション＋六軸センサーを管理する
 *  Joy-Con２本で１つのデバイスとするスタイルのみ適応
 *  最大接続デバイスは２つ
 * 
 *  制作者 : 髙森 煌明
 *  制作日 : 2025/12/18
 * 
 **********************************************/
//using UnityEngine;
//using nn.hid;
//using System.Collections.Generic;
//using static VibrationFramework;

//public enum HandType
//{
//    Left = 0,
//    Right = 1
//}

///// <summary>
///// Joy-Conの入力を管理する
///// </summary>
//public class NInputManager : SingletonMonoBehaviour<NInputManager>
//{
//    // Npad
//    // プレイヤーID
//    [SerializeField] private NpadId[] npadIds = { NpadId.Handheld, NpadId.No1, NpadId.No2 };

//    // 各NpadIdの状態を保持
//    private Dictionary<NpadId, NpadState> npadStates = new Dictionary<NpadId, NpadState>();
//    private Dictionary<NpadId, NpadStyle> npadStyles = new Dictionary<NpadId, NpadStyle>();

//    // バイブレーション
//    private const int MAX_VIBRATION_DEVICE = 2;  // 最大バイブレーションデバイス数
//    private VibrationDeviceHandle[] vibrationHandles = new VibrationDeviceHandle[MAX_VIBRATION_DEVICE];
//    private VibrationDeviceInfo[] vibrationInfos = new VibrationDeviceInfo[MAX_VIBRATION_DEVICE];
//    private int vibrationDeviceCount = 0;
//    private Dictionary<string, byte[]> vibrationFiles = new Dictionary<string, byte[]>();


//    // 六軸センサー関連
//    private SixAxisSensorHandle[] axisHandles = new SixAxisSensorHandle[2];
//    private SixAxisSensorState axisState = new SixAxisSensorState();
//    private int axisHandleCount = 0;

//    private Quaternion[] currentRotation = new Quaternion[2];
//    private Vector3[] currentPosition = new Vector3[2];
//    private Vector3[] previousPosition = new Vector3[2];
//    private nn.util.Float4 floatQuat = new nn.util.Float4();


//    // Controller Support 用 
//    private ControllerSupportArg controllerSupportArg = new ControllerSupportArg();
//    private nn.Result result = new nn.Result();


//    /*--------------------------------------------------------------------------------
//    || 実行前初期化処理
//    --------------------------------------------------------------------------------*/
//    /// <summary>
//    /// 実行前初期化処理
//    /// </summary>
//    protected override void Awake()
//    {
//        base.Awake();

//        // Npad初期化
//        Npad.Initialize();

//        // 対応するプレイヤーIDを設定
//        Npad.SetSupportedIdType(npadIds);

//        // 対応するスタイルを設定（携帯モード / Joy-Con 2本 / フルキーボード）
//        Npad.SetSupportedStyleSet(NpadStyle.Handheld | NpadStyle.JoyDual | NpadStyle.FullKey);

//        // Joy-Conの持ち方を設定（縦持ち）
//        NpadJoy.SetHoldType(NpadJoyHoldType.Vertical);

//        // 各NpadIdの初期状態を設定
//        foreach (var id in npadIds)
//        {
//            npadStates[id] = new NpadState();
//            npadStyles[id] = NpadStyle.Invalid;
//        }
//    }


//    /*--------------------------------------------------------------------------------
//    || 更新処理
//    --------------------------------------------------------------------------------*/
//    /// <summary>
//    /// 更新処理
//    /// </summary>
//    private void Update()
//    {
//        // Npad状態更新
//        UpdatePadStates();
//        // 六軸センサー更新
//        UpdateAxis();
//    }


//    /*--------------------------------------------------------------------------------
//    || Npad状態更新
//    --------------------------------------------------------------------------------*/
//    private void UpdatePadStates()
//    {
//        foreach (var id in npadIds)
//        {
//            // 現在のスタイルを取得
//            NpadStyle style = Npad.GetStyleSet(id);

//            if (style != NpadStyle.None)
//            {
//                // Npad状態を取得
//                NpadState state = npadStates[id];
//                Npad.GetState(ref state, id, style);

//                // 状態を格納
//                npadStates[id] = state;
//                npadStyles[id] = style;

//                // バイブレーションデバイス更新
//                SetupVibration(id, style);

//                // 六軸センサー更新
//                SetupAxis(id, style);
//            }
//            else
//            {
//                // 接続がない場合は状態をクリア
//                npadStates[id].Clear();
//                npadStyles[id] = NpadStyle.Invalid;
//            }
//        }
//    }

//    /// <summary>
//    /// 指定したNpadIdの状態を取得
//    /// </summary>
//    public NpadState GetNpadState(NpadId id) => npadStates.ContainsKey(id) ? npadStates[id] : new NpadState();

//    /// <summary>
//    /// 指定したNpadIdのスタイルを取得
//    /// </summary>
//    public NpadStyle GetNpadStyle(NpadId id) => npadStyles.ContainsKey(id) ? npadStyles[id] : NpadStyle.Invalid;


//    /// <summary>
//    /// すべてのNpadIdのスタイルを取得
//    /// </summary>
//    public NpadId[] GetSupportedIds() => npadIds;

//    /// <summary>
//    /// 指定したNpadIdが接続中か
//    /// </summary>
//    public bool IsConnected(NpadId id) => npadStyles.ContainsKey(id) && npadStyles[id] != NpadStyle.Invalid;


//    /*--------------------------------------------------------------------------------
//    || バイブレーション管理
//    --------------------------------------------------------------------------------*/
//    private void SetupVibration(NpadId id, NpadStyle style)
//    {
//        // デバイスハンドルを取得
//        vibrationDeviceCount = Vibration.GetDeviceHandles(vibrationHandles, MAX_VIBRATION_DEVICE, id, style);

//        // デバイスごとに初期化
//        for (int i = 0; i < vibrationDeviceCount; i++)
//        {
//            Vibration.InitializeDevice(vibrationHandles[i]);
//            Vibration.GetDeviceInfo(ref vibrationInfos[i], vibrationHandles[i]);
//        }
//    }

//    /// <summary>
//    /// バイブレーションデータをロード
//    /// </summary>
//    public void LoadVibration(string name, string path)
//    {
//        byte[] file = System.IO.File.ReadAllBytes(Application.streamingAssetsPath + path);

//        VibrationFileInfo fileInfo = new VibrationFileInfo();
//        VibrationFileParserContext context = new VibrationFileParserContext();
//        var result = VibrationFile.Parse(ref fileInfo, ref context, file, file.LongLength);

//        Debug.Assert(result.IsSuccess());
//        vibrationFiles[name] = file;
//    }

//    /// <summary>
//    /// バイブレーションを再生
//    /// </summary>
//    public void PlayVibration(NpadId id, HidVibrationSlot slot, string fileName)
//    {
//        if (!IsConnected(id) || !vibrationFiles.ContainsKey(fileName)) return;
//        VibrationFramework.Play(id, slot, vibrationFiles[fileName]);
//    }

//    /// <summary>
//    /// バイブレーションを停止
//    /// </summary>
//    public void StopVibration(NpadId id)
//    {
//        for (int i = 0; i < MAX_VIBRATION_DEVICE; i++)
//            VibrationFramework.Stop(id);
//    }


//    /*--------------------------------------------------------------------------------
//    || 六軸センサー管理
//    --------------------------------------------------------------------------------*/
//    private void SetupAxis(NpadId id, NpadStyle style)
//    {
//        // 現在のセンサーを停止
//        for (int i = 0; i < axisHandleCount; i++)
//            SixAxisSensor.Stop(axisHandles[i]);

//        // ハンドルを取得
//        axisHandleCount = SixAxisSensor.GetHandles(axisHandles, axisHandles.Length, id, style);

//        // 再度センサーを開始
//        for (int i = 0; i < axisHandleCount; i++)
//            SixAxisSensor.Start(axisHandles[i]);
//    }

//    private void UpdateAxis()
//    {
//        for (int i = 0; i < axisHandleCount; i++)
//        {
//            // 現在の状態を取得
//            SixAxisSensor.GetState(ref axisState, axisHandles[i]);

//            // クォータニオン取得
//            axisState.GetQuaternion(ref floatQuat);
//            currentRotation[i] = new Quaternion(floatQuat.x, floatQuat.z, floatQuat.y, -floatQuat.w);

//            // 過去の位置を保存
//            previousPosition[i] = currentPosition[i];

//            // 現在の加速度位置を取得
//            currentPosition[i] = new Vector3(axisState.acceleration.x, axisState.acceleration.y, axisState.acceleration.z);
//        }
//    }

//    /// <summary>
//    /// 指定インデックスの回転を取得
//    /// </summary>
//    public Quaternion GetRotation(int index) => index < axisHandleCount ? currentRotation[index] : Quaternion.identity;

//    /// <summary>
//    /// 指定インデックスの位置を取得
//    /// </summary>
//    public Vector3 GetPosition(int index) => index < axisHandleCount ? currentPosition[index] : Vector3.zero;

//    /// <summary>
//    /// 指定インデックスの移動量を取得
//    /// </summary>
//    public Vector3 GetDeltaPosition(int index) => index < axisHandleCount ? currentPosition[index] - previousPosition[index] : Vector3.zero;

//    /// <summary>
//    /// 指定インデックスの移動距離を取得
//    /// </summary>
//    public float GetDeltaDistance(int index) => GetDeltaPosition(index).magnitude;
//}
