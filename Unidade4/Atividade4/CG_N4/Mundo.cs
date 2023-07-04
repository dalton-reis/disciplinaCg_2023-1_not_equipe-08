#define CG_Gizmo  // debugar gráfico.
#define CG_OpenGL // render OpenGL.
// #define CG_DirectX // render DirectX.
// #define CG_Privado // código do professor.

using CG_Biblioteca;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using System;
using OpenTK.Mathematics;

//FIXME: padrão Singleton

namespace gcgcg
{
    public class Mundo : GameWindow
    {
        private readonly float[] _vertices =
        {
            // Positions          Normals              Texture coords
            -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  0.0f, 0.0f,
             0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  1.0f, 0.0f,
             0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  1.0f, 1.0f,
             0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  1.0f, 1.0f,
            -0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  0.0f, 1.0f,
            -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  0.0f, 0.0f,

            -0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  0.0f, 0.0f,
             0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  1.0f, 0.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  1.0f, 1.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  1.0f, 1.0f,
            -0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  0.0f, 1.0f,
            -0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  0.0f, 0.0f,

            -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,  1.0f, 0.0f,
            -0.5f,  0.5f, -0.5f, -1.0f,  0.0f,  0.0f,  1.0f, 1.0f,
            -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,  0.0f, 1.0f,
            -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,  0.0f, 1.0f,
            -0.5f, -0.5f,  0.5f, -1.0f,  0.0f,  0.0f,  0.0f, 0.0f,
            -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,  1.0f, 0.0f,

             0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f,  1.0f, 0.0f,
             0.5f,  0.5f, -0.5f,  1.0f,  0.0f,  0.0f,  1.0f, 1.0f,
             0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,  0.0f, 1.0f,
             0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,  0.0f, 1.0f,
             0.5f, -0.5f,  0.5f,  1.0f,  0.0f,  0.0f,  0.0f, 0.0f,
             0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f,  1.0f, 0.0f,

            -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,  0.0f, 1.0f,
             0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,  1.0f, 1.0f,
             0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,  1.0f, 0.0f,
             0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,  1.0f, 0.0f,
            -0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,  0.0f, 0.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,  0.0f, 1.0f,

            -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,  0.0f, 1.0f,
             0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,  1.0f, 1.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,  1.0f, 0.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,  1.0f, 0.0f,
            -0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,  0.0f, 0.0f,
            -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,  0.0f, 1.0f
        };

        private readonly Vector3[] _pointLightPositions =
        {
            new Vector3(0.7f, 0.2f, 2.0f),
            new Vector3(2.3f, -3.3f, -4.0f),
            new Vector3(-4.0f, 2.0f, -12.0f),
            new Vector3(0.0f, 0.0f, -3.0f)
        };

        private readonly Vector3 _posicaoLuz = new Vector3(1.2f, 1.0f, 2.0f);

        private int _vertexBufferObject;
        private int _cubo;
        private bool rotateCube;

        private Shader _shaderInicial;
        private Shader _shaderLightingMaps;
        private Shader _shaderBasicLighting;
        private Shader _shaderDirectionalLight;
        private Shader _shaderPointLight;
        private Shader _shaderSpotlight;
        private Shader _shaderMultipleLight;
        private ShaderType _currentShaderType = ShaderType.Initial;

        private Texture _texturaPrincipal;
        private Texture _texturaSpecular;

        private Camera _camera;
        private Vector3 _posicaoObjeto;
        private float _anguloRotacao;

        private Matrix4 model = Matrix4.Identity;

