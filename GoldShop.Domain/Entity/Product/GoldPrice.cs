using GoldShop.Domain.Common;
using GoldShop.Domain.Enums;

namespace GoldShop.Domain.Entity.Product;

public class GoldPrice : BaseEntity
{
    public long PricePerGram { get; set; }
    public long PriceApi { get; set; }
    public string GenderType { get; set; } = string.Empty; //نوع جنس
    public PriceType PriceType { get; set; }
    public DateTime UpdateTime { get; set; }
    
    public int YekGram18 { get; set; }
    public int YekMesghal18 { get; set; }
    public int SekehRob { get; set; }
    public int SekehNim { get; set; }
    public int SekehEmam { get; set; }
    public int SekehTamam { get; set; }
    public int SekehGerami { get; set; }
    public int OunceTala { get; set; }
    public int YekMesghal17 { get; set; }
    public int OunceNoghreh { get; set; }
    public int Pelatin { get; set; }
    public int Dollar { get; set; }
    public int Euro { get; set; }
    public int Derham { get; set; }
    public int YekGram20 { get; set; }
    public int YekGram21 { get; set; }
    public string TimeRead { get; set; }
}