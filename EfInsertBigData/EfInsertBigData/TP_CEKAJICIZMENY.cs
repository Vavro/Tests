namespace EfInsertBigData
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TP_CEKAJICIZMENY
    {
        [Key]
        [Column(Order = 0, TypeName = "numeric")]
        public decimal ID { get; set; }

        [Key]
        [Column(Order = 1)]
        public DateTime DATUMSTART { get; set; }

        public DateTime? DATUMEND { get; set; }

        [Column(TypeName = "ntext")]
        public string ZMENYXML { get; set; }
    }
}
