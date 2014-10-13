using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfInsertBigData
{
    public partial class TP_ZPRACOVANE
    {
        [Key]
        [Column(Order = 0, TypeName = "numeric")]
        public decimal ID { get; set; }

        [Column(Order = 1)]
        public DateTime DATUMSTART { get; set; }

        public DateTime? DATUMEND { get; set; }
        
        [Column(TypeName = "image")]
        public byte[] ZMENYXMLCOMP { get; set; }
    }
}
