using Restaurante.Web.Data.Enums;

namespace Restaurante.Web.Core.Extensions
{
    public static class PaymentMethodExtensions
    {
        public static string ToDisplayName(this PaymentMethod method) => method switch
        {
            PaymentMethod.Cash => "Efectivo",
            PaymentMethod.Card => "Tarjeta",
            PaymentMethod.Transfer => "Transferencia",
            _ => method.ToString()
        };
    }
}