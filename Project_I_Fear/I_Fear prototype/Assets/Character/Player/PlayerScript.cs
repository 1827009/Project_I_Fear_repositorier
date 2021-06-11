using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using UnityEngine.XR;

public class PlayerScript : OVRPlayerController
{
    [SerializeField, TooltipAttribute("足音")] AudioClip footstepSound;
    [SerializeField, TooltipAttribute("ベルのモデルObject")] GameObject bellModel;
    [SerializeField, TooltipAttribute("視界モード、聴覚モードの切り替えクールタイム")] float modeChangeCoolTime = 5;
    [SerializeField, TooltipAttribute("開かれるメニューprefab")] GameObject menuPrefab;
    [SerializeField, TooltipAttribute("LogUi")] LogMessage logMessage;
    [SerializeField, TooltipAttribute("LogUiImage")] Sprite bellGetMessageImage;
    public float Health
    {
        get { return health; }
        set { health = value; uiController.HealthTextUpdate(); }
    }
    float health;
    public float MaxHealth { get => maxHealth; set { maxHealth = value; } }
    float maxHealth;

    //足踏み音のペース(仮)
    const float footstepsPace = 0.75f;
    float footsteps;

    /// <summary>
    /// 視界モードのオンオフ
    /// </summary>
    public bool Fade { get { return fade; }set { fade = value; } }
    bool fade;
    float modeCoolTime;
    /// <summary>
    /// 聴覚のオンオフによる音の変動
    /// </summary>
    public static float FadeVolume;

    /// <summary>
    /// ベルを所持しているか
    /// </summary>
    public bool HaveBell { get { return haveBell; } set { haveBell = value; } }
    bool haveBell;

    /// <summary>
    /// 鈴を構えているか
    /// </summary>
    public bool OnBell { get { return bellActiveTime > 0; }}
    bool bell;
    float bellSoundCoolTime;
    float BellActiveTime
    { get { return bellActiveTime; } 
        set { bellActiveTime = value;updateDelegate += BellActiveTimeUpdate; } }
    float bellActiveTime;

    /// <summary>
    /// メニューを開いているか
    /// </summary>
    public bool OnMenuMode { get; set; }

    /// <summary>
    /// 全操作不能フラグ
    /// </summary>
    public bool ControlFrieze {
        get { return controlFrieze; } 
        set { controlFrieze = value;
            if (controlFrieze)
            {
                tompAcceleration = Acceleration;
                Acceleration = 0;
            }
            else
                Acceleration = tompAcceleration;
        } 
    }
    bool controlFrieze;
    float tompAcceleration;

    CharacterController controller;
    AudioSource audioSource;
    PlayerUiController uiController;
    [HideInInspector] public VRUIController vrGUiController;

    /// <summary>
    /// 更新のイベント
    /// </summary>
    Action updateDelegate;

    // Start is called before the first frame update
    [Obsolete]
    new void Start()
    {
        base.Start();
        controller = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
        uiController = GetComponent<PlayerUiController>();
        vrGUiController = GetComponent<VRUIController>();

        updateDelegate = new Action(BellActiveTimeUpdate);
        updateDelegate += ModeSwitchUpdate;
        updateDelegate += OpenMenu;

        Health = 3;
    }

    // Update is called once per frame
    public new void Update()
    {
        InputController.Update();
        base.Update();
        if (InputController.ApplicationQuit())
            Application.Quit();

        if (!controlFrieze)
        {
            updateDelegate();
        }
    }

    /// <summary>
    /// このオブジェクトにダメージを与える
    /// </summary>
    /// <param name="damage"></param>
    public void attackEntityFrom(float damage)
    {
        Health -= damage;
        uiController.DamageEffect();
        if (Health <= 0)
            Debug.Log("Dead");
    }

    /// <summary>
    /// ベルの使用を解禁する
    /// </summary>
    [Obsolete]
    public void ItemGetingBell()
    {
        logMessage.LogStartUp(bellGetMessageImage);
        updateDelegate += BellUpdate;
        haveBell = true;
    }

    /// <summary>
    /// メニューを開く
    /// </summary>
    public void OpenMenu()
    {
        if (InputController.MenuModeSwitch())
        {
                if (!OnMenuMode) 
                {
                    GameObject menuWindow = Instantiate(menuPrefab, transform.position + transform.forward, transform.rotation);
                    menuWindow.GetComponent<MenuObjectScript>().Open(GetComponent<PlayerScript>());
                }
        }
    }
        
    /// <summary>
    /// 初期から更新するメソッド
    /// </summary>
    void BellActiveTimeUpdate()
    {
        if (BellActiveTime > 0)
            BellActiveTime -= Time.deltaTime;
        else
        {
            updateDelegate -= this.BellActiveTimeUpdate;
        }
    }

