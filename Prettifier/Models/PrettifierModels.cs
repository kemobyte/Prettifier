using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace Prettifier.Models
{
    public class PrettifiedNumbers
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [Display(Name = "Orginal Number")]
        public string OrginalNumber { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Prettified Number")]
        public string PrettifiedNumber { get; set; }

        [Required]
        [Display(Name = "Created In")]
        public DateTime CreatedDate { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Category")]
        public string PrettifiedCategory { get; set; }

        [Required] [StringLength(128)]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }

}