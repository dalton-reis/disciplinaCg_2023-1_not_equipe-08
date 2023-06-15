#nullable enable
using CG_Biblioteca;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using System;
using OpenTK.Mathematics;
using System.Collections.Generic;

//FIXME: padrão Singleton

namespace gcgcg
{
    public class Mundo : GameWindow
    {
        Objeto mundo;
        private char _rotuloAtual = '?';
        private Objeto? _objetoSelecionado;
        private Objeto _objetoParaRemover;
        private List<Objeto> _objetosExistentes;
        private bool _isNovoPoligono;
        private List<Ponto4D> _pontosPoligono;

        private Objeto objetoBbox = null;

        private readonly float[] _sruEixos =
        {
            -0.5f, 0.0f, 0.0f, /* X- */ 0.5f, 0.0f, 0.0f, /* X+ */
            0.0f, -0.5f, 0.0f, /* Y- */ 0.0f, 0.5f, 0.0f, /* Y+ */
            0.0f, 0.0f, -0.5f, /* Z- */ 0.0f, 0.0f, 0.5f, /* Z+ */
        };

        private int _vertexBufferObject_sruEixos;
        private int _vertexArrayObject_sruEixos;

        private Shader _shaderVermelha;
        private Shader _shaderVerde;
        private Shader _shaderAzul;
        private Shader _shaderAmarela;

        public Mundo(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
            mundo = new Objeto(null, ref _rotuloAtual);
        }

        private void Diretivas()
        {
#if DEBUG
            Console.WriteLine("Debug version");
#endif
#if RELEASE
    Console.WriteLine("Release version");
#endif
#if CG_Gizmo
            Console.WriteLine("#define CG_Gizmo  // debugar gráfico.");
#endif
#if CG_OpenGL
            Console.WriteLine("#define CG_OpenGL // render OpenGL.");
#endif
#if CG_DirectX
      Console.WriteLine("#define CG_DirectX // render DirectX.");
#endif
#if CG_Privado
            Console.WriteLine("#define CG_Privado // código do professor.");
#endif
            Console.WriteLine("__________________________________ \n");
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            Diretivas();

            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);

            #region Eixos: SRU

            _vertexBufferObject_sruEixos = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject_sruEixos);
            GL.BufferData(BufferTarget.ArrayBuffer, _sruEixos.Length * sizeof(float), _sruEixos,
                BufferUsageHint.StaticDraw);
            _vertexArrayObject_sruEixos = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject_sruEixos);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            _shaderVermelha = new Shader("Shaders/shader.vert", "Shaders/shaderVermelha.frag");
            _shaderVerde = new Shader("Shaders/shader.vert", "Shaders/shaderVerde.frag");
            _shaderAzul = new Shader("Shaders/shader.vert", "Shaders/shaderAzul.frag");
            _shaderAmarela = new Shader("Shaders/shader.vert", "Shaders/shaderAmarela.frag");

            #endregion
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit);

#if CG_Gizmo
            Sru3D();