    /// <summary>
    /// 移動の更新
    /// </summary>
    protected override void UpdateController()
    {
        base.UpdateController();
        
        Vector3 input = InputController.Move();

        if (input != Vector3.zero)
        {
            //足音
            footsteps -= Time.deltaTime;
            if (footsteps <= 0 && fade)
            {
                footsteps = footstepsPace;
                FootstepSoundRing();
            }

        }
        else
        {
            footsteps = 0;
        }
    }

    // 継承元のtriggerによる加速を消去
    public override void UpdateMovement()
    {
        if (HaltUpdateMovement)
            return;

        if (EnableLinearMovement)
        {
            bool moveForward = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
            bool moveLeft = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
            bool moveRight = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
            bool moveBack = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);

            bool dpad_move = false;

            if (OVRInput.Get(OVRInput.Button.DpadUp))
            {
                moveForward = true;
                dpad_move = true;

            }

            if (OVRInput.Get(OVRInput.Button.DpadDown))
            {
                moveBack = true;
                dpad_move = true;
            }

            MoveScale = 1.0f;

            if ((moveForward && moveLeft) || (moveForward && moveRight) ||
                (moveBack && moveLeft) || (moveBack && moveRight))
                MoveScale = 0.70710678f;

            // No positional movement if we are in the air
            if (!Controller.isGrounded)
                MoveScale = 0.0f;

            MoveScale *= SimulationRate * Time.deltaTime;

            // Compute this for key movement
            float moveInfluence = Acceleration * 0.1f * MoveScale * MoveScaleMultiplier;

            // Run!
            if (dpad_move || Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                moveInfluence *= 2.0f;

            Quaternion ort = transform.rotation;
            Vector3 ortEuler = ort.eulerAngles;
            ortEuler.z = ortEuler.x = 0f;
            ort = Quaternion.Euler(ortEuler);

            if (moveForward)
                MoveThrottle += ort * (transform.lossyScale.z * moveInfluence * Vector3.forward);
            if (moveBack)
                MoveThrottle += ort * (transform.lossyScale.z * moveInfluence * BackAndSideDampen * Vector3.back);
            if (moveLeft)
                MoveThrottle += ort * (transform.lossyScale.x * moveInfluence * BackAndSideDampen * Vector3.left);
            if (moveRight)
                MoveThrottle += ort * (transform.lossyScale.x * moveInfluence * BackAndSideDampen * Vector3.right);

            moveInfluence = Acceleration * 0.1f * MoveScale * MoveScaleMultiplier;

            Vector2 primaryAxis = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

            // If speed quantization is enabled, adjust the input to the number of fixed speed steps.
            if (FixedSpeedSteps > 0)
            {
                primaryAxis.y = Mathf.Round(primaryAxis.y * FixedSpeedSteps) / FixedSpeedSteps;
                primaryAxis.x = Mathf.Round(primaryAxis.x * FixedSpeedSteps) / FixedSpeedSteps;
            }

            if (primaryAxis.y > 0.0f)
                MoveThrottle += ort * (primaryAxis.y * transform.lossyScale.z * moveInfluence * Vector3.forward);

            if (primaryAxis.y < 0.0f)
                MoveThrottle += ort * (Mathf.Abs(primaryAxis.y) * transform.lossyScale.z * moveInfluence *
                                       BackAndSideDampen * Vector3.back);

            if (primaryAxis.x < 0.0f)
                MoveThrottle += ort * (Mathf.Abs(primaryAxis.x) * transform.lossyScale.x * moveInfluence *
                                       BackAndSideDampen * Vector3.left);

            if (primaryAxis.x > 0.0f)
                MoveThrottle += ort * (primaryAxis.x * transform.lossyScale.x * moveInfluence * BackAndSideDampen *
                                       Vector3.right);
        }

        if (EnableRotation)
        {
            Vector3 euler = transform.rotation.eulerAngles;
            float rotateInfluence = SimulationRate * Time.deltaTime * RotationAmount * RotationScaleMultiplier;

            bool curHatLeft = OVRInput.Get(OVRInput.Button.PrimaryShoulder);

            if (curHatLeft && !prevHatLeft)
                euler.y -= RotationRatchet;

            prevHatLeft = curHatLeft;

            bool curHatRight = OVRInput.Get(OVRInput.Button.SecondaryShoulder);

            if (curHatRight && !prevHatRight)
                euler.y += RotationRatchet;

            prevHatRight = curHatRight;

            euler.y += buttonRotation;
            buttonRotation = 0f;


#if !UNITY_ANDROID || UNITY_EDITOR
            if (!SkipMouseRotation)
                euler.y += Input.GetAxis("Mouse X") * rotateInfluence * 3.25f;
#endif

            if (SnapRotation)
            {
                if (OVRInput.Get(OVRInput.Button.SecondaryThumbstickLeft) ||
                    (RotationEitherThumbstick && OVRInput.Get(OVRInput.Button.PrimaryThumbstickLeft)))
                {
                    if (ReadyToSnapTurn)
                    {
                        euler.y -= RotationRatchet;
                        ReadyToSnapTurn = false;
                    }
                }
                else if (OVRInput.Get(OVRInput.Button.SecondaryThumbstickRight) ||
                    (RotationEitherThumbstick && OVRInput.Get(OVRInput.Button.PrimaryThumbstickRight)))
                {
                    if (ReadyToSnapTurn)
                    {
                        euler.y += RotationRatchet;
                        ReadyToSnapTurn = false;
                    }
                }
                else
                {
                    ReadyToSnapTurn = true;
                }
            }
            else
            {
                Vector2 secondaryAxis = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
                if (RotationEitherThumbstick)
                {
                    Vector2 altSecondaryAxis = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
                    if (secondaryAxis.sqrMagnitude < altSecondaryAxis.sqrMagnitude)
                    {
                        secondaryAxis = altSecondaryAxis;
                    }
                }
                euler.y += secondaryAxis.x * rotateInfluence;
            }

            transform.rotation = Quaternion.Euler(euler);
        }
    }

