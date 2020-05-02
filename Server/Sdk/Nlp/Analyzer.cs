using EP.Morph;
using EP.Ner;
using EP.Ner.Goods;
using System.Collections.Generic;

namespace Sdk.Nlp
{
    public static class Analyzer
    {
        public static void Initialize()
        {
            ProcessorService.Initialize(MorphLang.RU);

            EP.Ner.Money.MoneyAnalyzer.Initialize();
            EP.Ner.Uri.UriAnalyzer.Initialize();
            EP.Ner.Phone.PhoneAnalyzer.Initialize();
            EP.Ner.Date.DateAnalyzer.Initialize();
            EP.Ner.Keyword.KeywordAnalyzer.Initialize();
            EP.Ner.Definition.DefinitionAnalyzer.Initialize();
            EP.Ner.Denomination.DenominationAnalyzer.Initialize();
            EP.Ner.Measure.MeasureAnalyzer.Initialize();
            EP.Ner.Bank.BankAnalyzer.Initialize();
            EP.Ner.Geo.GeoAnalyzer.Initialize();
            EP.Ner.Address.AddressAnalyzer.Initialize();
            EP.Ner.Org.OrganizationAnalyzer.Initialize();
            EP.Ner.Person.PersonAnalyzer.Initialize();
            EP.Ner.Mail.MailAnalyzer.Initialize();
            EP.Ner.Transport.TransportAnalyzer.Initialize();
            EP.Ner.Decree.DecreeAnalyzer.Initialize();
            EP.Ner.Instrument.InstrumentAnalyzer.Initialize();
            EP.Ner.Titlepage.TitlePageAnalyzer.Initialize();
            EP.Ner.Booklink.BookLinkAnalyzer.Initialize();
            EP.Ner.Business.BusinessAnalyzer.Initialize();
            EP.Ner.Goods.GoodsAnalyzer.Initialize();
            EP.Ner.Goods.GoodsAttrAnalyzer.Initialize();
            EP.Ner.Named.NamedEntityAnalyzer.Initialize();
            EP.Ner.Weapon.WeaponAnalyzer.Initialize();
        }

        public static List<string> ParseGoodList(IEnumerable<string> collection)
        {
            var resultList = new List<string>();

            foreach (var item in collection)
            {
                using (var proc = ProcessorService.CreateSpecificProcessor("GOODS, GOODATTR"))
                {
                    var ar = proc.Process(new SourceOfAnalysis(item));

                    foreach (var e in ar.Entities)
                    {
                        try
                        {
                            var value = e.Slots[1].Value.ToString().ToLower();
                            if (e is GoodAttributeReferent && e.Slots[0].Value.Equals("KEYWORD") && !resultList.Contains(value))
                                resultList.Add(value);
                        }
                        catch
                        {
                            // ignored
                        }
                    }
                }
            }

            return resultList;
        }
    }
}
