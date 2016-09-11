using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Direct3D;
using TGC.Core.Geometry;
using TGC.Core.Textures;

namespace TGC.Group.Helpers
{
    public static class TerrainHelper
    {

        public static void updateTerreno(List<TgcPlane> parcelas, String mediaDir, Vector3 position)
        {
            int pos = parcelas.IndexOf(parcelas.Find(planoPosicion => estaDentroDe(position, planoPosicion)));

            TgcTexture tipoTerreno = parcelas.ElementAt(0).Texture;
            TgcPlane parcelaTransicion;
            var TransicionPastoArenaRightTexture = TgcTexture.createTexture(D3DDevice.Instance.Device, mediaDir + "Texturas\\TransicionPastoArenaRight.jpg");

            foreach (TgcPlane parcela in parcelas)
            {
                if(tipoTerreno != parcela.Texture)
                {
                    parcelaTransicion = new TgcPlane(new Vector3(parcela.Origin.X, parcela.Origin.Y, parcela.Origin.Z), new Vector3(2000, 0, 200), TgcPlane.Orientations.XZplane, TransicionPastoArenaRightTexture, 10f, 1f);
                    parcelaTransicion.render();
                    parcela.Size = new Vector3(parcela.Size.X, parcela.Size.Y, (parcela.Size.Z - 200));
                    parcela.Origin = new Vector3(parcela.Origin.X, parcela.Origin.Y, (parcela.Origin.Z + 200));
                    tipoTerreno = parcela.Texture;
                }
                parcela.render();
            }
        }

        private static Boolean estaDentroDe(Vector3 position, TgcPlane plano)
        {

            return plano.Origin != position;
        }
    }
}
