using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ALIyerEdon
{
    public class Texture_Switch : MonoBehaviour
    {
        public Texture2D[] textures;
        public MeshRenderer[] targetRenderers;
        int id;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
                gameObject.SetActive(false);
        }
        // Update is called once per frame
        public void Switch_Texture()
        {
            for (int a = 0; a <= targetRenderers.Length - 1; a++)
                targetRenderers[a].sharedMaterials[1].SetTexture("_BaseMap", textures[id]);

            id++;

            if (id > textures.Length - 1)
                id = 0;

        }
    }
}