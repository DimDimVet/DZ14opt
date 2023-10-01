using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootPlayer : MonoBehaviour
{
    //
    [SerializeField] private ActionSettings actionSettings;
    //
    private IRegistrator dataReg;
    private RegistratorConstruction rezultListInput;
    private RegistratorConstruction rezulNetManager;

    [HideInInspector] public int CountBull;

    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform outBullet;

    [SerializeField] private ParticleSystem gunExitParticle;//������� ������

    //������� � ���� �������� �������

    private float shootDelay;
    private float shootTime = float.MinValue;

    //����
    private List<Bull> bulls=new List<Bull>();

    private void Start()
    {
        //���� ����������
        dataReg = new RegistratorExecutor();//������ � �����
        rezultListInput = dataReg.GetDataPlayer();
        rezulNetManager = dataReg.NetManager();

        dataReg.OutPos = outBullet;
        //
        InstBulls(3);
        //
        shootDelay = actionSettings.ShootDelay;
        StartCoroutine(Example());

    }

    //�������� ����
    private void InstBulls(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject bull= Instantiate(bullet, outBullet.position, outBullet.rotation);
            bulls.Add(bull.GetComponent<Bull>());
        }
    }


    void Update()
    {
        if (PhotonView.Get(this.gameObject).IsMine)
        {
            if (rezultListInput.UserInput == null)
            {
                rezultListInput = dataReg.GetDataPlayer();
                return;
            }

            if (rezultListInput.UserInput.InputData.Shoot != 0)//������� �������
            {
                Shoot();
            }
        }

    }
    private IEnumerator Example()
    {
        int i = 0;
        while (i < 3)
        {
            yield return new WaitForSeconds(0.2f);
            i++;
        }
    }

    public void Shoot()
    {
        if (Time.time < shootTime + shootDelay)
        {
            return;
        }
        else
        {
            shootTime = Time.time;
        }

        //bullFactory.Create();
        CountBull++;
        //Instantiate(bullet, outBullet.position, outBullet.rotation);
        for (int i = 0; i < bulls.Count; i++)
        {
            if (bulls[i].IsSet==false)
            {
                bulls[i].ShootBull(outBullet.position, true);
                return;
            }
        }
            
        //rezulNetManager.NetworkManager.BullInst(outBullet);
        gunExitParticle.Play();
        //Photon


    }
}
