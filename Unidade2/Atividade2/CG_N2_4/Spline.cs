#define CG_Debug

using System;
using System.Collections.Generic;
using CG_Biblioteca;
using OpenTK.Graphics.OpenGL4;

namespace gcgcg
{
  internal class Spline : Objeto
  {
    private int pontosBezier;
    private double incrementoBezier;

    private readonly List<Ponto> pontosSpline;
    private readonly Shader shaderSpline;
    private readonly Shader shaderPontoSelecionado;

    private readonly List<SegReta> linhasSpline;
    private readonly Shader shaderLinhaSpline;
    private readonly Shader shaderLinhaBezier;

    private int pontoSelecionado;

    public Spline(Objeto paiRef) : base(paiRef)
    {
      PrimitivaTipo = PrimitiveType.Points;
      PrimitivaTamanho = 10;

      pontosBezier = 10;
      incrementoBezier = 1.0 / (double)pontosBezier;

      pontosSpline = new List<Ponto>();
      shaderSpline = new Shader("Shaders/shader.vert", "Shaders/shaderAzul.frag");
      shaderPontoSelecionado = new Shader("Shaders/shader.vert", "Shaders/shaderAzul.frag");

      linhasSpline = new List<SegReta>();
      shaderLinhaSpline = new Shader("Shaders/shader.vert", "Shaders/shaderAzul.frag");
      shaderLinhaBezier = new Shader("Shaders/shader.vert", "Shaders/shaderAzul.frag");

      ReiniciarSpline();
    }

    public void ReiniciarSpline()
    {
      pontosLista.Clear();
      linhasSpline.Clear();
      pontosSpline.Clear();

      AdicionaPontos();
      Atualizar();
    }

    private void AdicionaPontos()
    {
      var pontosX = new double[] { -0.5, -0.5, 0.5, 0.5 };
      var pontosY = new double[] { -0.5, 0.5, 0.5, -0.5 };
      for (var i = 0; i < pontosX.Length; i++)
      {
        pontosSpline.Add(GetPonto(pontosX[i], pontosY[i]));
      }

      AtualizaPontoSelecionado(0);
    }

    private Ponto GetPonto(double x = 0.0, double y = 0.0)
    {
      var point = new Ponto(null, new Ponto4D(x: x, y: y));
      point.PrimitivaTamanho = 20;
      point.shaderCor = shaderSpline;
      return point;
    }

    private void AdicionaLinhas()
    {
      for (var i = 0; i < pontosSpline.Count - 1; i++)
      {
        var line = new SegReta(null, pontosSpline[i].Position, pontosSpline[i + 1].Position);
        line.shaderCor = shaderLinhaSpline;
        linhasSpline.Add(line);
      }
    }

    private void AdicionaBezier()
    {
      var bezierPoints = new List<Ponto4D>();
      for (var t = 0.0; t <= 1; t += incrementoBezier)
      {
        bezierPoints.Add(CalculaBezier(t));
      }

      for (var i = 0; i < bezierPoints.Count - 1; i++)
      {
        var line = new SegReta(null, bezierPoints[i], bezierPoints[i + 1]);
        line.shaderCor = shaderLinhaBezier;
        linhasSpline.Add(line);
      }
    }

    private Ponto4D CalculaBezier(double t)
    {
      var coeff = (1 - t);
      var bezierX = 0.0;
      bezierX += coeff*coeff*coeff*pontosSpline[0].Position.X;
      bezierX += 3*t*coeff*coeff*pontosSpline[1].Position.X;
      bezierX += 3*t*t*coeff*pontosSpline[2].Position.X;
      bezierX += t*t*t*pontosSpline[3].Position.X;

      var bezierY = 0.0;
      bezierY += coeff*coeff*coeff*pontosSpline[0].Position.Y;
      bezierY += 3*t*coeff*coeff*pontosSpline[1].Position.Y;
      bezierY += 3*t*t*coeff*pontosSpline[2].Position.Y;
      bezierY += t*t*t*pontosSpline[3].Position.Y;
      return new Ponto4D(x: bezierX, y: bezierY);
    }

    public void Atualizar()
    {
      AdicionaLinhas();
      AdicionaBezier();

      base.ObjetoAtualizar();

      foreach (var point in pontosSpline)
      {
        point.ObjetoAtualizar();
      }

      foreach (var line in linhasSpline)
      {
        line.ObjetoAtualizar();
      }
    }

    public override void Desenhar()
    {
      base.Desenhar();

      foreach (var point in pontosSpline)
      {
        point.Desenhar();
      }

      foreach (var line in linhasSpline)
      {
        line.Desenhar();
      }
    }

    public void SelecionarProximoPonto()
    {
      AtualizaPontoSelecionado((pontoSelecionado + 1) % pontosSpline.Count);
    }

    private void AtualizaPontoSelecionado(int novoPontoSelecionado)
    {
      pontoSelecionado = novoPontoSelecionado;

      var indexAntigo = pontoSelecionado == 0 ? pontosSpline.Count - 1 : pontoSelecionado - 1;
      pontosSpline[indexAntigo].shaderCor = shaderSpline;
      pontosSpline[pontoSelecionado].shaderCor = shaderPontoSelecionado;
    }

    public void MoverPontoSelecionado(Ponto4D move)
    {
      pontosLista.Clear();
      linhasSpline.Clear();
      pontosSpline[pontoSelecionado].Move(move);
      Atualizar();
    }

    public void AdicionarPontoSpline()
    {
      pontosLista.Clear();
      linhasSpline.Clear();

      pontosBezier += 1;
      incrementoBezier = 1.0 / (double)pontosBezier;
      Atualizar();
    }

    public void RemoverPontoSpline()
    {
      pontosLista.Clear();
      linhasSpline.Clear();
      
      pontosBezier -= 1;
      if (pontosBezier <= 0) {
        pontosBezier = 1;
      }
      incrementoBezier = 1.0 / (double)pontosBezier;
      Atualizar();
    }

#if CG_Debug
    public override string ToString()
    {
      string retorno;
      retorno = "__ Objeto Spline _ Tipo: " + PrimitivaTipo + "\n";
      retorno += base.ImprimeToString();
      return (retorno);
    }
#endif

  }
}