#endif
            mundo.Desenhar();
            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            // ☞ 396c2670-8ce0-4aff-86da-0f58cd8dcfdc

            #region Teclado

            var input = KeyboardState;
            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }
            else
            {
                if (input.IsKeyPressed(Keys.Enter))
                {
                    _isNovoPoligono = false;
                }

                if (input.IsKeyPressed(Keys.D))
                {
                    if (_objetoParaRemover != null)
                    {
                        Objeto pai = _objetoParaRemover.GetObjetoPai();
                        pai.RemoveObjeto(_objetoParaRemover);

                        _objetosExistentes.Remove(_objetoParaRemover);

                        _objetoSelecionado = null;
                        _objetoParaRemover = null;
                    }
                }

                if (input.IsKeyPressed(Keys.G))
                {
                    mundo.GrafocenaImprimir("");
                }

                if (input.IsKeyPressed(Keys.P))
                {
                    System.Console.WriteLine(_objetoSelecionado.ToString());
                }

                if (input.IsKeyPressed(Keys.M))
                {
                    _objetoSelecionado.MatrizImprimir();
                }

                //TODO: não está atualizando a BBox com as transformações geométricas
                if (input.IsKeyPressed(Keys.I))
                {
                    _objetoSelecionado?.MatrizAtribuirIdentidade();
                }

                if (input.IsKeyPressed(Keys.Left))
                {
                    _objetoSelecionado?.MatrizTranslacaoXYZ(-0.05, 0, 0, _objetoSelecionado);
                }

                if (input.IsKeyPressed(Keys.Right))
                {
                    _objetoSelecionado?.MatrizTranslacaoXYZ(0.05, 0, 0, _objetoSelecionado);
                }

                if (input.IsKeyPressed(Keys.Up))
                {
                    _objetoSelecionado?.MatrizTranslacaoXYZ(0, 0.05, 0, _objetoSelecionado);
                }

                if (input.IsKeyPressed(Keys.Down))
                {
                    _objetoSelecionado?.MatrizTranslacaoXYZ(0, -0.05, 0, _objetoSelecionado);
                }

                if (input.IsKeyPressed(Keys.PageUp))
                {
                    _objetoSelecionado.MatrizEscalaXYZ(2, 2, 2);
                }

                if (input.IsKeyPressed(Keys.PageDown))
                {
                    _objetoSelecionado.MatrizEscalaXYZ(0.5, 0.5, 0.5);
                }

                if (input.IsKeyPressed(Keys.Home))
                {
                    _objetoSelecionado.MatrizEscalaXYZBBox(0.5, 0.5, 0.5);
                }

                if (input.IsKeyPressed(Keys.End))
                {
                    _objetoSelecionado.MatrizEscalaXYZBBox(2, 2, 2);
                }

                if (input.IsKeyPressed(Keys.D1))
                {
                    _objetoSelecionado.MatrizRotacao(10);
                }

                if (input.IsKeyPressed(Keys.D2))
                {
                    _objetoSelecionado.MatrizRotacao(-10);
                }

                if (input.IsKeyPressed(Keys.D3))
                {
                    _objetoSelecionado.MatrizRotacaoZBBox(10);
                }

                if (input.IsKeyPressed(Keys.D4))
                {
                    _objetoSelecionado.MatrizRotacaoZBBox(-10);
                }

                if (input.IsKeyPressed(Keys.S))
                {
                    PontoEmPoligono();
                }

                if (input.IsKeyPressed(Keys.R))
                {
                    _objetoSelecionado.shaderCor = _shaderVermelha;
                }

                if (input.IsKeyPressed(Keys.G))
                {
                    _objetoSelecionado.shaderCor = _shaderVerde;
                }

                if (input.IsKeyPressed(Keys.B))
                {
                    _objetoSelecionado.shaderCor = _shaderAzul;
                }

                if (input.IsKeyPressed(Keys.P))
                {
                    _isNovoPoligono = !_isNovoPoligono;
                }

                if (input.IsKeyPressed(Keys.E))
                {
                    AlteraVerticeProximo(true);
                }
            }

            #endregion

            #region Mouse

            Vector2i janela = this.ClientRectangle.Size;

            var mouse = MouseState;
            var xReal = mouse.X - (janela.X / 2);
            var yReal = (janela.Y / 2) - mouse.Y;

            xReal = (2 * xReal) / janela.X;
            yReal = (2 * yReal) / janela.Y;

            Ponto4D novoPonto = new Ponto4D(xReal, yReal);

            // adicao de poligonos
            if (_isNovoPoligono)
            {
                _objetoSelecionado.PontosAlterar(novoPonto, _objetoSelecionado.GetQtdPontos() - 1);
                _objetoSelecionado.ObjetoAtualizar();
            }

            if (mouse.IsButtonPressed(MouseButton.Left))
            {
                Objeto pai = _objetoSelecionado;

                if (_objetoSelecionado == null)
                {
                    pai = mundo;
                }

                if (!_isNovoPoligono)
                {
                    _objetoSelecionado =
                        new Poligono(pai, ref _rotuloAtual, new List<Ponto4D> { novoPonto, novoPonto });
                    _isNovoPoligono = true;
                    _objetosExistentes.Add(_objetoSelecionado);
                }
                else
                {
                    _objetoSelecionado.PontosAdicionar(novoPonto);
                    _objetoSelecionado.ObjetoAtualizar();
                }
            }

            // seleciona poligono para remover
            if (mouse.IsButtonPressed(MouseButton.Right))
            {
                foreach (var objeto in _objetosExistentes)
                {
                    if (objeto.IsSelecionado(novoPonto))
                    {
                        _objetoParaRemover = objeto;
                        _objetoSelecionado = objeto;
                        break;
                    }
                }

                AlteraVerticeProximo(false);
            }

            #endregion
        }

        private void PontoEmPoligono()
        {
            #region Mouse

            Vector2i janela = this.ClientRectangle.Size;

            var mouse = MouseState;
            var xReal = mouse.X - (janela.X / 2);
            var yReal = (janela.Y / 2) - mouse.Y;

            xReal = (2 * xReal) / janela.X;
            yReal = (2 * yReal) / janela.Y;

            Ponto4D pontoSel = new Ponto4D(xReal, yReal);

            #endregion

            if (_objetoSelecionado != null && Matematica.Dentro(_objetoSelecionado.Bbox(), pontoSel))
            {
                ApresentaBbox();
                return;
            }

            foreach (Objeto objeto in _objetosExistentes)
            {
                int N = 0;
                List<Ponto4D> pontos = objeto.GetPontos();
                Ponto4D fimL = new Ponto4D(janela.X, pontoSel.Y);
                for (var i = 0; i < pontos.Count - 1; i++)
                {
                    if (pontos[i].Y != pontos[i + 1].Y)
                    {
                        Ponto4D pontoInter = GetPontoInterseccao(pontos[i], pontos[i + 1], pontoSel, fimL);
                        if (pontoInter.X == pontoSel.X)
                        {
                            double slope1 = (pontoSel.Y - pontos[i].Y) / (pontoSel.X - pontos[i].X);
                            double slope2 = (pontos[i + 1].Y - pontos[i].Y) / (pontos[i + 1].X - pontos[i].X);

                            double tolerancia = 0.000001;

                            if (Math.Abs(slope1 - slope2) < tolerancia &&
                                ((pontoSel.X >= Math.Min(pontos[i].X, pontos[i + 1].X) &&
                                  pontoSel.X <= Math.Max(pontos[i].X, pontos[i + 1].X)) &&
                                 (pontoSel.Y >= Math.Min(pontos[i].Y, pontos[i + 1].Y) &&
                                  pontoSel.Y <= Math.Max(pontos[i].Y, pontos[i + 1].Y))))
                            {
                                return;
                            }
                        }
                        else if ((pontoInter.X > pontoSel.X) &&
                                 (pontoInter.Y > Math.Min(pontos[i].Y, pontos[i + 1].Y)) &&
                                 (pontoInter.Y <= Math.Max(pontos[i].Y, pontos[i + 1].Y)))
                        {
                            N = N + 1;
                        }
                    }
                    else if (pontoSel.Y == pontos[i].Y &&
                             pontoSel.X >= Math.Min(pontos[i].X, pontos[i + 1].X) &&
                             pontoSel.X <= Math.Max(pontos[i].X, pontos[i + 1].X))
                    {
                        return;
                    }
                }

                if (N % 2 == 1)
                {
                    _objetoSelecionado = objeto;
                    _objetoParaRemover = objeto;
                    ApresentaBbox();
                    return;
                }
                else
                {
                    _objetoSelecionado = null;
                    _objetoParaRemover = null;
                }
            }
        }

        private Ponto4D GetPontoInterseccao(Ponto4D ponto1, Ponto4D ponto2, Ponto4D pontoSel, Ponto4D fimL)
        {
            double denominator = ((ponto1.X - ponto2.X) * (pontoSel.Y - fimL.Y)) -
                                 ((ponto1.Y - ponto2.Y) * (pontoSel.X - fimL.X));

            if (denominator == 0)
            {
                System.Console.WriteLine("As retas são paralelas e não se interceptam.");
            }

            double x = (((ponto1.X * ponto2.Y) - (ponto1.Y * ponto2.X)) * (pontoSel.X - fimL.X) -
                        (ponto1.X - ponto2.X) * ((pontoSel.X * fimL.Y) - (pontoSel.Y * fimL.X))) / denominator;
            double y = (((ponto1.X * ponto2.Y) - (ponto1.Y * ponto2.X)) * (pontoSel.Y - fimL.Y) -
                        (ponto1.Y - ponto2.Y) * ((pontoSel.X * fimL.Y) - (pontoSel.Y * fimL.X))) / denominator;

            return new Ponto4D(x, y);
        }

        private void ApresentaBbox()
        {
            BBox bBox = _objetoSelecionado.Bbox();
            List<Ponto4D> pontosLista = new List<Ponto4D>();
            pontosLista.Add(new Ponto4D(bBox.obterMaiorX, bBox.obterMaiorY));
            pontosLista.Add(new Ponto4D(bBox.obterMaiorX, bBox.obterMenorY));
            pontosLista.Add(new Ponto4D(bBox.obterMenorX, bBox.obterMenorY));
            pontosLista.Add(new Ponto4D(bBox.obterMenorX, bBox.obterMaiorY));
            char rotulo = '-';
            objetoBbox = new Poligono(null, ref rotulo, pontosLista);
            objetoBbox.shaderCor = _shaderAmarela;
            _objetoSelecionado.FilhoAdicionar(objetoBbox);
            _objetoSelecionado.ObjetoAtualizar();
        }

        protected void AlteraVerticeProximo(bool isDelete)
        {
            float menorDistancia = float.MaxValue;
            int indiceRemover = -1;
            Vector2 mousePonto = ObterMousePosition();
            if (_objetoSelecionado != null && _objetoSelecionado is Poligono)
            {
                Poligono poligonoSelecionado = (Poligono)_objetoSelecionado;
                List<Vector2> vertices = poligonoSelecionado.GetVertices();

                if (vertices.Count > 0)
                {
                    for (int i = 0; i < vertices.Count - 1; i++)
                    {
                        float distancia = Vector2.Distance(vertices[i], mousePonto);

                        if (distancia < menorDistancia)
                        {
                            menorDistancia = distancia;
                            indiceRemover = i;
                        }
                    }
                }
            }

            if (indiceRemover < 0)
            {
                return;
            }

            if (isDelete)
            {
                _objetoSelecionado.RemoverPonto(indiceRemover);
            }
            else
            {
                Ponto4D ponto = new Ponto4D(mousePonto.X, mousePonto.Y);
                _objetoSelecionado.PontosAlterar(ponto, indiceRemover);
            }

            _objetoSelecionado.ObjetoAtualizar();
        }


        protected Vector2 ObterMousePosition()
        {
            #region Mouse

            Vector2i janela = this.ClientRectangle.Size;

            var mouse = MouseState;
            var xReal = mouse.X - (janela.X / 2);
            var yReal = (janela.Y / 2) - mouse.Y;

            xReal = (2 * xReal) / janela.X;
            yReal = (2 * yReal) / janela.Y;

            Vector2 pontoMouse = new Vector2(xReal, yReal);
            return pontoMouse;

            #endregion
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Size.X, Size.Y);
        }

        protected override void OnUnload()
        {
            mundo.OnUnload();

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);

            GL.DeleteBuffer(_vertexBufferObject_sruEixos);
            GL.DeleteVertexArray(_vertexArrayObject_sruEixos);

            GL.DeleteProgram(_shaderVermelha.Handle);
            GL.DeleteProgram(_shaderVerde.Handle);
            GL.DeleteProgram(_shaderAzul.Handle);

            base.OnUnload();
        }

#if CG_Gizmo
        private void Sru3D()
        {
#if CG_OpenGL && !CG_DirectX
            var transform = Matrix4.Identity;
            GL.BindVertexArray(_vertexArrayObject_sruEixos);
            // EixoX
            _shaderVermelha.SetMatrix4("transform", transform);
            _shaderVermelha.Use();
            GL.DrawArrays(PrimitiveType.Lines, 0, 2);
            // EixoY
            _shaderVerde.SetMatrix4("transform", transform);
            _shaderVerde.Use();
            GL.DrawArrays(PrimitiveType.Lines, 2, 2);
            // EixoZ
            _shaderAzul.SetMatrix4("transform", transform);
            _shaderAzul.Use();
            GL.DrawArrays(PrimitiveType.Lines, 4, 2);
#elif CG_DirectX && !CG_OpenGL
      Console.WriteLine(" .. Coloque aqui o seu código em DirectX");
#elif (CG_DirectX && CG_OpenGL) || (!CG_DirectX && !CG_OpenGL)
      Console.WriteLine(" .. ERRO de Render - escolha OpenGL ou DirectX !!");
#endif
        }
#endif
    }
}