        public Mundo(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            GL.Enable(EnableCap.DepthTest);

            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            _cubo = GL.GenVertexArray();
            GL.BindVertexArray(_cubo);

            #region Shaders
            _shaderInicial = new Shader("Shaders/shaderTexture.vert", "Shaders/shaderTexture.frag");
            _shaderLightingMaps = new Shader("Shaders/shader.vert", "Shaders/lightingMap.frag");
            _shaderBasicLighting = new Shader("Shaders/shader.vert", "Shaders/basicLighting.frag");
            _shaderDirectionalLight = new Shader("Shaders/shader.vert", "Shaders/directionalLight.frag");
            _shaderPointLight = new Shader("Shaders/shader.vert", "Shaders/pointLight.frag");
            _shaderSpotlight = new Shader("Shaders/shader.vert", "Shaders/spotlight.frag");
            _shaderMultipleLight = new Shader("Shaders/shader.vert", "Shaders/multipleLight.frag");
            #endregion

            #region Texturas
            _texturaPrincipal = Texture.LoadFromFile("Texture/gato.png");
            _texturaSpecular = Texture.LoadFromFile("Texture/efeito.png");
            #endregion

            #region configuracao camera
            _posicaoObjeto = Vector3.Zero;
            _anguloRotacao = 0f;
            _camera = new Camera(Vector3.UnitZ * 5, Vector3.Zero, Size.X / (float)Size.Y);
            #endregion

        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.BindVertexArray(_cubo);
            _texturaPrincipal.Use(TextureUnit.Texture0);


            switch (_currentShaderType)
            {
                case ShaderType.BasicLighting:
                    InicializaShaderIluminacao();
                    rotacionaCubo();
                    _shaderBasicLighting.Use();

                    _shaderBasicLighting.SetMatrix4("model", Matrix4.Identity);
                    _shaderBasicLighting.SetMatrix4("view", _camera.GetViewMatrix());
                    _shaderBasicLighting.SetMatrix4("projection", _camera.GetProjectionMatrix());

                    _shaderBasicLighting.SetVector3("lightColor", new Vector3(1.0f, 1.0f, 1.0f));
                    _shaderBasicLighting.SetVector3("lightPos", _posicaoLuz);
                    _shaderBasicLighting.SetVector3("viewPos", _camera.Position);

                    break;
                case ShaderType.LightingMaps:
                    InicializaShaderIluminacao();
                    rotacionaCubo();

                    _texturaSpecular.Use(TextureUnit.Texture1);
                    _shaderLightingMaps.Use();

                    _shaderLightingMaps.SetMatrix4("model", model);
                    _shaderLightingMaps.SetMatrix4("view", _camera.GetViewMatrix());
                    _shaderLightingMaps.SetMatrix4("projection", _camera.GetProjectionMatrix());

                    _shaderLightingMaps.SetVector3("viewPos", _camera.Position);

                    _shaderLightingMaps.SetInt("material.diffuse", 0);
                    _shaderLightingMaps.SetInt("material.specular", 1);
                    _shaderLightingMaps.SetVector3("material.specular", new Vector3(0.5f, 0.5f, 0.5f));
                    _shaderLightingMaps.SetFloat("material.shininess", 32.0f);

                    _shaderLightingMaps.SetVector3("light.position", _posicaoLuz);
                    _shaderLightingMaps.SetVector3("light.ambient", new Vector3(0.2f));
                    _shaderLightingMaps.SetVector3("light.diffuse", new Vector3(0.5f));
                    _shaderLightingMaps.SetVector3("light.specular", new Vector3(1.0f));
                    break;
                case ShaderType.DirectionalLight:
                    InicializaShaderIluminacao();
                    rotacionaCubo();

                    _texturaSpecular.Use(TextureUnit.Texture1);
                    _shaderDirectionalLight.Use();

                    _shaderDirectionalLight.SetMatrix4("view", _camera.GetViewMatrix());
                    _shaderDirectionalLight.SetMatrix4("projection", _camera.GetProjectionMatrix());

                    _shaderDirectionalLight.SetVector3("viewPos", _camera.Position);

                    _shaderDirectionalLight.SetInt("material.diffuse", 0);
                    _shaderDirectionalLight.SetInt("material.specular", 1);
                    _shaderDirectionalLight.SetVector3("material.specular", new Vector3(0.5f, 0.5f, 0.5f));
                    _shaderDirectionalLight.SetFloat("material.shininess", 32.0f);

                    // (-0.2f, -1.0f, -0.3f) | (-0.2, -1.0, -0.5f)
                    _shaderDirectionalLight.SetVector3("light.direction", new Vector3(-0.2f, -1.0f, -0.3f));
                    _shaderDirectionalLight.SetVector3("light.ambient", new Vector3(0.2f));
                    _shaderDirectionalLight.SetVector3("light.diffuse", new Vector3(0.5f));
                    _shaderDirectionalLight.SetVector3("light.specular", new Vector3(1.0f));

                    _shaderDirectionalLight.SetMatrix4("model", model);
                    break;
                case ShaderType.PointLight:
                    InicializaShaderIluminacao();
                    rotacionaCubo();

                    _texturaSpecular.Use(TextureUnit.Texture1);
                    _shaderPointLight.Use();

                    _shaderPointLight.SetMatrix4("view", _camera.GetViewMatrix());
                    _shaderPointLight.SetMatrix4("projection", _camera.GetProjectionMatrix());

                    _shaderPointLight.SetVector3("viewPos", _camera.Position);

                    _shaderPointLight.SetInt("material.diffuse", 0);
                    _shaderPointLight.SetInt("material.specular", 1);
                    _shaderPointLight.SetVector3("material.specular", new Vector3(0.5f, 0.5f, 0.5f));
                    _shaderPointLight.SetFloat("material.shininess", 32.0f);

                    _shaderPointLight.SetVector3("light.position", _posicaoLuz);
                    _shaderPointLight.SetFloat("light.constant", 1.0f);
                    _shaderPointLight.SetFloat("light.linear", 0.09f);
                    _shaderPointLight.SetFloat("light.quadratic", 0.032f);
                    _shaderPointLight.SetVector3("light.ambient", new Vector3(0.2f));
                    _shaderPointLight.SetVector3("light.diffuse", new Vector3(0.5f));
                    _shaderPointLight.SetVector3("light.specular", new Vector3(1.0f));

                    _shaderPointLight.SetMatrix4("model", model);
                    break;
                case ShaderType.Spotlight:
                    InicializaShaderIluminacao();
                    rotacionaCubo();

                    _texturaSpecular.Use(TextureUnit.Texture1);
                    _shaderSpotlight.Use();

                    _shaderSpotlight.SetMatrix4("view", _camera.GetViewMatrix());
                    _shaderSpotlight.SetMatrix4("projection", _camera.GetProjectionMatrix());

                    _shaderSpotlight.SetVector3("viewPos", _camera.Position);

                    _shaderSpotlight.SetInt("material.diffuse", 0);
                    _shaderSpotlight.SetInt("material.specular", 1);
                    _shaderSpotlight.SetVector3("material.specular", new Vector3(0.5f, 0.5f, 0.5f));
                    _shaderSpotlight.SetFloat("material.shininess", 32.0f);

                    _shaderSpotlight.SetVector3("light.position", _camera.Position);
                    _shaderSpotlight.SetVector3("light.direction", _camera.Front);
                    _shaderSpotlight.SetFloat("light.cutOff", MathF.Cos(MathHelper.DegreesToRadians(12.5f)));
                    _shaderSpotlight.SetFloat("light.outerCutOff", MathF.Cos(MathHelper.DegreesToRadians(17.5f)));
                    _shaderSpotlight.SetFloat("light.constant", 1.0f);
                    _shaderSpotlight.SetFloat("light.linear", 0.09f);
                    _shaderSpotlight.SetFloat("light.quadratic", 0.032f);
                    _shaderSpotlight.SetVector3("light.ambient", new Vector3(0.2f));
                    _shaderSpotlight.SetVector3("light.diffuse", new Vector3(0.5f));
                    _shaderSpotlight.SetVector3("light.specular", new Vector3(1.0f));

                    _shaderSpotlight.SetMatrix4("model", model);
                    break;
                case ShaderType.MultipleLight:
                    InicializaShaderIluminacao();
                    rotacionaCubo();

                    _texturaSpecular.Use(TextureUnit.Texture1);
                    _shaderMultipleLight.Use();

                    _shaderMultipleLight.SetMatrix4("view", _camera.GetViewMatrix());
                    _shaderMultipleLight.SetMatrix4("projection", _camera.GetProjectionMatrix());

                    _shaderMultipleLight.SetVector3("viewPos", _camera.Position);

                    _shaderMultipleLight.SetInt("material.diffuse", 0);
                    _shaderMultipleLight.SetInt("material.specular", 1);
                    _shaderMultipleLight.SetVector3("material.specular", new Vector3(0.5f, 0.5f, 0.5f));
                    _shaderMultipleLight.SetFloat("material.shininess", 32.0f);

                    // Directional light
                    _shaderMultipleLight.SetVector3("dirLight.direction", new Vector3(-0.2f, -1.0f, -0.3f));
                    _shaderMultipleLight.SetVector3("dirLight.ambient", new Vector3(0.05f, 0.05f, 0.05f));
                    _shaderMultipleLight.SetVector3("dirLight.diffuse", new Vector3(0.4f, 0.4f, 0.4f));
                    _shaderMultipleLight.SetVector3("dirLight.specular", new Vector3(0.5f, 0.5f, 0.5f));

                    // Point lights
                    for (int i = 0; i < _pointLightPositions.Length; i++)
                    {
                        _shaderMultipleLight.SetVector3($"pointLights[{i}].position", _pointLightPositions[i]);
                        _shaderMultipleLight.SetVector3($"pointLights[{i}].ambient", new Vector3(0.05f, 0.05f, 0.05f));
                        _shaderMultipleLight.SetVector3($"pointLights[{i}].diffuse", new Vector3(0.8f, 0.8f, 0.8f));
                        _shaderMultipleLight.SetVector3($"pointLights[{i}].specular", new Vector3(1.0f, 1.0f, 1.0f));
                        _shaderMultipleLight.SetFloat($"pointLights[{i}].constant", 1.0f);
                        _shaderMultipleLight.SetFloat($"pointLights[{i}].linear", 0.09f);
                        _shaderMultipleLight.SetFloat($"pointLights[{i}].quadratic", 0.032f);
                    }

                    // Spot light
                    _shaderMultipleLight.SetVector3("spotLight.position", _camera.Position);
                    _shaderMultipleLight.SetVector3("spotLight.direction", _camera.Front);
                    _shaderMultipleLight.SetVector3("spotLight.ambient", new Vector3(0.0f, 0.0f, 0.0f));
                    _shaderMultipleLight.SetVector3("spotLight.diffuse", new Vector3(1.0f, 1.0f, 1.0f));
                    _shaderMultipleLight.SetVector3("spotLight.specular", new Vector3(1.0f, 1.0f, 1.0f));
                    _shaderMultipleLight.SetFloat("spotLight.constant", 1.0f);
                    _shaderMultipleLight.SetFloat("spotLight.linear", 0.09f);
                    _shaderMultipleLight.SetFloat("spotLight.quadratic", 0.032f);
                    _shaderMultipleLight.SetFloat("spotLight.cutOff", MathF.Cos(MathHelper.DegreesToRadians(12.5f)));
                    _shaderMultipleLight.SetFloat("spotLight.outerCutOff", MathF.Cos(MathHelper.DegreesToRadians(17.5f)));

                    _shaderMultipleLight.SetMatrix4("model", model);
                    break;

                case ShaderType.Initial:
                    InicializaShaderPadrao();
                    rotacionaCubo();
                    _shaderInicial.Use();
                    _shaderInicial.SetMatrix4("model", model);
                    _shaderInicial.SetMatrix4("view", _camera.GetViewMatrix());
                    _shaderInicial.SetMatrix4("projection", _camera.GetProjectionMatrix());
                    break;
            }

            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            _anguloRotacao += 0.0008f;
            float radius = 2;
            float x = (float)Math.Sin(_anguloRotacao) * radius;
            float z = (float)Math.Cos(_anguloRotacao) * radius;
            _camera.Position = new Vector3(x, 0f, z);
            _camera.Target = _posicaoObjeto;

            // ☞ 396c2670-8ce0-4aff-86da-0f58cd8dcfdc   TODO: forma otimizada para teclado.
            #region Teclado
            var input = KeyboardState;
            if (input.IsKeyDown(Keys.Escape))
                Close();
            if (input.IsKeyDown(Keys.D0))
                _currentShaderType = ShaderType.Initial;
            if (input.IsKeyDown(Keys.D1))
                _currentShaderType = ShaderType.BasicLighting;
            if (input.IsKeyDown(Keys.D2))
                _currentShaderType = ShaderType.LightingMaps;
            if (input.IsKeyDown(Keys.D3))
                _currentShaderType = ShaderType.DirectionalLight;
            if (input.IsKeyDown(Keys.D4))
                _currentShaderType = ShaderType.PointLight;
            if (input.IsKeyDown(Keys.D5))
                _currentShaderType = ShaderType.Spotlight;
            if (input.IsKeyDown(Keys.D6))
                _currentShaderType = ShaderType.MultipleLight;
            if (input.IsKeyPressed(Keys.Space))
                rotateCube = !rotateCube;

            #endregion
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            _camera.Fov -= e.OffsetY;
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Size.X, Size.Y);
            _camera.AspectRatio = Size.X / (float)Size.Y;
        }

