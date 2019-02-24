using System;
using System.Collections;
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

        private GameObject boomObject;
        private Animator boomAnim;

        private Vector2 curVelocity;
        private Vector2 prePlanetPos;

        private int sceneCount = 18;
        private int _centerX = 0;
        private int _centerY = 0;
        private float _halfLength = 14;

        int _MaxSize = 9;

        int _xMin;
        int _yMin;
        int _xMax;
        int _yMax;

        int MIN_X = -10000;
        int MIN_Y = -10000;
        int MAX_X = 10000;
        int MAX_Y = 10000;

        int MinPawnX = 5;
        int MaxPawnX = 10;
        int MinPawnY = 200;
        int MaxPawnY = 300;

        public bool gameStartFlag = false;

        public override void Init()
        {
            boomObject = GameObject.Find("Boom");
            boomObject.SetActive(false);
            boomAnim = boomObject.GetComponent<Animator>();

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

            GetEarthEntity().gameObject.SetActive(false);
        }

        private void Update()
        {
            if (GetGameFlag())
            {
                Vector2 pos = GetEarthEntity().GetPosition();
                UpdateScenes((int)pos.x, (int)pos.y);
                SetPlanets();
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
            GetEarthEntity().gameObject.SetActive(true);
            boomObject.SetActive(false);
            ShowScene(0, 0);
            ShowScene(14, 0);
            ShowScene(0, 14);
            ShowScene(14, 14);
            ShowScene(-14, 0);
            ShowScene(-14, 14);
            SetPlanets();
            gameStartFlag = true;
        }

        public void EndGame()
        {
            // 爆炸特效
            boomObject.transform.position = GetEarthEntity().GetPosition();
            boomObject.SetActive(true);

            StartCoroutine(WaitToShow(0.5f));
        }


        IEnumerator WaitToShow(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            UIManager.GetInstance().Show("Page_Restart");
        } 

        public void ResetGame()
        {
            // Reset
            for (int i = 0; i < PlanetManager.GetInstance().planets.Count; ++i)
            {
                GameObject planet = PlanetManager.GetInstance().planets[i];
                PlanetManager.GetInstance().HidePlanet(planet);
            }

            GetEarthEntity().Reset();
            UIManager.GetInstance().Show("Page_GameHUD");
            InitScene();
        }

        public void SetPlanets()
        {
            for (int i = 0; i < PlanetManager.GetInstance().planets.Count; ++i)
            {
                GameObject planet = PlanetManager.GetInstance().planets[i];
                if (Math.Abs(GetEarthEntity().GetPosition().y - planet.transform.position.y) > 50)
                {
                    PlanetManager.GetInstance().HidePlanet(planet);
                }
            }

            curVelocity = GetEarthVelocity();

            int L = 0;
            int R = 2;
            if (prePlanetPos.x - GetEarthEntity().GetPosition().x > 0)
            {
                L -= 2;
            }
            else
            {
                R += 2;
            }
            int flag = UnityEngine.Random.Range(L, R);
            float posX = UnityEngine.Random.Range(MinPawnX, MaxPawnX);
            posX = flag < 1 ? -posX : posX;
            float posY = UnityEngine.Random.Range(MinPawnX, MaxPawnX);

            Vector2 posOffset = new Vector2(posX, posY);
            Vector2 planetPos = GetEarthEntity().GetPosition() + posOffset;
            if (planetPos.y - prePlanetPos.y < 30)
                return;
            PlanetManager.GetInstance().ShowPlanet(planetPos);
            prePlanetPos = planetPos;
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

            float half = 28;
            if (localX > half || localY > half)
            {
                return false;
            }

            return true;

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

        public Vector2 GetEarthVelocity()
        {
            return GetEarthEntity().GetVelocityDir().normalized;
        }
    }
}
