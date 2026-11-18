using OpenTK.Graphics.OpenGL4;
using StbImageSharp;
using System.IO;
using System.Reflection.Metadata;

namespace Kralt.Framework.Graphics
{
    public class Texture
    {
        internal int Handle { get; }

        internal Texture(int glHandle)
        {
            Handle = glHandle;
        }

        public static Texture Load(string path)
        {
            int texh = GL.GenTexture();

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, texh);

            StbImage.stbi_set_flip_vertically_on_load(1);

            using(Stream stream = File.OpenRead(path))
            {
                ImageResult img = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, img.Width, img.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, img.Data);
            }

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            // OpenGL will automatically switch between mipmaps when an object gets sufficiently far away.
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            return new Texture(texh);
        }

        public void Use()
        {
            GL.BindTexture(TextureTarget.Texture2D, Handle);
        }
    }
}
