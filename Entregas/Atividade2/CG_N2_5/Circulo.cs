#define CG_OpenGL
#define CG_Debug

using CG_Biblioteca;
using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;
using System;

namespace gcgcg
{
    internal class Circulo : Objeto
    {

        // private Objeto objeto;
        private Ponto4D ponto = new Ponto4D();

        public Circulo(Objeto paiRef, float raio) : base(paiRef)
        {
            this.GerarCirculo(raio);
            Atualizar();
        }

        public void Atualizar()
        {
            base.ObjetoAtualizar();
        }

        // public Objeto GerarCirculo()
        // {
        //     for (int x = 0; x < 360; x += 5)
        //     {
        //         ponto = Matematica.GerarPtosCirculo(x, 0.5);
        //         GL.Enable(EnableCap.ProgramPointSize);
        //         GL.PointSize(5.0f);
        //         objeto.PontosAdicionar(ponto);
        //     }

        //     return objeto;
        // }

        private void GerarCirculo(float raio)
        {
            for (int x = 0; x < 360; x += 5)
            {
                ponto = Matematica.GerarPtosCirculo(x, raio);
                GL.Enable(EnableCap.ProgramPointSize);
                GL.PointSize(5.0f);
                base.PontosAdicionar(ponto);
                // move pontos para o quadrante escolhido
                // ponto.X += 0.3f;
                // ponto.Y += 0.3f;
            }

        }

    }
}
