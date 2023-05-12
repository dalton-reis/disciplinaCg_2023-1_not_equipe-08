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
        private Ponto4D posAtual;

        private float raioAtual;

        private Ponto4D ponto = new Ponto4D();

        public Circulo(Objeto paiRef, float raio) : base(paiRef)
        {
            this.posAtual = new Ponto4D();
            this.raioAtual = raio;
            this.GerarCirculo(raio);
            Atualizar();
        }

        public void Atualizar()
        {
            base.ObjetoAtualizar();
        }

        private void GerarCirculo(float raio)
        {
            for (int x = 0; x < 360; x += 5)
            {
                ponto = Matematica.GerarPtosCirculo(x, raio) + this.posAtual;
                GL.Enable(EnableCap.ProgramPointSize);
                GL.PointSize(5.0f);
                base.PontosAdicionar(ponto);
            }
        }

        internal void Mover(Ponto4D novaPos)
        {
            this.posAtual += novaPos;
            base.pontosLista.Clear();
            this.GerarCirculo(this.raioAtual);
            this.Atualizar();
        }
    }
}