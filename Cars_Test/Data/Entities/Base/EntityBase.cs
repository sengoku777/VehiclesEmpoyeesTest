using System.ComponentModel.DataAnnotations;

namespace Cars_Test.Data.Entities.Base
{
    public class EntityBase
    {

        [Key]
        public int Id { get; set; }

        public virtual void Deatach()
        {
            this.Id = 0;
        }
    }
}
