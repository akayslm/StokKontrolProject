using StokKontrolProject.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokKontrolProject.Entities.Entities
{
    public class Order : BaseEntity
    {
        public Order()
        {
            SiparisDetaylari = new List<OrderDetails>();
        }
        [ForeignKey("Kullanici")]
        public int UserID { get; set; }
        public Status Status { get; set; }
        public virtual User? Kullanici { get; set; }
        public virtual List<OrderDetails> SiparisDetaylari { get; set; }
    }
}
