#define CG_Debug

using CG_Biblioteca;
using OpenTK.Graphics.OpenGL4;

namespace gcgcg
{
  internal class SrPalito : Objeto
  {
    private Ponto4D origin;
    private float tamanho;
    private float angulo;

    public SrPalito(Objeto paiRef, Ponto4D origin, float tamanho, float angulo) : base(paiRef)
    {
      PrimitivaTipo = PrimitiveType.Lines;
      PrimitivaTamanho = tamanho;
      
      this.origin = origin;
      this.tamanho = tamanho;
      this.angulo = angulo;

      AddPoints();

      Atualizar();
    }

    private void AddPoints()
    {
      base.PontosAdicionar(origin);
      
      var head = Matematica.GerarPtosCirculo(angulo, tamanho) + origin;
      base.PontosAdicionar(head);
    }

    public void Atualizar()
    {
      base.ObjetoAtualizar();
    }

    public void Mover(float stepSize)
    {
      pontosLista.Clear();
      origin.X += stepSize;
      AddPoints();
      Atualizar();
    }

    public void Rotacao(float graus)
    {
      pontosLista.Clear();
      angulo += graus;
      AddPoints();
      Atualizar();
    }
    
    public void Escala(float tamanho)
    {
      pontosLista.Clear();
      this.tamanho += tamanho;
      AddPoints();
      Atualizar();
    }

#if CG_Debug
    public override string ToString()
    {
      string retorno;
      retorno = "__ Objeto Ponto _ Tipo: " + PrimitivaTipo + " _ Tamanho: " + PrimitivaTamanho + "\n";
      retorno += base.ImprimeToString();
      return (retorno);

    }
#endif

  }
}
