namespace SnapsLibrary
{
    public struct SnapsImage
    {
        public Windows.Storage.StorageFile File;

        public SnapsImage(Windows.Storage.StorageFile file)
        {
            File = file;
        }
    }
}
