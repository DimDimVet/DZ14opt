using UnityEngine;


public class Bull : MonoBehaviour
{

    [SerializeField] private BullSettings bullSettings;
    public bool IsSet = false;
    //���������
    private Renderer rendererGO;
    [SerializeField] ParticleSystem particleSystemGO;
    private float shootTime = float.MinValue;

    private int hashGO;
    private IRegistrator dataReg;
    private RegistratorConstruction rezultListGO;

    [SerializeField] private GameObject decalGO;
    private int damage;
    private int speed;
    private Collider collaiderBullet;
    private Vector3 startPos;

    private void Start()
    {
        rendererGO = gameObject.GetComponent<Renderer>();

        damage = bullSettings.Damage;
        speed = bullSettings.Speed;
        collaiderBullet = gameObject.GetComponent<Collider>();
        startPos = transform.position;


        if (IsSet == false)
        {
            particleSystemGO.Stop();
            rendererGO.enabled = IsSet;
            collaiderBullet.enabled = IsSet;
        }
    }

    public void ShootBull(Vector3 startPos, bool isSet)
    {
        transform.position = startPos;
        IsSet = isSet;
    }


    private void Update()
    {
        if (IsSet == false)
        {

        }
        else
        {
            particleSystemGO.Play();
            rendererGO.enabled = IsSet;
            collaiderBullet.enabled = IsSet;

            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            RaycastHit hit;
            GameObject decal;
            if (Physics.Linecast(startPos, transform.position, out hit))
            {
                ExecutorCollision(hit);

                collaiderBullet.enabled = false;
                decal = Instantiate(decalGO);
                decal.transform.position = hit.point + hit.normal * 0.001f;
                decal.transform.rotation = Quaternion.LookRotation(-hit.normal);
                Destroy(decal, 1);

                //Destroy(gameObject);
                //
                IsSet = false;
                particleSystemGO.Stop();
                rendererGO.enabled = IsSet;
                collaiderBullet.enabled = IsSet;
                //
            }
            else
            {
                //Destroy(gameObject, 5);
                //
                if (Time.time < shootTime + 50)
                {
                    return;
                }
                else
                {
                    shootTime = Time.time;
                }

                IsSet = false;
                particleSystemGO.Stop();
                rendererGO.enabled = IsSet;
                collaiderBullet.enabled = IsSet;
                //
            }
            startPos = transform.position;
        }
    }

    private void ExecutorCollision(RaycastHit hit)
    {
        //���� ������
        hashGO = hit.collider.gameObject.GetHashCode();
        dataReg = new RegistratorExecutor();//������ � �����
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
