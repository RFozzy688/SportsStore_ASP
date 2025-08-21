using Microsoft.EntityFrameworkCore;

namespace SportsStore.Models
{
    public class EFOrderRepository : IOrderRepository
    {
        private ApplicationDbContext context;

        public EFOrderRepository( ApplicationDbContext ctx )
        {
            context = ctx;
        }

        public IQueryable<Order> Orders => context.Orders
            .Include(o => o.Lines)
            .ThenInclude(l => l.Product);

        //public void SaveOrder( Order order )
        //{
        //    context.AttachRange(order.Lines.Select(l => l.Product));
        //    if ( order.OrderID == 0 )
        //    {
        //        context.Orders.Add(order);
        //    }
        //    context.SaveChanges();
        //}

        public void SaveOrder( Order order )
        {
            // Убедимся, что продукты в заказе не дублируются в контексте
            foreach ( var line in order.Lines )
            {
                var productId = line.Product.ProductID;
                var local = context.Products.Local.FirstOrDefault(p => p.ProductID == productId);
                if ( local != null )
                {
                    // Используем локальный экземпляр
                    line.Product = local;
                }
                else
                {
                    // Если продукта нет в Local, привязываем его без пометки Modified
                    context.Products.Attach(line.Product);
                    // при необходимости можно пометить состояние как Unchanged/Modified
                    // context.Entry(line.Product).State = EntityState.Unchanged;
                }
            }

            if ( order.OrderID == 0 )
            {
                context.Orders.Add(order);
            }

            context.SaveChanges();

        }
    }
}
