using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models { 
    [Table("repair")]
    public class Repair
    {
        [Key]
        public string Rtel { get; set; } = null!;

        public string Rdate1 { get; set; } = null!;

        public string Rtime { get; set; } = null!;

        public string? Rno { get; set; }

        public string? Rprodno { get; set; }

        public string? Rproduct { get; set; }

        public string? Rnano { get; set; }

        public string? Rna { get; set; }

        public string? Rmode { get; set; }

        public string? Rid { get; set; }

        public string? Rerr1 { get; set; }

        public string? Rerr2 { get; set; }

        public string? Rerr3 { get; set; }

        public decimal Rearn { get; set; }

        public decimal Rcost { get; set; }

        public decimal Rprofit { get; set; }

        public string? Runitno { get; set; }

        public string? Runit { get; set; }

        public string? Rmanno { get; set; }

        public string? Rman { get; set; }

        public string? Rsno { get; set; }

        public string? Rstate { get; set; }

        public string Rdate2 { get; set; } = null!;

        public string? Rinout { get; set; }

        public string? Rmemo { get; set; }

        public string? Ryear1 { get; set; }

        public string? Ryear2 { get; set; }
    }
}