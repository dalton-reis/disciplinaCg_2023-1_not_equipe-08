#define CG_Debug

using CG_Biblioteca;
using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;

namespace gcgcg
{
  internal class Circulo : Objeto
  {

    public Circulo(Objeto paiRef) : base(paiRef)
    {
      PrimitivaTipo = PrimitiveType.LineLoop;
      FormaCircunferencia();
    }

    public void FormaCircunferencia()
    {
      Ponto4D ponto = new Ponto4D();
      for(int x=0; x<360; x+=5){
        ponto = Matematica.GerarPtosCirculo(x, 0.5);
        GL.Enable(EnableCap.ProgramPointSize);
        GL.PointSize(5.0f);
        base.PontosAdicionar(ponto);
      }
      base.Atualizar();
    }

#if CG_Debug
    public override string ToString()
    {
      string retorno;
      retorno = "__ Objeto Circulo _ Tipo: " + PrimitivaTipo + " _ Tamanho: " + PrimitivaTamanho + "\n";
      retorno += base.ToString();
      return (retorno);
    }
#endif

  }
}
