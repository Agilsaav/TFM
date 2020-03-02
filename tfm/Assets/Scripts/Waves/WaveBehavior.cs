using System.Collections;
using UnityEngine;

namespace WavesBehavior
{
    public class WaveBehavior : MonoBehaviour
    {
        [SerializeField] Vector3 initialRadius;
        [SerializeField] float maxDist = 50f;
        [SerializeField] float step = 1.0f;
        [SerializeField] float waveSeconds = 0.01f;
        [SerializeField] float alphaPercentatgeChange = 0.5f;
        [SerializeField] float alphaStep = 0.1f;

        [SerializeField] bool MainWave = false;


        Renderer m_ObjectRenderer;
        float distanceAlpha;
        bool backFaceActive = false;

        private void Awake()
        {
            m_ObjectRenderer = GetComponent<Renderer>();
        }

        private void Start()
        {
            if (MainWave) BackFaceCuling();
            transform.localScale = initialRadius;
            distanceAlpha = maxDist * alphaPercentatgeChange;

            StartCoroutine(ChangeWaveRadius());
        }

        private void Update()
        {
            ChangeCullling();
            if (transform.localScale.x >= distanceAlpha) ChangeAlpha();
            if (transform.localScale.x >= maxDist) Destroy(gameObject);
        }


        public void SetEmissionIntensity(float intensity)
        {
            Color col = m_ObjectRenderer.material.GetColor("_Emission");
            m_ObjectRenderer.material.SetColor("_Emission", intensity * col);
        }

        private IEnumerator ChangeWaveRadius()
        {
            while (transform.localScale.x <= maxDist)
            {
                transform.localScale += new Vector3(step, step, step);
                yield return new WaitForSeconds(waveSeconds);
            }
        }

        private void ChangeCullling()
        {
            //Change BackFaceCulling if the camera is inside the wave 
            if (CheckInsideSphere(transform.localScale.x) && !backFaceActive && !MainWave) //Check Inside
            {
                backFaceActive = true;
                BackFaceCuling();
            }
            else if (!CheckInsideSphere(transform.localScale.x) && backFaceActive && !MainWave) // Check Outside
            {
                backFaceActive = false;
                BackFaceCuling();
            }
        }

        private void ChangeAlpha()
        {
            float currenTrans = m_ObjectRenderer.material.GetFloat("_transparency");
            if ((currenTrans - alphaStep) > 0.0f)
            {
                currenTrans -= alphaStep;
                m_ObjectRenderer.material.SetFloat("_transparency", currenTrans);
            }
            else
                m_ObjectRenderer.material.SetFloat("Vector1_B7C5EC10", 0.0f);
        }

        private bool CheckInsideSphere(float radius)
        {
            Vector3 pos = Camera.main.transform.position;
            Vector3 posSpawner = transform.position;

            if (Mathf.Pow(pos.x - posSpawner.x, 2) + Mathf.Pow(pos.y - posSpawner.y, 2) + Mathf.Pow(pos.z - posSpawner.z, 2) > Mathf.Pow(radius, 2)) return false;
            else return true;

        }

        private void BackFaceCuling()
        {
            var meshFilters = GetComponentsInChildren<MeshFilter>();
            for (int m = 0; m < meshFilters.Length; m++)
            {
                var mesh = meshFilters[m].mesh;
                for (int sm = 0; sm < mesh.subMeshCount; sm++)
                {
                    var vertices = mesh.vertices;
                    var uv = mesh.uv;
                    var normals = mesh.normals;
                    var szV = vertices.Length;
                    var newUv = new Vector2[szV];
                    var newNorms = new Vector3[szV];
                    for (var j = 0; j < szV; j++)
                    {
                        // revert the new ones
                        newNorms[j] = -normals[j];
                        newUv[j] = uv[j];
                    }
                    var triangles = mesh.GetTriangles(sm);
                    var szT = triangles.Length;
                    var newTris = new int[szT]; // double the triangles
                    for (var i = 0; i < szT; i += 3)
                    {
                        // save the new reversed triangle
                        var j = i;
                        newTris[j] = triangles[i];
                        newTris[j + 2] = triangles[i + 1];
                        newTris[j + 1] = triangles[i + 2];
                    }

                    mesh.uv = newUv;
                    mesh.normals = newNorms;
                    mesh.SetTriangles(newTris, sm); // assign triangles last!	
                }

            }
        }
    }

}