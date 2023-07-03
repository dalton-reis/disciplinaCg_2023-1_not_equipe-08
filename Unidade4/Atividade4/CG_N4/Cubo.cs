//https://github.com/mono/opentk/blob/main/Source/Examples/Shapes/Old/Cube.cs

#define CG_Debug
using CG_Biblioteca;

namespace gcgcg
{
  internal class Cubo : Objeto
  {
    Ponto4D[] pontos;

    public Cubo(Objeto paiRef, ref char _rotulo) :
      this(paiRef, ref _rotulo, new Ponto4D(-0.5, -0.5), new Ponto4D(0.5, 0.5))
    { }

    public Cubo(Objeto paiRef, ref char _rotulo, Ponto4D ptoInfEsq, Ponto4D ptoSupDir) : base(paiRef, ref _rotulo)
    {
      pontos = new Ponto4D[]
      {
        new Ponto4D(-0.2, -0.2,  0.2), // 0 - frente baixo esq
        new Ponto4D( 0.2, -0.2,  0.2), // 1 - frente baixo dir
        new Ponto4D( 0.2,  0.2,  0.2), // 2 - frente cima dir
        new Ponto4D(-0.2,  0.2,  0.2), // 3 - frente cima esq
        new Ponto4D(-0.2, -0.2, -0.2), // 4 - atrás baixo esq
        new Ponto4D( 0.2, -0.2, -0.2), // 5 - atrás baixo dir
        new Ponto4D( 0.2,  0.2, -0.2), // 6 - atrás cima dir
        new Ponto4D(-0.2,  0.2, -0.2)  // 7 - atrás cima esq
      };

      // Frente
      base.PontosAdicionar(pontos[2]);
      base.PontosAdicionar(pontos[1]);
      base.PontosAdicionar(pontos[0]);
      base.PontosAdicionar(pontos[3]);

      // Cima
      base.PontosAdicionar(pontos[3]);
      base.PontosAdicionar(pontos[2]);
      base.PontosAdicionar(pontos[6]);
      base.PontosAdicionar(pontos[7]);

      // Atrás
      base.PontosAdicionar(pontos[7]);
      base.PontosAdicionar(pontos[6]);
      base.PontosAdicionar(pontos[5]);
      base.PontosAdicionar(pontos[4]);

      // Esquerda
      base.PontosAdicionar(pontos[4]);
      base.PontosAdicionar(pontos[7]);
      base.PontosAdicionar(pontos[3]);
      base.PontosAdicionar(pontos[0]);

      // Baixo
      base.PontosAdicionar(pontos[0]);
      base.PontosAdicionar(pontos[4]);
      base.PontosAdicionar(pontos[5]);
      base.PontosAdicionar(pontos[1]);
      
      // Direita
      base.PontosAdicionar(pontos[1]);
      base.PontosAdicionar(pontos[5]);
      base.PontosAdicionar(pontos[6]);
      base.PontosAdicionar(pontos[2]);

      Atualizar();
    }

    private void Atualizar()
    {

      base.ObjetoAtualizar();
    }
    

#if CG_Debug
    public override string ToString()
    {
      string retorno;
      retorno = "__ Objeto Cubo _ Tipo: " + PrimitivaTipo + " _ Tamanho: " + PrimitivaTamanho + "\n";
      retorno += base.ImprimeToString();
      return (retorno);
    }
#endif

  }
}
