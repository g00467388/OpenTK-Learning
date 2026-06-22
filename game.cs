using System.Diagnostics;
using gl.shaders;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace gl;

public class Game : GameWindow
{
    private readonly Stopwatch _time = new Stopwatch();
    private int _vertexBufferObject;
    private int _vertexArrayObject;
    private Shader? _shader;
    private Texture? _texture;
    private int _elementBufferObject;

    private readonly float[] _borderColour = [1.0f, 0.0f, 0.0f, 1.0f];
    

    private readonly float[] _vertices =
    [ 
    //[vertex layout vec 3][texture cords vec 2]
        0.5f,  0.5f, 0.0f, 1.0f, 1.0f, // top right
        0.5f, -0.5f, 0.0f, 1.0f, 0.0f, // bottom right
        -0.5f, -0.5f, 0.0f, 0.0f, 0.0f, // bottom left
        -0.5f,  0.5f, 0.0f, 0.0f, 1.0f  // top left

    ];

    private readonly uint[] _indices =
    [
        0, 1, 2,
        0, 2, 3
    ];
    public Game(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
        : base(gameWindowSettings, nativeWindowSettings)
    {
    }

    protected override void OnLoad()
    {
        base.OnLoad();
        _time.Start();
        _vertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * _vertices.Length, _vertices,
            BufferUsageHint.StaticDraw);


        _vertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(_vertexArrayObject);
        
        _elementBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
        GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(uint) * _indices.Length, _indices, BufferUsageHint.StaticDraw);
        
        
        // attribute aPos
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, sizeof(float) * 5, 0);
        GL.EnableVertexAttribArray(0);

        // attribute aTexCord
        GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, sizeof(float) * 5, sizeof(float) * 3);
        GL.EnableVertexAttribArray(1);

     
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBorderColor, _borderColour);
        // TODO fix weird file path location 
        _shader = new Shader("../../../shaders/shader.vert", "../../../shaders/shader.frag");
        _shader.Use();

        _texture = new Texture("../../../Resources/container.jpg");
  
        
    }


    protected override void OnUnload()
    {
        // Cleanup
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.DeleteBuffer(_vertexBufferObject);

        GL.BindVertexArray(0);
        GL.DeleteVertexArray(_vertexArrayObject);


        _shader?.Dispose();
        base.OnUnload();
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);
        if (KeyboardState.IsKeyDown(Keys.Escape))
            Close();
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        GL.BindVertexArray(_vertexArrayObject);
        GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
        
        _texture?.Use();
        
        SwapBuffers();
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);

        GL.Viewport(0, 0, Size.X, Size.Y);
    }
}