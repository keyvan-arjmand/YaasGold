﻿namespace GoldShop.Comman;

public static class Helper
{
    public static double GetPrice(this double goldPrice, double weight, double fixedWages, double percentageWages)
    {
        // محاسبه قیمت طلای خام
        var gold = weight * goldPrice;

        // محاسبه اجرت ریالی (ثابت)
        var fixedWagesGold = fixedWages * weight;

        // محاسبه اجرت درصدی
        var percentageWagesGold = (percentageWages / 100) * gold;

        // جمع اجرت ریالی و اجرت درصدی
        var totalWages = fixedWagesGold + percentageWagesGold;

        // محاسبه سود طلافروش
        var profit = (gold + totalWages) * 7 / 100;

        // محاسبه مالیات بر ارزش افزوده
        var tax = (gold + totalWages + profit) * 9 / 100;

        // محاسبه قیمت نهایی
        return gold + totalWages + profit + tax;
    }

}