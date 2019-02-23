using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WanderingEarth
{
    /// <summary>
    /// 星球 Manager。
    /// </summary>
    class PlanetManager : BaseManager<PlanetManager>
    {
        public List<GameObject> planets;
        public List<GameObject> planetsPool;
        public int planetCount=5;
        public float maxMass = 100;
        public float minMass = 5;
        public float maxRho = 1.0f;
        public float minRho = 0.05f;

        public override void Init()
        {
            planets = new List<GameObject>();
            planetsPool = new List<GameObject>();
            GameObject planetNode=GameObject.Find("PlanetNodes");
            GameObject orgPlanet=(GameObject)Resources.Load("planet/planet");
            for (int i=0;i<planetCount;++i)
            {
                GameObject planet = Instantiate(orgPlanet);
                planet.transform.parent = planetNode.transform;
                planet.SetActive(false);
                planetsPool.Add(planet);
            }
        }

        public override void Final()
        {
            foreach (GameObject planet in planets)
                Destroy(planet);
        }

        public Vector2 GetPlanetsForce(Vector2 earthPos, float earthMass)
        {
            Vector2 force = new Vector2(0, 0);
            foreach (GameObject planet in planets)
            {
                Vector2 distanceVector = new Vector2(planet.transform.position.x, planet.transform.position.y) - earthPos;
                force += Utility.GetAttractiveForce(distanceVector, earthMass, planet.GetComponent<Rigidbody>().mass);
            }
            return force;
        }

        public void ShowPlanet(Vector2 pos)
        {
            int length = planetsPool.Count();
            int index = UnityEngine.Random.Range(0, length);
            GameObject planet = planetsPool[index];
            planetsPool.RemoveAt(index);
            planet.SetActive(true);
            planet.transform.position = new Vector3(pos.x, pos.y, 0);
            float mass= UnityEngine.Random.Range(minMass, maxMass);
            float rho= UnityEngine.Random.Range(minRho, maxRho);
            float scale = rho * mass;
            planet.transform.localScale = new Vector3(scale, scale, scale);
            planet.GetComponent<Rigidbody>().mass = mass;
            planets.Add(planet);
        }

        public void HidePlanet(GameObject planet)
        {
            for(int i=0;i<planets.Count();++i)
            {
                if(planets[i]==planet)
                {
                    planet.SetActive(false);
                    planets.RemoveAt(i);
                    break;
                }
            }
            planetsPool.Add(planet);
        }
    }
}
