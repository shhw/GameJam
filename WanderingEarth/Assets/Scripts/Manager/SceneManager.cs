﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WanderingEarth
{
    /// <summary>
    /// 场景 Manager。
    /// </summary>
    class SceneManager : BaseManager<SceneManager>
    {
        private GameObject earthObject;
        private EarthEntity earthEntity;
        private GameObject sceneNodes;
        private GameObject orgSceneObject;

        private List<GameObject> scenes;
        private List<GameObject> scenesPool;

        private int sceneCount = 9;
        private int _centerX = 0;
        private int _centerY = 0;
        private float _halfLength = 14;

        int _MaxSize = 6;

        int _xMin;
        int _yMin;
        int _xMax;
        int _yMax;

        int MIN_X = -10000;
        int MIN_Y = -10000;
        int MAX_X = 10000;
        int MAX_Y = 10000;

        public bool gameStartFlag = false;

        public override void Init()
        {
            scenes = new List<GameObject>();
            scenesPool = new List<GameObject>();
            sceneNodes = GameObject.Find("SceneNodes");
            orgSceneObject = Resources.Load<GameObject>("Prefabs/scene/SceneObject");
            for (int i = 0; i < sceneCount; ++i)
            {
                GameObject scene = Instantiate(orgSceneObject);
                scene.transform.parent = sceneNodes.transform;
                scene.SetActive(false);
                scenesPool.Add(scene);
            }
        }

        private void Update()
        {
            if (GetGameFlag())
            {
                Vector2 pos = GetEarthEntity().GetPosition();
                UpdateScenes((int)pos.x, (int)pos.y);
            }
        }

        public override void Final()
        {
            foreach (GameObject scene in scenes)
                GameObject.Destroy(scene);
            foreach (GameObject scenes in scenesPool)
                GameObject.Destroy(scenes);
        }

        public void InitScene()
        {
            ShowScene(0, 0);
            ShowScene(0, 14);
            gameStartFlag = true;
        }

        public void UpdateScenes(int newCenterX, int newCenterY)
        {
            for (int i = 0; i < scenes.Count; i++)
            {
                scenes[i].gameObject.SetActive(true);
            }

            if (_centerX == newCenterX
                && _centerY == newCenterY)
            {
                return;
            }

            if (Math.Abs(newCenterX % 14) != Math.Abs(_centerX % 14) || Math.Abs(newCenterY % 14) != Math.Abs(_centerY % 14))
            {
                _centerX = newCenterX;
                _centerY = newCenterY;
            }

            List<GameObject> releaseScenes = new List<GameObject>();

            for (int i = 0; i < scenes.Count; i++)
            {
                if (!InArea((int)scenes[i].transform.position.x, (int)scenes[i].transform.position.y))
                {
                    releaseScenes.Add(scenes[i]);
                }
            }

            for (int i = 0; i < releaseScenes.Count; i++)
            {
                HideScene(releaseScenes[i]);
            }

            _xMin = ((_centerX - (int)_halfLength) < MIN_X) ? MIN_X : (_centerX - (int)_halfLength);
            _xMax = ((_centerX + (int)_halfLength) > MAX_X) ? MAX_X : (_centerX + (int)_halfLength);
            _yMin = ((_centerY - (int)_halfLength) < MIN_Y) ? MIN_Y : (_centerY - (int)_halfLength);
            _yMax = ((_centerY + (int)_halfLength) > MAX_Y) ? MAX_Y : (_centerY + (int)_halfLength);

            for (int x = _xMin; x <= _xMax; x++)
            {
                for (int y = _yMin; y <= _yMax; y++)
                {
                    if (InArea(x, y) && x % 14 == 0 && y % 14 == 0)
                    {
                        ShowScene(x, y);
                    }
                }
            }
        }

        public bool InArea(int checkX, int checkY)
        {
            int localX = Math.Abs(_centerX - checkX);
            int localY = Math.Abs(_centerY - checkY);

            float half = 14;
            if (localX > half || localY > half)
            {
                return false;
            }

            return true;

            //int localX = Math.Abs(_centerX % 14);
            //int localY = Math.Abs(_centerY % 14);

            //int half = 14 / 2;
            //if (localX < half && localY < half)
            //{
            //    if (Math.Abs(_centerX) >= Math.Abs(checkX) && Math.Abs(_centerY) >= Math.Abs(checkY))
            //        return true;
            //}
            //else if (localX >= half && localY < half)
            //{
            //    if (Math.Abs(_centerX) <= Math.Abs(checkX) && Math.Abs(_centerY) >= Math.Abs(checkY))
            //        return true;
            //}
            //else if (localX < half && localY >= half)
            //{
            //    if (Math.Abs(_centerX) >= Math.Abs(checkX) && Math.Abs(_centerY) <= Math.Abs(checkY))
            //        return true;
            //}
            //else
            //{
            //    if (Math.Abs(_centerX) <= Math.Abs(checkX) && Math.Abs(_centerY) <= Math.Abs(checkY))
            //        return true;
            //}

            //return false;
        }

        public void ShowScene(int x, int y)
        {
            if (scenes.Count >= _MaxSize)
                return;

            foreach (GameObject _scene in scenes)
            {
                if (_scene.transform.position.x == x && _scene.transform.position.y == y)
                {
                    return;
                }
            }

            int index = UnityEngine.Random.Range(0, scenesPool.Count());
            GameObject scene = scenesPool[index];
            scenesPool.RemoveAt(index);
            scene.SetActive(true);
            scenes.Add(scene);

            scene.transform.position = new Vector3(x, y, 0);
        }

        public void HideScene(GameObject scene)
        {
            if (scenesPool.Count <= 0)
                return;
            for (int i = 0; i < scenes.Count(); ++i)
            {
                if (scenes[i] == scene)
                {
                    scene.SetActive(false);
                    scenes.RemoveAt(i);
                    break;
                }
            }
            scenesPool.Add(scene);
        }

        public EarthEntity GetEarthEntity()
        {
            if (earthEntity == null)
                earthEntity = GameObject.Find("EarthObject").GetComponent<EarthEntity>();
            return earthEntity;
        }

        public bool GetGameFlag()
        {
            return gameStartFlag;
        }

        public void SetGameStart(bool flag)
        {
            gameStartFlag = flag;
        }

        public float GetTravelDistance()
        {
            return GetEarthEntity().GetTravelDistance();
        }
    }
}
