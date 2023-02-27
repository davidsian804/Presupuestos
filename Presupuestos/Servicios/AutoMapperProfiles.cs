using AutoMapper;
using Presupuestos.Models;

namespace Presupuestos.Servicios
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Cuenta, CuentaCreacionViewModel>();
        }
    }
}
