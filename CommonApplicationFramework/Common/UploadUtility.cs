using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonApplicationFramework.Common
{
  public class UploadUtility
    {
        public static string UploadImage(string imagepath, AttachmentFile image)
        {
            byte[] imageBytes = Convert.FromBase64String(image.File);
            FileInfo fi = new FileInfo(image.FileName);
            string OriFileName = Guid.NewGuid() + fi.Extension;
            var path = Path.Combine(imagepath, Path.GetFileName(OriFileName));
            if (!Directory.Exists(imagepath))
            {
                Directory.CreateDirectory(imagepath);
            }
            System.IO.File.WriteAllBytes(path, imageBytes);
            return OriFileName;
        }

        public static string UploadImageIntranet(string imagepath, AttachmentFile image, string OriFileName)
        {
            byte[] imageBytes = Convert.FromBase64String(image.File);
            //FileInfo fi = new FileInfo(image.FileName);
            //string OriFileName = Guid.NewGuid() + fi.Extension;
            var path = Path.Combine(imagepath, Path.GetFileName(OriFileName));
            if (!Directory.Exists(imagepath))
            {
                Directory.CreateDirectory(imagepath);
            }
            System.IO.File.WriteAllBytes(path, imageBytes);
            return OriFileName;
        }

        public static Item GetValueItem(string id, string Value)
        {
            Item item = new Item();
            if (!string.IsNullOrEmpty(id))
            {
                item.Id = Convert.ToInt32(id);
                item.Value = Value;
            }
            else
            {
                item.Id = 0;
                item.Value = null;
            }
            return item;
        }

    }
}
