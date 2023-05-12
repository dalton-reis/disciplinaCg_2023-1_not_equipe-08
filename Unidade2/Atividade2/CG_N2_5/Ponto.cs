#define CG_Debug

using CG_Biblioteca;
using OpenTK.Graphics.OpenGL4;

namespace gcgcg
{
    internal class Ponto : Objeto
    {
        public Ponto4D posAtual { get; private set; }

        public Ponto(Objeto paiRef, Ponto4D pto) : base(paiRef)
        {
            PrimitivaTipo = PrimitiveType.Points;
            PrimitivaTamanho = 20;

            base.PontosAdicionar(pto);

            this.posAtual = pto;

            Atualizar();
        }

        public void Atualizar()
        {
            base.ObjetoAtualizar();
        }

        internal void Mover(Ponto4D novaPos)
        {
            this.posAtual += novaPos;
            base.pontosLista.Clear();
            base.PontosAdicionar(this.posAtual);
            this.Atualizar();
        }

    }
}
