using UnityEngine;


public class Bull : MonoBehaviour
{
    public bool isSet = false;

    [SerializeField] private ParticleSystem particleSys;
    [SerializeField] private TrailRenderer trailRender;
    [SerializeField] private BullSettings bullSettings;

    private int hashGO;
    private IRegistrator dataReg;
    private RegistratorConstruction rezultListGO;

    [SerializeField] private GameObject decalGO;
    private int damage;
    private int speed;
    private Collider collaiderBullet;
    private Vector3 startPos;
    public Vector3 EndPos;
    private Renderer rendererGO;

    private float shootDelay=5f;
    private float shootTime = float.MinValue;

    private GameObject decal;
    private SpriteRenderer spriteRender;

    private void Start()
    {
        rendererGO = GetComponent<Renderer>();

        damage = bullSettings.Damage;
        speed = bullSettings.Speed;
        collaiderBullet = gameObject.GetComponent<Collider>();
        startPos = transform.position;

        if (isSet == false)
        {
            //collaiderBullet.enabled = isSet;
            rendererGO.enabled = isSet;
            particleSys.Stop();
            trailRender.enabled = isSet;
        }

        InstDecal();
    }

    private void InstDecal()
    {
        decal = Instantiate(decalGO);
        spriteRender = decal.GetComponent<SpriteRenderer>();
        spriteRender.enabled = false;
    }



    public void ShootBull(bool _isSet, Transform _startPos)
    {
        transform.position = _startPos.position;
        transform.rotation = _startPos.rotation;
        startPos = transform.position;

        EndPos = _startPos.position;
        EndPos.y = _startPos.position.y + 1f;
        isSet = _isSet;

        {
            rendererGO.enabled = true;
            particleSys.Play();
            trailRender.enabled = true;
        }
    }

    private void Update()
    {
        if (isSet == false)
        {
            return;
        }
        else
        {

            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            RaycastHit hit;
            //GameObject decal;
            if (Physics.Linecast(startPos, transform.position, out hit))
            {
                ExecutorCollision(hit);

                collaiderBullet.enabled = false;
                //decal = Instantiate(decalGO);

                decal.transform.position = hit.point + hit.normal * 0.001f;
                decal.transform.rotation = Quaternion.LookRotation(-hit.normal);
                spriteRender.enabled = true;

                {
                    rendererGO.enabled = false;
                    particleSys.Stop();
                    trailRender.enabled = false;
                    transform.position = EndPos;
                    isSet = false;
                }


                if (Time.time < shootTime * 1.1f)
                {
                    return;
                }
                else
                {
                    shootTime = Time.time;
                }
                spriteRender.enabled = false;

                //Destroy(decal, 1);

                //Destroy(gameObject);
                
                
            }
            else
            {
                //Destroy(gameObject, 5);

                if (Time.time < shootTime + shootDelay)
                {
                    return;
                }
                else
                {
                    shootTime = Time.time;
                }

                rendererGO.enabled = false;
                particleSys.Stop();
                trailRender.enabled = false;
                transform.position = EndPos;
                isSet = false;
            }
            //startPos = transform.position;
        }


    }

    private void ExecutorCollision(RaycastHit hit)
    {
        //ищем объект
        hashGO = hit.collider.gameObject.GetHashCode();
        dataReg = new RegistratorExecutor();//доступ к листу
        rezultListGO = dataReg.GetData(hashGO);

        //Healt
        if (rezultListGO.Hash == hashGO)
        {
            if (rezultListGO.HealtObj != null)
            {
                rezultListGO.HealtObj.Damage = damage;
            }
            if (rezultListGO.PlayerHealt != null)
            {
                rezultListGO.PlayerHealt.Damage = damage;
            }
        }
        else
        {
            Debug.Log("No Script");
        }
    }
}
