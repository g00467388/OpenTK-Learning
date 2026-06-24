using System.Runtime.InteropServices.JavaScript;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using StbImageSharp;

namespace gl.shaders;

public sealed class Shader : IDisposable
{
    private int Handle { get; set; }
    
    private readonly string _vertexPath;
    private readonly string _fragmentPath;
    private int _vertexShader;
    private int _fragmentShader;
    private bool _isDisposed;


    public Shader(string vertexPath, string fragmentPath)
    {
        _vertexPath = (vertexPath);
        _fragmentPath = (fragmentPath);
        Compile();
    }

    private void Compile()
    {
        var vertexShaderSource = File.ReadAllText(_vertexPath);
        var fragmentShaderSource = File.ReadAllText(_fragmentPath);
        _vertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(_vertexShader, vertexShaderSource);

        _fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(_fragmentShader, fragmentShaderSource);

        GL.CompileShader(_vertexShader);

        GL.GetShader(_vertexShader, ShaderParameter.CompileStatus, out int shaderStatus);
        if (shaderStatus == 0)
        {
            string infoLog = GL.GetShaderInfoLog(shaderStatus);
            Console.WriteLine(infoLog);
        }

        GL.CompileShader(_fragmentShader);

        GL.GetShader(_fragmentShader, ShaderParameter.CompileStatus, out int success);
        if (success == 0)
        {
            string infoLog = GL.GetShaderInfoLog(_fragmentShader);
            Console.WriteLine(infoLog);
        }

        Handle = GL.CreateProgram();

        GL.AttachShader(Handle, _vertexShader);
        GL.AttachShader(Handle, _fragmentShader);

        GL.LinkProgram(Handle);

        GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, out int programStatus);
        if (programStatus == 0)
        {
            string infoLog = GL.GetProgramInfoLog(Handle);
            Console.WriteLine(infoLog);
        }
        // clean up
        GL.DetachShader(Handle, _vertexShader);
        GL.DetachShader(Handle, _fragmentShader);
        GL.DeleteShader(_fragmentShader);
        GL.DeleteShader(_vertexShader);
    }

    public void Use()
    {
        GL.UseProgram(Handle);
        int location = GL.GetUniformLocation(Handle, "transform");
        // Transform position on X axis by +1
        Matrix4 transformation = Matrix4.CreateTranslation(new Vector3(1.0f, 0.0f, 0.0f));
        GL.UniformMatrix4(location, true, ref transformation);
    }

    private void Dispose(bool disposing)
    {
        if (!_isDisposed)
        {
            GL.DeleteProgram(Handle);

            _isDisposed = true;
        }
    }
    ~Shader()
    {
        if (!_isDisposed)
        {
            Console.WriteLine("GPU Resource leak! Did you forget to call Dispose()?");
        }
    }


    public void Dispose()
    {
        Dispose(true);
    }
}