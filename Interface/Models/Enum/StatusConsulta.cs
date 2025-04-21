using System;
using System.ComponentModel;

namespace Interface.Models.Enum
{
    public enum StatusConsulta
    {
        [Description("Aguardando confirmação do médico")]
        AguardandoAutorizacao = 1,

        [Description("Autorizado")]
        Autorizado = 2,

        [Description("Cancelada paciente")]
        CanceladoPaciente = 3,

        [Description("Cancelada médico")]
        CanceladoMedico = 4,

        [Description("Realizada")]
        Realizado = 5
    }

    public static class StatusConsultaHelper
    {
        public static string GetEnumDescription(StatusConsulta value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
            return attribute?.Description ?? value.ToString();
        }
    }
}
