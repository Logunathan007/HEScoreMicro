using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEScoreMicro.Domain.Entity
{
    public interface IHasId
    {
        public Guid Id { get; set; }
    }
    public interface IHasBuildingId
    {
        public Guid BuildingId { get; set; }
    }
}
