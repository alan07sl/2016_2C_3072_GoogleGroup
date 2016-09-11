using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.SceneLoader;

namespace TGC.Group.Optimization
{
    internal class KdTreeNode
    {
        public KdTreeNode[] children;
        public TgcMesh[] models;

        //Corte realizado
        public float xCut;

        public float yCut;
        public float zCut;

        public bool isLeaf()
        {
            return children == null;
        }
    }
}
