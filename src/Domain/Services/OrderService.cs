using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Domain.Services;

using Domain.Entities;

public static class OrderService
{
    private static readonly List<Order> _lastOrders = new();
    public static IReadOnlyList<Order> LastOrders => _lastOrders;
    public static Order CreateTerribleOrder(string customer, string product, int qty, decimal price)
    {
        var o = new Order { Id = RandomNumberGenerator.GetInt32(1, 9999999), CustomerName = customer, ProductName = product, Quantity = qty, UnitPrice = price };
        _lastOrders.Add(o);
        return o;
    }
}
