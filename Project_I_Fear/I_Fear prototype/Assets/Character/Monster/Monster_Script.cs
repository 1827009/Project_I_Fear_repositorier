using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster_Script : MonoBehaviour
{
    [SerializeField, TooltipAttribute("プレイヤー")] public GameObject player = null;
    [SerializeField, TooltipAttribute("巡回ルート")] public GameObject[] movePoint = null;
    [SerializeField, TooltipAttribute("逃走ルート")] public GameObject[] escapePoint = null;
    [SerializeField, TooltipAttribute("カメラ")] public new Clear2 camera;
    [SerializeField, TooltipAttribute("太陽")] public new GameObject light;
    public GameObject effect;
    public int BellMove = 0;
    public float NoDamegeTime=2;
    public float StopTime = 2;
    public bool Dead = false;
    public bool hit = false;
    public bool Rstart = false;
    public AudioClip voice;
    public AudioClip Voise_verBell;
    public AudioClip Walk;
    public AudioClip DeadVoice;
    public AudioClip AttackSE;


    private GameObject NavMeshObstacle;
    AudioSource audioSource;
    PlayerScript player_sclipt;
    Clear2 Clear_sclipt;
    NavMeshAgent navMesh;
    public GameObject target;
    GameObject AttackAnimetion;
    private Animator animator;
    Vector3 oldPlayer;
    int p;
    float AttackTime=0;
    float CountTime = 0;
    bool CountON = false;
    bool sound = false;
    bool attck = false;



    // Start is called before the first frame update
    void Start()
    {
        player_sclipt = player.GetComponent<PlayerScript>();
        Clear_sclipt = camera.GetComponent<Clear2>();
        if (BellMove == 1)
        {
            NavMeshObstacle = player.transform.GetChild(7).gameObject;
        }
        target = movePoint[0];
        navMesh = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        p = 1;
        audioSource.clip = voice;
        audioSource.Play();
        // AttackAnimetion.SetActive(true);
        AttackAnimetion = player.transform.GetChild(4).gameObject;
        animator = AttackAnimetion.GetComponent<Animator>();
        animator.SetBool("Attack", false);

    }

    // Update is called once per frame
    void Update()
    {
        
        
        if (target != null)
        {
            navMesh.destination = target.transform.position;
            if (Vector3.Distance(transform.position, target.transform.position) < 2)
            {
                target = movePoint[p];
                p++;
                if (movePoint.Length == p||movePoint[p]==null)
                {
                    p = 0;
                }

            }
            if (player_sclipt.Fade)
            {
                if (!audioSource.isPlaying)
                {
                    audioSource.UnPause();
                }
            }
            else
            {
                if (audioSource.isPlaying)
                {
                    audioSource.Pause();
                }
            }
            DeadOn();
            Induction();
            
            if (CountON)
            {
                CountDwun();
            }
        }

        oldPlayer = player_sclipt.transform.position;

    }

    bool start = true;
    private void DeadOn()
    {
        if (Dead)
        {
            //アニメーション
            target = light;
            transform.position = new Vector3(light.transform.position.x, transform.position.y, light.transform.position.z);
            if (start)
            {
                Vector3 pos = transform.position;
                pos.y = 0;
                Instantiate(effect, pos, Quaternion.identity);
                audioSource.clip = DeadVoice;
                audioSource.volume = 0.5f;
                audioSource.loop = false;
                audioSource.Play();
                start = false;
            }
            if (audioSource.clip == DeadVoice)
            {
                StartCoroutine(Checking(() =>
                {
                    int count;
                    count = Clear_sclipt.Dead();
                    if (Clear_sclipt.DeadCount != count)
                    {
                        Rstart = true;
                    }
                    Destroy(this.gameObject);
                    Dead = false;
                }));
            }


        }
    }

    //音が鳴り終わったかどうか
    public delegate void functionType();
    private IEnumerator Checking(functionType callback)
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            if (!audioSource.isPlaying && Dead)
            {
                callback();
                break;
            }

        }
    }



    //誘導
    private void Induction()
    {
       // if (hit)
       // {
            if (player_sclipt.OnBell && player_sclipt.Fade)
            {
            switch (BellMove)
            {
                case 0:
                    target = player;
                    break;
                case 1:
                    if (0 < player.transform.position.z)
                    {
                        target = escapePoint[Random.Range(0, escapePoint.Length/2-1)];
                    }
                    else
                    {
                        
                        target = escapePoint[Random.Range(escapePoint.Length/2, escapePoint.Length-1)];
                    }
                    NavMeshObstacle.SetActive(true);
                    break;
                default:
                    break;
            }
            if (!sound)
                {
                    audioSource.clip = Voise_verBell;
                    audioSource.Play();
                    sound = true;
                }
                 CountON = false;
            }

            if (!player_sclipt.Fade && target == player)
            {
                CountON = true;
                CountTime = Time.time;
               // target = movePoint[p];
               /* if (sound)
                {
                    audioSource.clip = voice;
                    audioSource.Play();
                    sound = false;
                }*/

            }
       // }

    }

    private void CountDwun()
    {
        if (Time.time - CountTime > StopTime)
        {
            target = movePoint[Random.Range(0, movePoint.Length)];
            audioSource.clip = voice;
            audioSource.Play();
            CountON = false;
        }
    }
    //アタック
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.tag == "Player")
        {
            if (Time.time - AttackTime > NoDamegeTime)
            {
                AttackAnimetion.SetActive(false);
                if (player_sclipt.Fade)
                {
                    audioSource.PlayOneShot(AttackSE,PlayerScript.FadeVolume);
                }
                else
                {
                    
                        AttackAnimetion.SetActive(true);
                        animator.SetBool("Attack", true);
                    
                    
                }
                player_sclipt.attackEntityFrom(1);
                AttackTime = Time.time;
            }
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.transform.tag == "Player")
        {
            animator.SetBool("Attack", false);
        }
    }

    public void Search()
    {
        hit = true;
    }

    public GameObject Taget()
    {
        return target;
    }
}