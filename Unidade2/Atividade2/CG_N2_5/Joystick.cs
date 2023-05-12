#define CG_Debug

using System;
using CG_Biblioteca;
using OpenTK.Graphics.OpenGL4;

namespace gcgcg
{
    internal class Joystick : Objeto
    {
        private Ponto pontoCentro;
        private Circulo circuloMaior;
        private Circulo circuloMenor;
        private Retangulo retangulo;

        public Joystick(Objeto paiRef) : base(paiRef)
        {
            PrimitivaTipo = PrimitiveType.Points;
            PrimitivaTamanho = 20;

            this.pontoCentro = new Ponto(null, new Ponto4D());
            this.pontoCentro.PrimitivaTamanho = 10;
            base.FilhoAdicionar(pontoCentro);

            this.circuloMaior = new Circulo(null, 0.3f);
            base.FilhoAdicionar(circuloMaior);

            this.circuloMenor = new Circulo(null, 0.1f);
            base.FilhoAdicionar(circuloMenor);

            double ponto = Matematica.GerarPtosCirculoSimetrico(0.3f);
            Ponto4D ptoDireita = new Ponto4D(ponto, ponto, 0.0, 1.0);
            Ponto4D ptoEsquerda = new Ponto4D(-ponto, -ponto, 0.0, 1.0);
            this.retangulo = new Retangulo(null, new Ponto4D(ptoDireita), new Ponto4D(ptoEsquerda));
            this.retangulo.PrimitivaTipo = PrimitiveType.LineLoop;
            base.FilhoAdicionar(retangulo);

            Atualizar();
        }

        public void Atualizar()
        {
            base.ObjetoAtualizar();
        }

        internal void Mover(Ponto4D novaPos)
        {
            if (this.retangulo.IsPontoFora(this.pontoCentro.posAtual))
            {
                this.retangulo.shaderCor = new Shader("Shaders/shader.vert", "Shaders/shaderVermelha.frag");
            }
            else
            {
                this.retangulo.shaderCor = new Shader("Shaders/shader.vert", "Shaders/shaderVerde.frag");
            }

            this.pontoCentro.Mover(novaPos);
            this.circuloMenor.Mover(novaPos);
        }
    }
}
