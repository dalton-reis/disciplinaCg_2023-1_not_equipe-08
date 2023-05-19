#define CG_Debug

using CG_Biblioteca;
using OpenTK.Graphics.OpenGL4;

namespace gcgcg
{
  internal class Retangulo : Objeto
  {
    private Ponto4D ptoInfEsq;
    private Ponto4D ptoSupDir;


    public Retangulo(Objeto paiRef, Ponto4D ptoInfEsq, Ponto4D ptoSupDir) : base(paiRef)
    {
      PrimitivaTipo = PrimitiveType.Points;
      PrimitivaTamanho = 10;

      this.ptoInfEsq = ptoInfEsq;
      this.ptoSupDir = ptoSupDir;

      base.PontosAdicionar(ptoInfEsq);
      base.PontosAdicionar(new Ponto4D(ptoSupDir.X, ptoInfEsq.Y));
      base.PontosAdicionar(ptoSupDir);
      base.PontosAdicionar(new Ponto4D(ptoInfEsq.X, ptoSupDir.Y));
      Atualizar();
    }

    public void Atualizar()
    {

      base.ObjetoAtualizar();
    }
    
    public bool IsPontoFora(Ponto4D ponto){
      bool foraEsq = (ponto.X < ptoSupDir.X) || (ponto.Y < ptoSupDir.Y);
      bool foraDir = (ponto.X > ptoInfEsq.X) || (ponto.Y > ptoInfEsq.Y);

      return foraEsq || foraDir;
    }
  }
}
