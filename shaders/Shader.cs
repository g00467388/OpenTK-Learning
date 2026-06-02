using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
public class Shader
{
    private int _handle;
    private string _vertexPath;
    private string _fragmentPath;
    private int _vertexShader; 
    private int _fragmentShader;
    
    public Shader(string vertexPath, string fragmentPath)
    {
        _vertexPath = (vertexPath);
        _fragmentPath = (fragmentPath);
        Compile();
    }

    public void Compile()
    {
        var vertexShaderSource =  File.ReadAllText(_vertexPath);
        var fragmentShaderSource =  File.ReadAllText(_fragmentPath);
        GL.CreateShader(ShaderType.VertexShader);
//        var x  = GL.CreateShader(ShaderType.VertexShader);        
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
        _handle = GL.CreateProgram();

        GL.AttachShader(_handle, _vertexShader);
        GL.AttachShader(_handle, _fragmentShader);

        GL.LinkProgram(_handle);

        GL.GetProgram(_handle, GetProgramParameterName.LinkStatus, out int programStatus);
        if (programStatus == 0)
        {
            string infoLog = GL.GetProgramInfoLog(_handle);
            Console.WriteLine(infoLog);
        }
        GL.DetachShader(_handle, _vertexShader);
        GL.DetachShader(_handle, _fragmentShader);
        GL.DeleteShader(_fragmentShader);
        GL.DeleteShader(_vertexShader);

    }

    public void Use()
    {
        GL.UseProgram(_handle);
    }
 

    private bool disposedValue = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            GL.DeleteProgram(_handle);

            disposedValue = true;
        }
    }

    ~Shader()
    {
        if (disposedValue == false)
        {
            Console.WriteLine("GPU Resource leak! Did you forget to call Dispose()?");
        }
    }


    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

}