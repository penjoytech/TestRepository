namespace CommonApplicationFramework.Common.DMS
{
    public class ImageDocument
    {
        public int Id { get; set; }

        public int DocumentId { get; set; }

        public string ImageName { get; set; }

        public int ImageTypeId { get; set; }

        public int DocumentVersionId { get; set; }

        public decimal? Width { get; set; }

        public decimal? Height { get; set; }

        public string FilePath { get; set; }

        public string ImageTypeName { get; set; }
    }
}
