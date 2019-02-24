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
<<<<<<< HEAD
        public int planetCount=9;
        public float maxMass = 50;
        public float minMass = 20;
        public float maxRho = 0.02f;
        public float minRho = 0.015f;
=======
        public int planetCount = 9;
        public float maxMass = 20;
        public float minMass = 5;
        public float maxRho = 0.1f;
        public float minRho = 0.01f;
>>>>>>> 866b2907332800f9ab3a7b9335473b03f98446b0

        public override void Init()
        {
            planets = new List<GameObject>();
            planetsPool = new List<GameObject>();
            GameObject planetNode = GameObject.Find("PlanetNodes");
            for (int i = 0; i < planetCount; ++i)
            {
                string objPath = "Prefabs/planet/" + (i + 1).ToString();
                GameObject orgPlanet = Resources.Load<GameObject>(objPath);
                GameObject planet = Instantiate(orgPlanet);
                planet.transform.parent = planetNode.transform;
                planet.SetActive(false);
                planetsPool.Add(planet);
            }
        }

        public override void Final()
        {
            foreach (GameObject planet in planets)
                GameObject.Destroy(planet);
            foreach (GameObject planet in planetsPool)
                GameObject.Destroy(planet);
        }

        public Vector2 GetPlanetsForce(Vector2 earthPos, float earthMass)
        {
            Vector2 force = new Vector2(0, 0);
            foreach (GameObject planet in planets)
            {
                Vector2 distanceVector = new Vector2(planet.transform.position.x, planet.transform.position.y) - earthPos;
                force += Utility.GetAttractiveForce(distanceVector, earthMass, planet.GetComponent<Rigidbody2D>().mass);
            }
            return force;
        }

        public void ShowPlanet(Vector2 pos)
        {
            int length = planetsPool.Count();
            if (length < 1)
                return;
            int index = UnityEngine.Random.Range(0, length);
            GameObject planet = planetsPool[index];
            planetsPool.RemoveAt(index);
            planet.transform.position = new Vector3(pos.x, pos.y, 0);
            float mass = UnityEngine.Random.Range(minMass, maxMass);
            float rho = UnityEngine.Random.Range(minRho, maxRho);
            int picture = UnityEngine.Random.Range(1, 11);
            string picturePath = "Picture/planet/" + picture.ToString();
            float scale = rho * mass;
            planet.transform.localScale = new Vector3(scale, scale, scale);
            planet.GetComponent<Rigidbody2D>().mass = mass;
            planets.Add(planet);

            planet.SetActive(true);
        }

        public void HidePlanet(GameObject planet)
        {
            for (int i = 0; i < planets.Count(); ++i)
            {
                if (planets[i] == planet)
                {
                    planet.SetActive(false);
                    planets.RemoveAt(i);
                    planetsPool.Add(planet);
                    break;
                }
            }
        }
    }
}
