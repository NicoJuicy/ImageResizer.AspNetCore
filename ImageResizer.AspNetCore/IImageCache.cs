namespace Sapico.ImageResizer
{
    public interface IImageCache
    {
        bool TryGet(long key, out byte[] imageBytes);
        void Set(long key, byte[] imageBytes);
    }
}
