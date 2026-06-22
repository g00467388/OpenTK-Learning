
using OpenTK.Graphics.OpenGL4;
using StbImageSharp;

namespace gl;
// simplify reading images from Resources/*
public class Texture
{
    private readonly string _path;
    private int Handle { get; set; }

    public Texture(string path)
    {
        Handle = GL.GenTexture();
        _path = path;
    }

    public void Use()
    {
        StbImage.stbi_set_flip_vertically_on_load(1);
        ImageResult image = ImageResult.FromStream(File.OpenRead(_path), ColorComponents.RedGreenBlueAlpha);
        // upload texture data 
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);
        GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
    }
}