        private void InicializaShaderIluminacao()
        {
            var aPos = _shaderLightingMaps.GetAttribLocation("aPos");
            GL.EnableVertexAttribArray(aPos);
            GL.VertexAttribPointer(aPos, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);

            var aNormal = _shaderLightingMaps.GetAttribLocation("aNormal");
            GL.EnableVertexAttribArray(aNormal);
            GL.VertexAttribPointer(aNormal, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));

            var aTexCoords = _shaderLightingMaps.GetAttribLocation("aTexCoords");
            GL.EnableVertexAttribArray(aTexCoords);
            GL.VertexAttribPointer(aTexCoords, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));
        }

        private void InicializaShaderPadrao()
        {
            var aPosition = _shaderInicial.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(aPosition);
            GL.VertexAttribPointer(aPosition, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);

            var aTexCoord = _shaderInicial.GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(aTexCoord);
            GL.VertexAttribPointer(aTexCoord, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));
        }

        private void rotacionaCubo()
        {
            model = Matrix4.Identity;
            if (rotateCube)
            {
                model *= Matrix4.CreateTranslation(new Vector3(0.0f, 0.0f, 0.0f));
                float angle = 20.0f;
                model *= Matrix4.CreateFromAxisAngle(new Vector3(1.0f, 0.3f, 0.5f), angle);
            }
        }

    }
    public enum ShaderType
    {
        Initial,
        BasicLighting,
        LightingMaps,
        DirectionalLight,
        PointLight,
        Spotlight,
        MultipleLight
    }
}
