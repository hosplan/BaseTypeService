using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BaseTypeService.Model
{
    public class BaseRootType
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }

        public IEnumerable<BaseBranchType> BaseBrachTypes { get; set; }
    }
}
