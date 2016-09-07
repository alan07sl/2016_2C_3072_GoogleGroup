using Microsoft.DirectX;
using Microsoft.DirectX.DirectInput;
using System;
using System.Collections.Generic;
using System.Drawing;
using TGC.Core.Direct3D;
using TGC.Core.Example;
using TGC.Core.Geometry;
using TGC.Core.Input;
using TGC.Core.SceneLoader;
using TGC.Core.Textures;
using TGC.Core.Utils;
using TGC.Group.Camera;

namespace TGC.Group.Model
{
    /// <summary>
    ///     Ejemplo para implementar el TP.
    ///     Inicialmente puede ser renombrado o copiado para hacer más ejemplos chicos, en el caso de copiar para que se
    ///     ejecute el nuevo ejemplo deben cambiar el modelo que instancia GameForm <see cref="Form.GameForm.InitGraphics()" />
    ///     line 97.
    /// </summary>
    public class GameModel : TgcExample
    {
        /// <summary>
        ///     Constructor del juego.
        /// </summary>
        /// <param name="mediaDir">Ruta donde esta la carpeta con los assets</param>
        /// <param name="shadersDir">Ruta donde esta la carpeta con los shaders</param>
        public GameModel(string mediaDir, string shadersDir) : base(mediaDir, shadersDir)
        {
            Category = Game.Default.Category;
            Name = Game.Default.Name;
            Description = Game.Default.Description;
        }

        //Caja que se muestra en el ejemplo.
        private TgcBox Box { get; set; }

        //Mesh de TgcLogo.
        private TgcMesh Mesh { get; set; }

        //Boleano para ver si dibujamos el boundingbox
        private bool BoundingBox { get; set; }

        private List<TgcMesh> meshes;
        private TgcMesh palmeraOriginal;
        private TgcMesh arbolBananas;
        private TgcMesh barrilPolvora;
        private TgcPlane suelo;

        /// <summary>
        ///     Se llama una sola vez, al principio cuando se ejecuta el ejemplo.
        ///     Escribir aquí todo el código de inicialización: cargar modelos, texturas, estructuras de optimización, todo
        ///     procesamiento que podemos pre calcular para nuestro juego.
        ///     Borrar el codigo ejemplo no utilizado.
        /// </summary>
        public override void Init()
        {
            //Crear suelo
            var pisoTexture = TgcTexture.createTexture(D3DDevice.Instance.Device, MediaDir + "Texturas\\pasto.jpg");
            suelo = new TgcPlane(new Vector3(-500, 0, -500), new Vector3(2000, 0, 2000), TgcPlane.Orientations.XZplane, pisoTexture, 10f, 10f);

            //Cargar modelo de palmera original
            var loader = new TgcSceneLoader();
            var scene =
                loader.loadSceneFromFile(MediaDir + "MeshCreator\\Meshes\\Vegetacion\\Palmera\\Palmera-TgcScene.xml");
            palmeraOriginal = scene.Meshes[0];

            //Cargar modelo de Arbol Bananas
            //var loader = new TgcSceneLoader();
            var scene2 =
                loader.loadSceneFromFile(MediaDir + "MeshCreator\\Meshes\\Vegetacion\\ArbolBananas\\ArbolBananas-TgcScene.xml");
            arbolBananas = scene2.Meshes[0];

            //Cargar modelo de Barril Polvora
            //var loader = new TgcSceneLoader();
            var scene3 =
                loader.loadSceneFromFile(MediaDir + "MeshCreator\\Meshes\\Objetos\\BarrilPolvora\\BarrilPolvora-TgcScene.xml");
            barrilPolvora = scene3.Meshes[0];

            //Crear varias instancias del modelo original, pero sin volver a cargar el modelo entero cada vez
            var rows = 2;
            var cols = 3;
            float offset = 250;
            meshes = new List<TgcMesh>();
            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < cols; j++)
                {
                    //Crear instancia de modelo
                    var instance = palmeraOriginal.createMeshInstance(palmeraOriginal.Name + i + "_" + j);
                    //No recomendamos utilizar AutoTransform, en juegos complejos se pierde el control. mejor utilizar Transformaciones con matrices.
                    instance.AutoTransformEnable = true;
                    //Desplazarlo
                    instance.move(i * offset, 0, j * offset);
                    //instance.Scale = new Vector3(0.25f, 0.25f, 0.25f);

                    meshes.Add(instance);
                }
            }

            //Crear varias instancias del modelo original, pero sin volver a cargar el modelo entero cada vez
            rows = 2;
            cols = 3;
            //float offset = 250;
            offset = 250;
            //meshes = new List<TgcMesh>();
            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < cols; j++)
                {
                    //Crear instancia de modelo
                    var instance = arbolBananas.createMeshInstance(arbolBananas.Name + i + "_" + j);
                    //No recomendamos utilizar AutoTransform, en juegos complejos se pierde el control. mejor utilizar Transformaciones con matrices.
                    instance.AutoTransformEnable = true;
                    //Desplazarlo
                    instance.move(i * offset + 500, 0, j * offset + 500);
                    instance.Scale = new Vector3(3.25f, 3.25f, 3.25f);

                    meshes.Add(instance);
                }
            }

            //Crear instancia de modelo
            var instanceB = barrilPolvora.createMeshInstance(barrilPolvora.Name);
            //No recomendamos utilizar AutoTransform, en juegos complejos se pierde el control. mejor utilizar Transformaciones con matrices.
            instanceB.AutoTransformEnable = true;
            //Desplazarlo
            instanceB.move(900, 0, 0);
            instanceB.Scale = new Vector3(1.5f, 1.5f, 1.5f);

            meshes.Add(instanceB);

            //Camara en primera persona
            Camara = new TgcFpsCamera(new Vector3(900f, 400f, 900f), Input);
        }

        /// <summary>
        ///     Se llama en cada frame.
        ///     Se debe escribir toda la lógica de computo del modelo, así como también verificar entradas del usuario y reacciones
        ///     ante ellas.
        /// </summary>
        public override void Update()
        {
            PreUpdate();
        }

        /// <summary>
        ///     Se llama cada vez que hay que refrescar la pantalla.
        ///     Escribir aquí todo el código referido al renderizado.
        ///     Borrar todo lo que no haga falta.
        /// </summary>
        public override void Render()
        {
            //Inicio el render de la escena, para ejemplos simples. Cuando tenemos postprocesado o shaders es mejor realizar las operaciones según nuestra conveniencia.
            PreRender();

            //Renderizar suelo
            suelo.render();

            //Renderizar instancias
            foreach (var mesh in meshes)
            {
                mesh.render();
            }

            PostRender();
        }

        /// <summary>
        ///     Se llama cuando termina la ejecución del ejemplo.
        ///     Hacer Dispose() de todos los objetos creados.
        ///     Es muy importante liberar los recursos, sobretodo los gráficos ya que quedan bloqueados en el device de video.
        /// </summary>
        public override void Dispose()
        {
            /*
            
            //Dispose de la caja.
            Box.dispose();
            //Dispose del mesh.
            Mesh.dispose();

            */

            suelo.dispose();

            //Al hacer dispose del original, se hace dispose automaticamente de todas las instancias
            palmeraOriginal.dispose();

            arbolBananas.dispose();

            barrilPolvora.dispose();
        }
    }
}