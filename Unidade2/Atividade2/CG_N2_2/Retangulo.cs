#define CG_Debug
#define CG_OpenGL
using OpenTK.Graphics.OpenGL4;
using CG_Biblioteca;

namespace gcgcg
{
internal class Retangulo : Objeto
{
    public Retangulo(Objeto paiRef, Ponto4D ptoInfEsq, Ponto4D ptoSupDir) : base(paiRef)
    {
        base.PrimitivaTamanho = 10;
        base.PontosAdicionar(ptoInfEsq);
        base.PontosAdicionar(new Ponto4D(ptoSupDir.X, ptoInfEsq.Y));
        base.PontosAdicionar(ptoSupDir);
        base.PontosAdicionar(new Ponto4D(ptoInfEsq.X, ptoSupDir.Y));
        this.Atualizar();
    }

    public void Atualizar()
    {
        base.ObjetoAtualizar();
    }

    public override string ToString()
    {
        string[] textArray1 = new string[] { "__ Objeto Retangulo _ Tipo: ", base.PrimitivaTipo.ToString(), " _ Tamanho: ", ((float) base.PrimitivaTamanho).ToString(), "\n" };
        return (string.Concat((string[]) textArray1) + base.ToString());
    }
}


}