namespace CommonApplicationFramework.Common.DMS
{
    public class ImageType : BaseModel
    {
        public int ImageTypeId { get; set; }

        public string ImageTypeName { get; set; }

        public bool IsUserUploaded { get; set; }

        public decimal? Width { get; set; }

        public decimal? Height { get; set; }

        public int DocumentTypeId { get; set; }

        public int? ParentImageTypeId { get; set; }

        public int DocumentId { get; set; }

        public string DocumentTypeName { get; set; }

        public string DocumentTypeCode { get; set; }

        public string ImageFile { get; set; }
    }
}
