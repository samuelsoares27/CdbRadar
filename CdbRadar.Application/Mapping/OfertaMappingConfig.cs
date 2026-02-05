using CdbRadar.Application.DTOs;
using CdbRadar.Domain.Model;
using Mapster;
using System;
using System.Collections.Generic;
using System.Text;

namespace CdbRadar.Application.Mapping
{
    public static class OfertaMappingConfig
    {
        public static void Register()
        {
            TypeAdapterConfig<CdbOfertasDto, CdbOfertas>
                .NewConfig()
                .Ignore(dest => dest.Id)
                .Ignore(dest => dest.DataColeta)
                .Map(dest => dest.Liquidez,
                     src => "Não informado")
                .Map(dest => dest.Mercado,
                     src => "Secundário")
                .Map(dest => dest.Plataforma,
                     src => "BTG")
                .Map(dest => dest.DataColeta,
                     src => DateTime.Now);
        }
    }
}
