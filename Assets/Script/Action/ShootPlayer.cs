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

    [SerializeField] private ParticleSystem gunExitParticle;//система частиц

    //соберем в лист стороние скрипты

    private float shootDelay;
    private float shootTime = float.MinValue;

    //Bulls
    private List<Bull> bulls = new List<Bull>(); //тут будем хранить экз пуль   

    private void Start()
    {
        //ищем управление
        dataReg = new RegistratorExecutor();//доступ к листу
        rezultListInput = dataReg.GetDataPlayer();
        rezulNetManager = dataReg.NetManager();

        dataReg.OutPos = outBullet;
        shootDelay =actionSettings.ShootDelay;

        InstBulls(10);

        StartCoroutine(Example());

    }

    private void InstBulls(int count)
    {
        for (int i = 0; i < count; i++) 
        {
            GameObject bulle =Instantiate(bullet, outBullet.position, outBullet.rotation);
            Bull scriptBulle = bulle.GetComponent<Bull>();

            scriptBulle.isSet = false;
            bulls.Add(scriptBulle);
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

            if (rezultListInput.UserInput.InputData.Shoot != 0)//получим нажатие
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
        for (int i = 0; i < bulls.Count; i++)
        {
            if (bulls[i].isSet==false)
            {
                bulls[i].ShootBull(true, outBullet);
                return;
            }
        }


        //Instantiate(bullet, outBullet.position, outBullet.rotation);
        //rezulNetManager.NetworkManager.BullInst(outBullet);
        //gunExitParticle.Play();
        //Photon
        

    }
}
