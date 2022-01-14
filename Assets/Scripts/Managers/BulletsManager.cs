using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletsManager : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private List<GameObject> _bulletsList = new List<GameObject>();

    public void InstantiateBullet(Vector3 position, Vector3 direction)
    {
        GameObject chosenBullet = VerifyBulletPool();

        if(chosenBullet == null) 
        {
            GameObject newBullet = Instantiate(_bulletPrefab, position, Quaternion.identity, transform);
            newBullet.transform.up = direction;
            _bulletsList.Add(newBullet);
        }
        else
        {
            chosenBullet.transform.position = position;
            chosenBullet.transform.up = direction;
            chosenBullet.SetActive(true);
        }
    }

    GameObject VerifyBulletPool()
    {
        int count = _bulletsList.Count;
        for(int i = 0; i < count; i++)
        {
            if(_bulletsList[i].activeSelf) continue;

            return _bulletsList[i];
        }

        return null;
    }
}
