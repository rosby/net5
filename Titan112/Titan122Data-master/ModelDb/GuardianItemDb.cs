using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace Titan112Data
{
    public class GuardianItemDb : NsgDataItem
    {
        public string Id { get; set; }
        [MaxLength(50)]
        public string FirstName { get; set; }
        [MaxLength(50)]
        public string SecondName { get; set; }
        [MaxLength(50)]
        public string LastName { get; set; }
        public virtual ImageDb Photo { get; set; }
        public DateTime LastChanges { get; set; }

        public static GuardianItemDb findById(TitanDBContext db, String id)
        {
            return db.Guardians.Find(id);
        }
        public GuardianItemDb findById(TitanDBContext db)
        {
            return db.Guardians.Find(Id);
        }

        public void SavePhoto(TitanDBContext db, Bitmap newBM)
        {
            ImageDb image;
            if (Photo == null)
            {
                image = new ImageDb();
                image.Id = Guid.NewGuid();
                db.Add(image);
            }
            else image = Photo;
            image.LastChanges = DateTime.Now;
            image.Image = Service.ImageToByteArray(newBM);
            Photo = image;
            db.SaveChanges();
        }

        public override List<NsgDataItem> GetDBObjects(TitanDBContext db)
        {
            List<NsgDataItem> list = db.Guardians.ToList<NsgDataItem>();
            return list;
        }

    }
}
