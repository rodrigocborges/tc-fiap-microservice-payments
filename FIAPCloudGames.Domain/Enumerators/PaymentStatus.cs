using System.ComponentModel;

namespace FIAPCloudGames.Domain.Enumerators
{
    public enum PaymentStatus
    {
        [Description("Pendente")]
        Pending = 0,
        [Description("Em Análise")]
        Checking = 1,
        [Description("Aprovado")]
        Approved = 2,
        [Description("Cancelado")]
        Canceled = 3,
        [Description("Reprovado")]
        Repproved = 4
    }
}
