using Restaurante.Web.Data.Enums;

namespace Restaurante.Web.Core.Extensions
{
    public static class OrderStatusExtensions
    {
        public static string ToDisplayName(this OrderStatus status) => status switch
        {
            OrderStatus.Pending => "Pendiente",
            OrderStatus.InProgress => "En preparación",
            OrderStatus.Served => "Servido",
            OrderStatus.Paid => "Pagado",
            OrderStatus.Cancelled => "Cancelado",
            _ => status.ToString()
        };

        public static string ToBadgeClass(this OrderStatus status) => status switch
        {
            OrderStatus.Pending => "bg-warning text-dark",
            OrderStatus.InProgress => "bg-info text-dark",
            OrderStatus.Served => "bg-primary",
            OrderStatus.Paid => "bg-success",
            OrderStatus.Cancelled => "bg-secondary",
            _ => "bg-light text-dark"
        };
    }
}