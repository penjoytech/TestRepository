using System.Collections.Generic;
using System.ComponentModel;

namespace CommonApplicationFramework.Common.DMS
{
    public class DocumentUploadModel : DocumentProperties
    {
        public string DocumentType { get; set; }// Required

        public string DocumentTypeCode { get; set; }// Required

        public int FileFormat { get; set; }// Required

        public string FileName { get; set; }// Required

        public string ActivetedFileName { get; set; } 

        public string File { get; set; }// Required

        public string FilePath { get; set; }

        public ObjectModel ObjectDetails { get; set; }

        public List<object> DocMetaData { get; set; } //MetaData

        public decimal? Width { get; set; }

        public decimal? Height { get; set; }

        [DefaultValue(false)]
        public bool IsOverride { get; set; }

        public ImageDocument imageDocument { get; set; }

        public ImageType imageType { get; set; }

        public List<ImageDocument> imageDocumentList { get; set; }

        public decimal? MinImageWidth { get; set; }

        public decimal? MinImageHeight { get; set; }

        public decimal? MaxImageWidth { get; set; }

        public decimal? MaxImageHeight { get; set; }

        public int FolderId { get; set; } // Required
        public string FolderCode { get; set; }// Required

        [DefaultValue(false)]
        public bool IsNewVersion { get; set; }

        [DefaultValue(false)]
        public bool IsNewEntry { get; set; }

        public string ReviewStatus { get; set; }
        public bool? AllowReviewersToReassignReview { get; set; }
        public bool? NotifyInitiatorWhenReviewComplete { get; set; }
        public bool? UseDefaultWorkflow { get; set; }
        public string ReviewerInstruction { get; set; }
    }

    public class ImageObjectDetails
    {
        public List<int> ObjectId { get; set; }
        public string DocumentTypeCode { get; set; }
        public string ObjectType { get; set; }
        public int DocumentId { get; set; }
    }
}
