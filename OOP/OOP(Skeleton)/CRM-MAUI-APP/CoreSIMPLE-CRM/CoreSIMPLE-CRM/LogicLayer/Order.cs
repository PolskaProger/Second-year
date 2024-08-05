using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreSIMPLECRM.LogicLayer
{
    public class Order
    {
        public string CustomerName { get; set; }
        public string Contact { get; set; }
        public List<Position> PositionsInOrder { get; set; }
        public float TotalCost { get; set; }
        public DateTime Date { get; set; }
        public string Id { get; set; }

        public static List<Order> orders = new List<Order>();
        public Order CreateOrder(string customerName, string contact, DateTime dateOfOrder, List<Position> positions)
        {
            var totalCost = 0f;
            foreach (var position in positions)
            {
                totalCost += position.Cost*position.Quantity;
            }

            var newOrder = new Order
            {
                CustomerName = customerName,
                Contact = contact,
                PositionsInOrder = positions,
                TotalCost = totalCost,
                Date = dateOfOrder,
                Id = Guid.NewGuid().ToString()
            };
            orders.Add(newOrder);
            return newOrder;
            
        }

        public Order EditOrder(string Id, string newCustomerName, string newContact, List<Position> newPositions)
        {
            // Assuming 'orders' is a list of Order objects
            var order = orders.Find(o => o.Id == Id);
            if (order != null)
            {
                order.CustomerName = newCustomerName;
                order.Contact = newContact;
                order.PositionsInOrder = newPositions;
                order.TotalCost = 0;
                foreach (var position in newPositions)
                {
                    order.TotalCost += position.Cost*position.Quantity;
                }
                return order;
            }
            else
            {
                return null;
            }
        }

        public bool DeleteOrder(string Id)
        {
            var orderToDel = orders.Find(o => o.Id == Id);
            if (orderToDel != null)
            {
                orders.Remove(orderToDel);
                return true;
            }
            else
            {
                return false;
            }
        }
        public List<Position> GetAllPosOfOrder(string Id)
        {
            var order = orders.Find(o => o.Id == Id);
            return order.PositionsInOrder;
        }
        public static List<Order> GetAllOrders()
        {
            return orders;
        }

        public void AddOrderToList(Order order)
        {
            orders.Add(order);
        }
          
    }

}
