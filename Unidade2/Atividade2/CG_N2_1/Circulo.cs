#define CG_OpenGL
#define CG_Debug

using CG_Biblioteca;
using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;
using System;

namespace gcgcg
{
    internal class Circulo
    {
        private Objeto objeto;
        private Ponto4D ponto = new Ponto4D();

        public Circulo(Objeto objeto)
        { 
            this.objeto = objeto;
        }

        public Objeto GetObjeto()
        {
            return this.objeto;
        }

        public Objeto GerarCirculo()
        {
            for (int x = 0; x < 360; x += 5)
            {
                ponto = Matematica.GerarPtosCirculo(x, 0.5);
                GL.Enable(EnableCap.ProgramPointSize);
                GL.PointSize(5.0f);
                objeto.PontosAdicionar(ponto);
            }

            return objeto;
        }

    }
}