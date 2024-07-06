namespace GoldShop.Comman;

public static class Helper
{
    public static double GetPrice(this double goldPrice, double weight, double wages)
    {
        var gold = weight * goldPrice;
        var wagesGold = wages * weight;
        var profit = (gold + wagesGold) * 7 / 100;
        var tax = (wagesGold + profit) * 9 / 100;
        return gold + wagesGold + tax + profit;
    }
}