    /// <summary>
    /// 歩行時の効果音
    /// </summary>
    void FootstepSoundRing()
    {
        audioSource.PlayOneShot(footstepSound, PlayerScript.FadeVolume);
    }

    // inputDownバグったのでごり押し
    bool inputErrorAvoidance;
    /// <summary>
    /// 視界モードと聴覚モードの切り替え入力の更新
    /// </summary>
    void ModeSwitchUpdate()
    {
        if (InputController.ModeSwitch())
        {
            if (!inputErrorAvoidance)
            {
                if (fade)
                    fade = false;
                else
                    fade = true;
            }
            inputErrorAvoidance = true;
            modeCoolTime = modeChangeCoolTime;
        }
        else
            inputErrorAvoidance = false;
    }

    /// <summary>
    /// ベルのオンオフの更新
    /// </summary>
    [Obsolete]
    public void BellUpdate()
    {
        if (InputController.BeelButtonDown())
        {
            if (!bell)
            {
                bellModel.SetActive(true);
                bellModel.GetComponent<BellSwingGeter>().Initialized();
                bell = true;
            }
        }
        else if (bell)
        {
            bellModel.SetActive(false);
            bell = false;
        }
    }
    /// <summary>
    /// ベル発動時に呼ぶ
    /// </summary>
    public void BellSwing(float activeTime)
    {
        BellActiveTime = activeTime;
    }
}

public class InputController
{
    static bool inputTomp = false;

    static bool inputErrorMenu;
    public static void Update()
    {
        menuDown = false;
        if (OVRInput.Get(OVRInput.RawButton.Start))
        {
            if (!inputErrorMenu)
            {
                menuDown = true;
            }
            inputErrorMenu = true;
        }
        else
            inputErrorMenu = false;
    }

    public static Vector3 Move()
    {
        Vector2 vector= OVRInput.Get(OVRInput.RawAxis2D.LThumbstick);
        return new Vector3(vector.x, 0, vector.y);//new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }
    public static Vector2 Camera()
    {
        return OVRInput.Get(OVRInput.RawAxis2D.RThumbstick);
    }
    public static bool ModeSwitch()
    {
        return OVRInput.Get(OVRInput.RawButton.X);//Input.GetKeyDown(KeyCode.Q);
    }
    public static bool BeelButtonDown()
    {
        return OVRInput.Get(OVRInput.RawButton.LHandTrigger);
    }
    public static bool BeelButtonUp()
    {
        return OVRInput.Get(OVRInput.RawButton.LHandTrigger);
    }
    public static bool UIDecision()
    {
        return OVRInput.Get(OVRInput.RawButton.RIndexTrigger) || OVRInput.Get(OVRInput.RawButton.A);
    }
    static bool menuDown;
    public static bool MenuModeSwitch()
    {
        return menuDown;
    }
    public static bool ApplicationQuit()
    {
        return Input.GetKeyDown(KeyCode.Escape);
    }
}
/// <summary>
/// 自作よく使いそう処理
/// </summary>
public class MyUtility
{
    /// <summary>
    /// 目標値に向かって加算し、処理前の値が目標と同じか処理後の数値が目標値をまたいでいるときtrueを返す
    /// </summary>
    /// <param name="a">加算される値</param>
    /// 
    /// <param name="b">加算する値</param>
    /// 
    /// <param name="goal">目標値</param>
    /// 
    /// <returns></returns>
    public static bool FloatErrorAdjustment(ref float a, float b, float goal)
    {
        float c = a + b;
        if ((goal > a && goal <= c) || (goal < a && goal >= c) || a == goal)
        {
            a = goal;
            return true;
        }
        else
        {
            a = c;
            return false;
        }
    }
}