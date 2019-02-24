using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WanderingEarth
{
    /// <summary>
    /// 道具 Manager。
    /// </summary>
    class ItemManager : BaseManager<ItemManager>
    {
        public List<GameObject> meteorolites;
        public List<GameObject> meteorolitesPool;

        public override void Init()
        {
            meteorolites = new List<GameObject>();
            meteorolitesPool = new List<GameObject>();
            GameObject obj = Resources.Load<GameObject>("Prefabs/meteorolites/1");
            GameObject meteorolite = Instantiate(obj);
            GameObject meteoroliteNode = GameObject.Find("ItemNodes");
            meteorolite.transform.parent = meteoroliteNode.transform;
            meteorolite.SetActive(false);
            meteorolitesPool.Add(meteorolite);

            obj = Resources.Load<GameObject>("Prefabs/meteorolites/2");
            meteorolite = Instantiate(obj);
            meteorolite.transform.parent = meteoroliteNode.transform;
            //meteorolite.SetActive(false);
            meteorolite.transform.position = new Vector3(100, 50, 0);
            meteorolitesPool.Add(meteorolite);
        }

        public override void Final()
        {
            foreach (GameObject meteorolite in meteorolites)
                GameObject.Destroy(meteorolite);
            foreach (GameObject meteorolite in meteorolitesPool)
                GameObject.Destroy(meteorolite);
        }

        public void ShowMeteorolite(Vector2 pos)
        {
            int length = meteorolitesPool.Count();
            if (length < 1)
                return;
            int index = UnityEngine.Random.Range(0, length);
            GameObject meteorolite = meteorolitesPool[index];
            meteorolitesPool.RemoveAt(index);
            meteorolite.SetActive(true);
            meteorolite.transform.position = new Vector3(pos.x, pos.y, 0);
            meteorolites.Add(meteorolite);
        }

        public void HidePlanet(GameObject meteorolite)
        {
            for (int i = 0; i < meteorolites.Count(); ++i)
            {
                if (meteorolites[i] == meteorolite)
                {
                    meteorolite.SetActive(false);
                    meteorolites.RemoveAt(i);
                    break;
                }
            }
            meteorolitesPool.Add(meteorolite);
        }
    }
}
