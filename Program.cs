using OpenTK.Windowing.Desktop;

namespace gl;

public class Program {
    public static void Main(string[] args)
    {
        var game = new Game(new GameWindowSettings(), new NativeWindowSettings()); 
        game.Run();
    }
}